using System;
using System.Collections;
using UnityEngine;

public class MiningTaskHandler : ITaskHandler
{
    public IEnumerator ExecuteTask(UP_Worker worker, Task task, Action onComplete)
    {
        MiningTask miningTask = (MiningTask)task;
        
        int currentMine = 0;
        // Debug.Log($"Need mine {miningTask.resourceAmount}");

        while (currentMine < miningTask.resourceAmount)
        {
            // Move to resource
            // Debug.Log($"Move to {miningTask.resource}");
            Vector3 resourcePosition = miningTask.resourcePosition;

            yield return worker.movement.MoveTo(resourcePosition, UnitMovement.E_MoveTo.PlacedObject);

            // Mining phase
            while (worker.GetCurrentAmount() < worker.GetMaxAmount())
            {
                worker.AddCurrentAmount(1); // Можно сделать параметр скорости добычи
                yield return new WaitForSeconds(0.5f); // Эмуляция добычи
            }

            // Debug.Log($"Mined {worker.GetCurrentAmount()} resources, moving to processor");
            currentMine += worker.GetCurrentAmount();

            // Move to processor
            IProcessor processor = ProcessorSystem.current.GetNearbyProcessor(worker.transform.position, miningTask.resource);
            yield return worker.movement.MoveTo(processor.GetPosition(), UnitMovement.E_MoveTo.PlacedObject);

            // Store resources
            // Debug.Log($"Storing {worker.GetCurrentAmount()} resources in {processor}");
            processor.AddResources(worker.GetCurrentAmount());
            worker.ClearCurrentAmount();
        }

        // complete
        onComplete?.Invoke();
    }
}