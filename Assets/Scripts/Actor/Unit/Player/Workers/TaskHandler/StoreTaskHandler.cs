using System;
using System.Collections;

public class StoreTaskHandler : ITaskHandler
{
    public IEnumerator ExecuteTask(UP_Worker worker, Task task, Action onComplete)
    {
        StoreTask storeTask = (StoreTask)task;

        // move to processor
        yield return worker.movement.MoveTo(storeTask.processor.GetPosition(), UnitMovement.E_MoveTo.PlacedObject);

        int processedAmout = storeTask.processor.Unload();        

        int residue = 0;
        do
        {
            IStorage storage = StorageSystem.current.FindNearbyStorage(storeTask.resource, worker.transform.position);
            yield return worker.movement.MoveTo(storage.GetPosition(), UnitMovement.E_MoveTo.PlacedObject);

            residue = storage.AddResources(processedAmout);
        }
        while(residue > 0);

        onComplete?.Invoke();
    }
}