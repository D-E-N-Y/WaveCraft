using System;
using System.Collections;
using UnityEngine;

public class DestroyTaskHandler : ITaskHandler
{
    public IEnumerator ExecuteTask(UP_Worker worker, Task task, Action onComplete)
    {
        DestroyTask destroyTask = (DestroyTask)task;
        
        destroyTask.SetExecutingState(EDestroyExecutingState.MoveToBuilding);
        
        // move to building
        worker.animator.SetBool("isMove", true);
        yield return worker.movement.MoveTo(destroyTask.building.GetPosition(), UnitMovement.E_MoveTo.PlacedObject);
        worker.animator.SetBool("isMove", false);

        destroyTask.SetExecutingState(EDestroyExecutingState.Destruction);

        // destruction
        worker.animator.SetTrigger("Build");
        worker.ActiceInsturcent(UP_Worker.E_Instrument.Hammer);
        float elapsedTime = 0f;
        while(elapsedTime < destroyTask.timeToDestroy)
        {
            elapsedTime += Time.deltaTime;

            float progress = Time.deltaTime / destroyTask.timeToDestroy * 100;
            destroyTask.SetProgress(progress);

            yield return null;
        }
        yield return new WaitForSeconds(worker.animator.GetCurrentAnimatorStateInfo(0).length);
        worker.DisactiveInstument(UP_Worker.E_Instrument.Hammer);
        worker.animator.Play("Idle");

        // destroy building
        destroyTask.building.Destroy();
        
        // return resources for building
        ResourceSystem.current.AddResources(destroyTask.buildingCost);

        destroyTask.SetExecutingState(EDestroyExecutingState.none);

        // complete
        onComplete?.Invoke();
    }
}