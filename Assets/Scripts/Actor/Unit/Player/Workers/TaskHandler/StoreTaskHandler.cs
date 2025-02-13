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
            worker.animator.SetBool("isMove", true);
            yield return worker.movement.MoveTo(storeTask.processor.GetPosition(), UnitMovement.E_MoveTo.PlacedObject);
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
                
                worker.animator.SetBool("isMove", true);
                yield return worker.movement.MoveTo(storage.GetPosition(), UnitMovement.E_MoveTo.PlacedObject);
                worker.animator.SetBool("isMove", false);

                residue = ResourceSystem.current.AddResources(storage, storeTask.resource, processedAmout);
            }
            while(residue > 0);
        }
        else if(storeTask.production != null)
        {
            // move to production
            worker.animator.SetBool("isMove", true);
            yield return worker.movement.MoveTo(storeTask.production.GetPosition(), UnitMovement.E_MoveTo.PlacedObject);
            worker.animator.SetBool("isMove", false);

            int produceAmout = storeTask.production.Unload();

            IProcessor processor = ProcessorSystem.current.GetNearbyProcessor(worker.transform.position, storeTask.resource);

            worker.animator.SetBool("isMove", true);
            yield return worker.movement.MoveTo(processor.GetPosition(), UnitMovement.E_MoveTo.PlacedObject);
            worker.animator.SetBool("isMove", false);

            processor.AddResources(produceAmout);
        }
        

        onComplete?.Invoke();
    }
}