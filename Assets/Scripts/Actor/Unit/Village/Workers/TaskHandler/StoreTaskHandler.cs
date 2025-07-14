using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoreTaskHandler : ITaskHandler
{
    private StoreTask storeTask;

    public IEnumerator ExecuteTask(UV_Worker worker, Task task, Action onComplete)
    {
        storeTask = (StoreTask)task;

        IProcessor processor = storeTask.source
            .gameObject
            .GetComponents<MonoBehaviour>()
            .OfType<IProcessor>()
            .FirstOrDefault(x => ((IIndustrial)x).GetTypeResource() == storeTask.resource);

        IProduction production = storeTask.source
            .gameObject
            .GetComponents<MonoBehaviour>()
            .OfType<IProduction>()
            .FirstOrDefault(x => ((IIndustrial)x).GetTypeResource() == storeTask.resource);


        foreach (var component in storeTask.source.GetComponents<MonoBehaviour>())
        {
            if (component is IProduction)
            {
                IIndustrial industrial = (IIndustrial)component;
                if (industrial.GetTypeResource() == storeTask.resource)
                {
                    production = (IProduction)storeTask.source;
                    break;
                }
            }
        }

        if (processor != null)
        {
            // check have store resources
            if (worker.GetCurrentStoreAmount() > 0)
            {
                yield return StoreToStorage(worker);
            }

            storeTask.SetExecutingState(EStoreExecutingState.MoveToSource);

            // move to processor
            IPosition position = (IPosition)processor;

            yield return Moving(worker, position, UnitMovement.E_MoveTo.PlacedObject);

            storeTask.SetProgress((storeTask.goal - storeTask.progress) / 2);

            // store to storage
            worker.AddCurrentCarryingAmount(storeTask.resource, processor.Unload());
            storeTask.SetAmount(worker.GetCurrentCarryingAmountByResource(storeTask.resource));
            yield return StoreToStorage(worker);
        }
        else if (production != null)
        {
            // check have mine resources
            if (!worker.CheckFreeSpaceMineAmount())
            {
                yield return StoreToProcessor(worker);
            }

            storeTask.SetExecutingState(EStoreExecutingState.MoveToSource);

            // move to production
            IPosition position = (IPosition)storeTask.source;

            yield return Moving(worker, position, UnitMovement.E_MoveTo.PlacedObject);

            storeTask.SetProgress((storeTask.goal - storeTask.progress) / 2);

            // store to processor
            worker.AddCurrentMineAmount(storeTask.resource, production.Unload(worker.GetMaxMineAmount()));
            storeTask.SetAmount(worker.GetCurrentCarryingAmountByResource(storeTask.resource));
            yield return StoreToProcessor(worker);
        }

        storeTask.SetExecutingState(EStoreExecutingState.none);
        storeTask.SetProgress(storeTask.goal);

        onComplete?.Invoke();
    }

    private IEnumerator StoreToProcessor(UV_Worker worker)
    {
        foreach (E_Resource resource in Enum.GetValues(typeof(E_Resource)))
        {
            if (worker.GetCurrentMineAmountByResource(resource) < 1) continue;

            IProcessor processor = ProcessorSystem.current.GetNearbyProcessor(worker.transform.position, resource);
            IPosition position = (IPosition)processor;

            if (processor is IModule)
            {
                storeTask.SetStorage(((IModule)processor).GetBuilding());
            }
            else
            {
                storeTask.SetStorage((Building)processor);
            }

            storeTask.SetExecutingState(EStoreExecutingState.MoveToStorage);

            yield return Moving(worker, position, UnitMovement.E_MoveTo.PlacedObject);

            processor.AddResources(worker.GetCurrentMineAmountByResource(resource));
            worker.ClearCurrentMineAmount(resource);
        }
    }

    private IEnumerator StoreToStorage(UV_Worker worker)
    {
        foreach (E_Resource resource in Enum.GetValues(typeof(E_Resource)))
        {
            if (worker.GetCurrentCarryingAmountByResource(resource) < 1) continue;

            int residue = 0;
            do
            {
                if (!StorageSystem.current.CheckFreeSpace(resource))
                {
                    MessageSystem.current.AddMessage($"Not enough space to store {residue} {resource}");
                    break;
                }

                IStorage storage = StorageSystem.current.FindNearbyStorage(resource, worker.transform.position);
                IPosition position = (IPosition)storage;

                if (storage is IModule)
                {
                    storeTask.SetStorage(((IModule)storage).GetBuilding());
                }
                else
                {
                    storeTask.SetStorage((Building)storage);
                }

                storeTask.SetExecutingState(EStoreExecutingState.MoveToStorage);

                yield return Moving(worker, position, UnitMovement.E_MoveTo.PlacedObject);

                residue = ResourceSystem.current.AddResourceByType(storage, resource, worker.GetCurrentCarryingAmountByResource(resource));

                if (residue > 0)
                {
                    storeTask.SetProgress((storeTask.goal - storeTask.progress) / 2);
                    worker.RemoveCurrentCarryingAmount(resource, worker.GetCurrentCarryingAmountByResource(resource) - residue);
                }
                else
                {
                    worker.ClearCurrentCarryingAmount(resource);
                }
            }
            while (residue > 0);
        }
    }
    
    public IEnumerator Moving(UV_Worker worker, IPosition iPosition, UnitMovement.E_MoveTo to)
    {
        int countAttemps = 0;

        worker.animator.SetBool("isMove", true);

        while (true)
        {
            yield return worker.movement.MoveTo(iPosition, to);

            if (!worker.movement.isCanMove)
            {
                countAttemps++;

                if (countAttemps >= 5)
                {
                    MessageSystem.current.AddMessage($"Worker {worker.nameActor} canceled the task: {worker.tasks[0].nameTask} due to сan't reach the object");
                    worker.CancelTask(worker.tasks[0]);
                    break;
                }
            }
            else
            {
                break;
            }
        }

        worker.animator.SetBool("isMove", false);
    }
}