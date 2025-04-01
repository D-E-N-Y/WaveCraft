using System;
using System.Collections;
using UnityEngine;

public class DestroyTaskHandler : ITaskHandler
{
    public IEnumerator ExecuteTask(UP_Worker worker, Task task, Action onComplete)
    {
        DestroyTask destroyTask = (DestroyTask)task;
        
        // move to building
        worker.animator.SetBool("isMove", true);
        yield return worker.movement.MoveTo(destroyTask.building.GetPosition(), UnitMovement.E_MoveTo.Object);

        worker.animator.SetBool("isMove", false);

        // build
        worker.animator.SetTrigger("Build");
        worker.ActiceInsturcent(UP_Worker.E_Instrument.Hammer);
        float elapsedTime = 0f;
        while(elapsedTime < destroyTask.timeToDestroy)
        {
            elapsedTime += Time.deltaTime;

            // Update UI

            // Debug.Log($"Building progress: {elapsedTime / task.timeToBuild * 100}%");

            yield return null;
        }
        yield return new WaitForSeconds(worker.animator.GetCurrentAnimatorStateInfo(0).length);
        worker.DisactiveInstument(UP_Worker.E_Instrument.Hammer);
        worker.animator.Play("Idle");

        // destroy building
        destroyTask.building.gameObject.SetActive(false);
        
        // return resources for building
        foreach(S_Cost _cost in destroyTask.buildingCost)
        {
            ResourceSystem.current.AddResources(_cost.resourse, _cost.count);
        }

        // complete
        onComplete?.Invoke();
    }
}