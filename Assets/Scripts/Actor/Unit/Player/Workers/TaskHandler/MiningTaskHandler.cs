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

            worker.animator.SetBool("isMove", true);
            yield return worker.movement.MoveTo(miningTask.resourcePosition, UnitMovement.E_MoveTo.PlacedObject);
            worker.animator.SetBool("isMove", false);

            // Mining phase
            worker.animator.SetTrigger("Mine");
            worker.ActiceInsturcent(UP_Worker.E_Instrument.Pickaxe);
            
            while (worker.GetCurrentAmount() < worker.GetMaxAmount())
            {
                yield return null;

                if(currentMine + worker.GetCurrentAmount() >= miningTask.resourceAmount)
                {
                    break;
                }
            }
            
            AnimatorStateInfo stateInfo = worker.animator.GetCurrentAnimatorStateInfo(0);
            float remainigTime = ((worker.GetCurrentAmount() / worker.GetDamage()) - stateInfo.normalizedTime) * stateInfo.length;

            yield return new WaitForSeconds(remainigTime);
            
            worker.animator.Play("Idle");
            worker.DisactiveInstument(UP_Worker.E_Instrument.Pickaxe);

            // Debug.Log($"Mined {worker.GetCurrentAmount()} resources, moving to processor");
            currentMine += worker.GetCurrentAmount();

            // Move to processor
            worker.animator.SetBool("isMove", true);
            IProcessor processor = ProcessorSystem.current.GetNearbyProcessor(worker.transform.position, miningTask.resource);
            yield return worker.movement.MoveTo(processor.GetPosition(), UnitMovement.E_MoveTo.PlacedObject);
            worker.animator.SetBool("isMove", false);

            // Store resources
            // Debug.Log($"Storing {worker.GetCurrentAmount()} resources in {processor}");
            processor.AddResources(worker.GetCurrentAmount());
            worker.ClearCurrentAmount();
        }

        // complete
        onComplete?.Invoke();
    }
}