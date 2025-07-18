using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTaskHandler : ITaskHandler
{
    public IEnumerator ExecuteTask(UV_Worker worker, Task task, Action onComplete)
    {
        BuildTask buildTask = (BuildTask)task;

        buildTask.SetExecutingState(EBuildExecutingState.MoveToBuilding);

        // move to building
        yield return Moving(worker, buildTask.building, E_MoveTo.PlacedObject);

        buildTask.SetExecutingState(EBuildExecutingState.Construstion); 

        // build
        worker.animator.SetTrigger("Build");
        worker.ActiceInsturcent(UV_Worker.E_Instrument.Hammer);
        float elapsedTime = 0f;
        while (elapsedTime < buildTask.building.GetTimeToBuild())
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
    
    public IEnumerator Moving(UV_Worker worker, IPosition iPosition, E_MoveTo to)
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