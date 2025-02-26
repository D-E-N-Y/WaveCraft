using System;
using System.Collections;
using UnityEngine;

public class MiningTaskHandler : ITaskHandler
{
    public IEnumerator ExecuteTask(UP_Worker worker, Task task, Action onComplete)
    {
        // check have resource
        if(!worker.CheckFreeSpaceMiningAmount())
        {
            yield return StoreResources(worker);
        }
        
        MiningTask miningTask = (MiningTask)task;
        Resource resource = miningTask.resource;
        int currentMine = 0;

        if(!resource)
        {
            // get nearby resource by type
        }

        while (currentMine < miningTask.resourceAmount)
        {
            if(!resource.gameObject.activeSelf) break;

            // Move to resource
            worker.animator.SetBool("isMove", true);
            yield return worker.movement.MoveTo(resource.GetPosition(), UnitMovement.E_MoveTo.PlacedObject);
            worker.animator.SetBool("isMove", false);

            // Mining phase
            worker.ActiceInsturcent(UP_Worker.E_Instrument.Pickaxe);
            while (worker.GetCurrentAmount() < worker.GetMaxAmount())
            {
                worker.animator.SetTrigger("Mine");
                yield return null;
                yield return new WaitForSeconds(worker.animator.GetCurrentAnimatorStateInfo(0).length);

                if(currentMine + worker.GetCurrentAmount() >= miningTask.resourceAmount)
                {
                    break;
                }

                if(!resource.gameObject.activeSelf)
                {
                    break;
                }
            }
            
            worker.animator.Play("Idle");
            worker.DisactiveInstument(UP_Worker.E_Instrument.Pickaxe);

            // Debug.Log($"Mined {worker.GetCurrentAmount()} resources, moving to processor");
            currentMine += worker.GetCurrentAmount();

            // Move to processor
            yield return StoreResources(worker);
        }

        // complete
        onComplete?.Invoke();
    }

    private IEnumerator StoreResources(UP_Worker worker)
    {
        while(worker.GetCurrentAmount() > 0)
        {
            foreach(E_Resource resource in Enum.GetValues(typeof(E_Resource)))
            {
                if(worker.GetCurrentAmountByResource(resource) > 0)
                {
                    // Move to processor
                    worker.animator.SetBool("isMove", true);
                    IProcessor processor = ProcessorSystem.current.GetNearbyProcessor(worker.transform.position, resource);
                    IPosition position = (IPosition)processor;
                    
                    yield return worker.movement.MoveTo(position.GetPosition(), UnitMovement.E_MoveTo.PlacedObject);
                    worker.animator.SetBool("isMove", false);

                    // Store resources
                    processor.AddResources(worker.GetCurrentAmountByResource(resource));
                    worker.ClearCurrentAmount(resource);
                }
            }
        }
    }
}