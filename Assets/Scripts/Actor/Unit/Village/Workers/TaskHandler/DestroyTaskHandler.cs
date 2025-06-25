using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTaskHandler : ITaskHandler
{
    public IEnumerator ExecuteTask(UV_Worker worker, Task task, Action onComplete)
    {
        DestroyTask destroyTask = (DestroyTask)task;

        destroyTask.SetExecutingState(EDestroyExecutingState.MoveToBuilding);

        // move to building
        yield return Moving(worker, destroyTask.building, UnitMovement.E_MoveTo.PlacedObject);

        destroyTask.SetExecutingState(EDestroyExecutingState.Destruction);

        // destruction
        worker.animator.SetTrigger("Build");
        worker.ActiceInsturcent(UV_Worker.E_Instrument.Hammer);
        float elapsedTime = 0f;
        while (elapsedTime < destroyTask.timeToDestroy)
        {
            elapsedTime += Time.deltaTime;

            float progress = Time.deltaTime / destroyTask.timeToDestroy * 100;
            destroyTask.SetProgress(progress);

            yield return null;
        }
        yield return new WaitForSeconds(worker.animator.GetCurrentAnimatorStateInfo(0).length);
        worker.DisactiveInstument(UV_Worker.E_Instrument.Hammer);
        worker.animator.Play("Idle");

        // destroy building
        destroyTask.building.Destroy();

        // return resources for building
        ResourceSystem.current.AddResources(destroyTask.buildingCost);

        destroyTask.SetExecutingState(EDestroyExecutingState.none);

        // complete
        onComplete?.Invoke();
    }
    
    public IEnumerator Moving(UV_Worker worker, IPosition iPosition, UnitMovement.E_MoveTo to)
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
                    Debug.Log("Дойти невозможно, задача отменяется!");
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