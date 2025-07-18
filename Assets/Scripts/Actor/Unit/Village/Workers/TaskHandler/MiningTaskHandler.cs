using System;
using System.Collections;
using UnityEngine;

public class MiningTaskHandler : ITaskHandler
{
    MiningTask miningTask;

    public IEnumerator ExecuteTask(UV_Worker worker, Task task, Action onComplete)
    {
        // check have resource
        if (!worker.CheckFreeSpaceMineAmount())
        {
            yield return StoreResources(worker);
        }

        miningTask = (MiningTask)task;
        Resource resource = miningTask.resource;

        // if(!resource)
        // {
        //     // get nearby resource by type
        // }

        while (miningTask.progress < miningTask.goal)
        {
            if (!resource.gameObject.activeSelf) break;

            miningTask.SetExecutingState(EMiningExecutingState.MoveToResource);

            // Move to resource
            yield return Moving(worker, resource, E_MoveTo.NatureObject);

            miningTask.SetExecutingState(EMiningExecutingState.Mining);

            // Mining phase
            worker.ActiceInsturcent(UV_Worker.E_Instrument.Pickaxe);
            while (worker.GetCurrentMineAmount() < worker.GetMaxMineAmount())
            {
                worker.animator.SetTrigger("Mine");
                yield return null;
                yield return new WaitForSeconds(worker.animator.GetCurrentAnimatorStateInfo(0).length);

                if (miningTask.progress + worker.GetCurrentMineAmount() >= miningTask.goal)
                {
                    break;
                }

                if (!resource.gameObject.activeSelf)
                {
                    break;
                }
            }

            worker.animator.Play("Idle");
            worker.DisactiveInstument(UV_Worker.E_Instrument.Pickaxe);

            miningTask.SetExecutingState(EMiningExecutingState.MoveToProcessor);

            // Move to processor
            yield return StoreResources(worker);
        }

        miningTask.SetExecutingState(EMiningExecutingState.none);

        // complete
        onComplete?.Invoke();
    }

    private IEnumerator StoreResources(UV_Worker worker)
    {
        while (worker.GetCurrentMineAmount() > 0)
        {
            foreach (E_Resource resource in Enum.GetValues(typeof(E_Resource)))
            {
                if (worker.GetCurrentMineAmountByResource(resource) > 0)
                {
                    // Move to processor
                    IProcessor processor = ProcessorSystem.current.GetNearbyProcessor(worker.transform.position, resource);
                    IPosition position = (IPosition)processor;

                    if (processor is IModule)
                    {
                        miningTask.SetProcessor(((IModule)processor).GetBuilding());
                    }
                    else
                    {
                        miningTask.SetProcessor((Building)processor);
                    }

                    yield return Moving(worker, position, E_MoveTo.PlacedObject);

                    miningTask.SetProgress(worker.GetCurrentMineAmount());
                    miningTask.SetProcessor(null);

                    // Store resources
                    processor.AddResources(worker.GetCurrentMineAmountByResource(resource));
                    worker.ClearCurrentMineAmount(resource);
                }
            }
        }
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