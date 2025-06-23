using System;
using System.Collections;
using UnityEngine;

public class BuildTaskHandler : ITaskHandler
{
    public IEnumerator ExecuteTask(UV_Worker worker, Task task, Action onComplete)
    {
        BuildTask buildTask = (BuildTask)task;
        
        buildTask.SetExecutingState(EBuildExecutingState.MoveToBuilding);

        // move to building
        worker.animator.SetBool("isMove", true);
        yield return worker.movement.MoveTo(buildTask.building.GetPosition(), UnitMovement.E_MoveTo.PlacedObject);
        
        worker.animator.SetBool("isMove", false);

        buildTask.SetExecutingState(EBuildExecutingState.Construstion);

        // build
        worker.animator.SetTrigger("Build");
        worker.ActiceInsturcent(UV_Worker.E_Instrument.Hammer);
        float elapsedTime = 0f;
        while(elapsedTime < buildTask.building.GetTimeToBuild())
        {
            elapsedTime += Time.deltaTime;

            float progress = Time.deltaTime / buildTask.building.GetTimeToBuild() * 100;
            buildTask.SetProgress(progress);

            yield return null;
        }
        yield return new WaitForSeconds(worker.animator.GetCurrentAnimatorStateInfo(0).length);
        worker.DisactiveInstument(UV_Worker.E_Instrument.Hammer);
        worker.animator.Play("Idle");

        buildTask.building.Built();
        
        buildTask.SetExecutingState(EBuildExecutingState.none);

        // complete
        onComplete?.Invoke();
    }
}