using System;
using System.Collections;
using UnityEngine;

public class StoreTaskHandler : ITaskHandler
{
    public IEnumerator ExecuteTask(UP_Worker worker, Task task, Action onComplete)
    {
        StoreTask storeTask = (StoreTask)task;

        if(storeTask.processor != null)
        {
            // check have store resources
            if(worker.GetCurrentStoreAmount() > 0)
            {
                yield return StoreToStorage(worker);
            }
            
            // move to processor
            IPosition position = (IPosition)storeTask.processor;
            
            worker.animator.SetBool("isMove", true);
            yield return worker.movement.MoveTo(position.GetPosition(), UnitMovement.E_MoveTo.PlacedObject);
            worker.animator.SetBool("isMove", false);

            // store to storage
            worker.AddCurrentStoreAmount(storeTask.resource, storeTask.processor.Unload());
            yield return StoreToStorage(worker);
            
        }
        else if(storeTask.production != null)
        {
            // check have mine resources
            if(!worker.CheckFreeSpaceMineAmount())
            {
                yield return StoreToProcessor(worker);
            }
            
            // move to production
            IPosition position = (IPosition)storeTask.production;

            worker.animator.SetBool("isMove", true);
            yield return worker.movement.MoveTo(position.GetPosition(), UnitMovement.E_MoveTo.PlacedObject);
            worker.animator.SetBool("isMove", false);

            // store to processor
            worker.AddCurrentMineAmount(storeTask.resource, storeTask.production.Unload(worker.GetMaxMineAmount()));
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

            worker.animator.SetBool("isMove", true);
            yield return worker.movement.MoveTo(position.GetPosition(), UnitMovement.E_MoveTo.PlacedObject);
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

                worker.animator.SetBool("isMove", true);
                yield return worker.movement.MoveTo(position.GetPosition(), UnitMovement.E_MoveTo.PlacedObject);
                worker.animator.SetBool("isMove", false);

                residue = ResourceSystem.current.AddResources(storage, resource, worker.GetCurrentStoreAmountByResource(resource));
            }
            while(residue > 0);
            worker.ClearCurrentStoreAmount(resource);
        }
    }
}