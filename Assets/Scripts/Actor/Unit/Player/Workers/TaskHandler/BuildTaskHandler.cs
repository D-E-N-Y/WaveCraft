using System;
using System.Collections;
using UnityEngine;

public class BuildTaskHandler : ITaskHandler
{
    public IEnumerator ExecuteTask(UP_Worker worker, Task task, Action onComplete)
    {
        BuildTask buildTask = (BuildTask)task;
        
        // move to building
        yield return worker.movement.MoveTo(buildTask.building.transform.position, UnitMovement.E_MoveTo.Object);

        Debug.Log("start build");

        // build
        float elapsedTime = 0f;
        while(elapsedTime < buildTask.building.GetTimeToBuild())
        {
            elapsedTime += Time.deltaTime;

            // Update UI

            // Debug.Log($"Building progress: {elapsedTime / task.timeToBuild * 100}%");

            yield return null;
        }
        buildTask.building.Built();
        
        // complete
        onComplete?.Invoke();
    }
}