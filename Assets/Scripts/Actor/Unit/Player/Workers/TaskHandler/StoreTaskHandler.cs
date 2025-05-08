using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class StoreTaskHandler : ITaskHandler
{
    private StoreTask storeTask;

    public IEnumerator ExecuteTask(UP_Worker worker, Task task, Action onComplete)
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


        foreach(var component in storeTask.source.GetComponents<MonoBehaviour>())
        {
            if(component is IProduction)
            {
                IIndustrial industrial = (IIndustrial)component;
                if(industrial.GetTypeResource() == storeTask.resource)
                {
                    production = (IProduction)storeTask.source;
                    break;
                }
            }
        }

        if(processor != null)
        {
            // check have store resources
            if(worker.GetCurrentStoreAmount() > 0)
            {
                yield return StoreToStorage(worker);
            }
            
            // move to processor
            IPosition position = (IPosition)processor;
            
            worker.animator.SetBool("isMove", true);
            yield return worker.movement.MoveTo(position.GetPosition(), UnitMovement.E_MoveTo.Object);
            worker.animator.SetBool("isMove", false);

            // store to storage
            worker.AddCurrentStoreAmount(storeTask.resource, processor.Unload());
            yield return StoreToStorage(worker);
            
        }
        else if(production != null)
        {
            // check have mine resources
            if(!worker.CheckFreeSpaceMineAmount())
            {
                yield return StoreToProcessor(worker);
            }
            
            // move to production
            IPosition position = (IPosition)storeTask.source;

            worker.animator.SetBool("isMove", true);
            yield return worker.movement.MoveTo(position.GetPosition(), UnitMovement.E_MoveTo.Object);
            worker.animator.SetBool("isMove", false);

            // store to processor
            worker.AddCurrentMineAmount(storeTask.resource, production.Unload(worker.GetMaxMineAmount()));
            yield return StoreToProcessor(worker);
        }

        onComplete?.Invoke();
    }

    private IEnumerator StoreToProcessor(UP_Worker worker)
    {
        foreach(E_Resource resource in Enum.GetValues(typeof(E_Resource)))
        {
            if(worker.GetCurrentMineAmountByResource(resource) < 1) continue;
            
            IProcessor processor = ProcessorSystem.current.GetNearbyProcessor(worker.transform.position, resource);
            IPosition position = (IPosition)processor;

            if(processor is IModule)
            {
                storeTask.SetStorage(((IModule)processor).GetBuilding());
            }
            else
            {
                storeTask.SetStorage((Building)processor);
            }

            worker.animator.SetBool("isMove", true);
            yield return worker.movement.MoveTo(position.GetPosition(), UnitMovement.E_MoveTo.Object);
            worker.animator.SetBool("isMove", false);

            processor.AddResources(worker.GetCurrentMineAmountByResource(resource));
        }
    }

    private IEnumerator StoreToStorage(UP_Worker worker)
    {
        foreach(E_Resource resource in Enum.GetValues(typeof(E_Resource)))
        {
            if(worker.GetCurrentStoreAmountByResource(resource) < 1) continue;
            
            int residue = 0;
            do
            {
                if(!StorageSystem.current.CheckFreeSpace(resource))
                {
                    Debug.Log($"not have free space for {residue} {resource}");
                    break;
                }
                
                IStorage storage = StorageSystem.current.FindNearbyStorage(resource, worker.transform.position);
                IPosition position = (IPosition)storage;

                if(storage is IModule)
                {
                    storeTask.SetStorage(((IModule)storage).GetBuilding());
                }
                else
                {
                    storeTask.SetStorage((Building)storage);
                }

                worker.animator.SetBool("isMove", true);
                yield return worker.movement.MoveTo(position.GetPosition(), UnitMovement.E_MoveTo.Object);
                worker.animator.SetBool("isMove", false);

                residue = ResourceSystem.current.AddResources(storage, resource, worker.GetCurrentStoreAmountByResource(resource));
            }
            while(residue > 0);
            worker.ClearCurrentStoreAmount(resource);
        }
    }
}