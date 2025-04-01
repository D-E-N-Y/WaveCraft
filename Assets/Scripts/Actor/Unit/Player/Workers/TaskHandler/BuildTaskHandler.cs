using System;
using System.Collections;
using UnityEngine;

public class BuildTaskHandler : ITaskHandler
{
    public IEnumerator ExecuteTask(UP_Worker worker, Task task, Action onComplete)
    {
        BuildTask buildTask = (BuildTask)task;
        
        // move to building
        worker.animator.SetBool("isMove", true);
        yield return worker.movement.MoveTo(buildTask.building.GetPosition(), UnitMovement.E_MoveTo.Object);
        
        worker.animator.SetBool("isMove", false);

        // Debug.Log("start build");

        // build
        worker.animator.SetTrigger("Build");
        worker.ActiceInsturcent(UP_Worker.E_Instrument.Hammer);
        float elapsedTime = 0f;
        while(elapsedTime < buildTask.building.GetTimeToBuild())
        {
            elapsedTime += Time.deltaTime;

            // Update UI

            // Debug.Log($"Building progress: {elapsedTime / task.timeToBuild * 100}%");

            yield return null;
        }
        yield return new WaitForSeconds(worker.animator.GetCurrentAnimatorStateInfo(0).length);
        worker.DisactiveInstument(UP_Worker.E_Instrument.Hammer);
        worker.animator.Play("Idle");

        buildTask.building.Built();
        
        // complete
        onComplete?.Invoke();
    }
}