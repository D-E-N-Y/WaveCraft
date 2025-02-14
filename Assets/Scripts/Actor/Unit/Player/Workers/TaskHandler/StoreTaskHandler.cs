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
            // move to processor
            IPosition position = (IPosition)storeTask.processor;
            
            worker.animator.SetBool("isMove", true);
            yield return worker.movement.MoveTo(position.GetPosition(), UnitMovement.E_MoveTo.PlacedObject);
            worker.animator.SetBool("isMove", false);

            int processedAmout = storeTask.processor.Unload();

            int residue = 0;
            do
            {
                if(!StorageSystem.current.CheckFreeSpace(storeTask.resource))
                {
                    Debug.Log($"not have free space for {residue} {storeTask.resource}");
                    break;
                }
                
                IStorage storage = StorageSystem.current.FindNearbyStorage(storeTask.resource, worker.transform.position);
                position = (IPosition)storage;

                worker.animator.SetBool("isMove", true);
                yield return worker.movement.MoveTo(position.GetPosition(), UnitMovement.E_MoveTo.PlacedObject);
                worker.animator.SetBool("isMove", false);

                residue = ResourceSystem.current.AddResources(storage, storeTask.resource, processedAmout);
            }
            while(residue > 0);
        }
        else if(storeTask.production != null)
        {
            // move to production
            IPosition position = (IPosition)storeTask.production;

            worker.animator.SetBool("isMove", true);
            yield return worker.movement.MoveTo(position.GetPosition(), UnitMovement.E_MoveTo.PlacedObject);
            worker.animator.SetBool("isMove", false);

            int produceAmout = storeTask.production.Unload();

            IProcessor processor = ProcessorSystem.current.GetNearbyProcessor(worker.transform.position, storeTask.resource);
            position = (IPosition)processor;

            worker.animator.SetBool("isMove", true);
            yield return worker.movement.MoveTo(position.GetPosition(), UnitMovement.E_MoveTo.PlacedObject);
            worker.animator.SetBool("isMove", false);

            processor.AddResources(produceAmout);
        }
        

        onComplete?.Invoke();
    }
}