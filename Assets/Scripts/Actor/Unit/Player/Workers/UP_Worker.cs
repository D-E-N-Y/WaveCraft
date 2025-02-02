using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UP_Worker : U_Player
{
    public E_WorkerState state { private set; get; }

    private List<Task> tasks;
    [SerializeField, Range(1, 10)] private int limitTasks;
    
    [SerializeField] private NavMeshAgent agent;
    
    [SerializeField, Range(1, 20)] private int maxAmount;
    private int currentAmount;

    public override void Initialize()
    {
        tasks = new List<Task>();
        currentAmount = 0;

        state = E_WorkerState.Idle;

        TaskSystem.current.AddWorker(this);
    }
    
    #region Control tasks
        
    public void AddTask(Task task)
    {
        tasks.Add(task);

        DoTask();
    }

    public void DoTask()
    {
        if(state != E_WorkerState.Idle || tasks.Count == 0)
        {
            return;
        }

        state = E_WorkerState.Busy;
        
        switch(tasks[0])
        {
            case BuildTask buildTask:
                StartCoroutine(BuildTask(buildTask));
                break;

            case MiningTask miningTask:
                StartCoroutine(MiningTask(miningTask));
                break;

            case DestroyTask destroyTask:
                break;

            case StoreTask storeTask:
                break;
        }

    }

    public void StopTask()
    {

    }

    public void ContinueTask()
    {
        
    }

    private void CompleteTask(Task task)
    {
        TaskSystem.current.CompleteTask(task);
        
        tasks.Remove(task);
        state = E_WorkerState.Idle;

        DoTask();
    }

    #endregion

    public bool HasFreeTaskSpace()
    {
        return limitTasks > tasks.Count;
    }


    #region Movement

    private Vector3 lastTargetPosition;
    Vector3 GetBestNavMeshPoint(Vector3 agentPosition, Vector3 targetPosition)
    {
        Vector3 midPoint = (agentPosition + targetPosition) / 2;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(midPoint, out hit, 5f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return targetPosition;
    }

    void MoveToObject(Vector3 target)
    {
        Vector3 bestPoint = GetBestNavMeshPoint(transform.position, target);
        
        if (Vector3.Distance(agent.destination, bestPoint) > 1f)
        {
            agent.SetDestination(bestPoint);
        }
    }    
    
    #endregion


    #region Do tasks

    private IEnumerator BuildTask(BuildTask task)
    {
        // move to building
        agent.SetDestination(task.building.transform.position);
        
        // Wait until the path is calculated
        while (agent.pathPending)
        {
            yield return null;
        }

        // Wait until agent reaches the destination
        while (agent.remainingDistance > 0.1f || agent.velocity.sqrMagnitude > 0f)
        {
            yield return null;
        }
        agent.ResetPath();

        Debug.Log("start build");

        // build
        float elapsedTime = 0f;
        while(elapsedTime < task.building.GetTimeToBuild())
        {
            elapsedTime += Time.deltaTime;

            // Update UI

            // Debug.Log($"Building progress: {elapsedTime / task.timeToBuild * 100}%");

            yield return null;
        }
        task.building.Built();
        
        // complete
        CompleteTask(task);
    }

    private IEnumerator MiningTask(MiningTask task)
    {
        int currentMine = 0;
        Debug.Log($"Need mine {task.resourceAmount}");

        while(currentMine <= task.resourceAmount)
        {
            // move to resource
            Debug.Log($"Move to {task.resource}");
            Vector3 resourcePosition = task.resourcePosition;
            if(resourcePosition == null)
            {
                resourcePosition = GetNearbyResource(task.resource);
            }
            
            while (true)
            {
                MoveToObject(resourcePosition);
                lastTargetPosition = resourcePosition;

                yield return new WaitForSeconds(0.5f);

                if(agent.remainingDistance <= 0.1f || agent.velocity.sqrMagnitude <= 0f)
                {
                    break;
                }
            }
            agent.ResetPath();

            // mining
            // animation mining event
            while(currentAmount != maxAmount)
            {
                currentAmount++;
            }

            Debug.Log($"Mine {currentAmount} resources, move to processor");
            currentMine += currentAmount;
            
            // move to processor
            IProcessor processor = ProcessorSystem.current.GetNearbyProcessor(transform.position, task.resource);
            while (true)
            {
                MoveToObject(processor.GetPosition());
                lastTargetPosition = resourcePosition;

                yield return new WaitForSeconds(0.5f);

                if(agent.remainingDistance <= 0.1f || agent.velocity.sqrMagnitude <= 0f)
                {
                    break;
                }
            }
            agent.ResetPath();

            // store amount resources to processor
            Debug.Log($"Store {currentAmount} to {processor} ");
            processor.AddResources(currentAmount);
            currentAmount = 0;
        }

        // complete
        Debug.Log("Mining complete");
        CompleteTask(task);
    }

    private Vector3 GetNearbyResource(E_Resource resource)
    {
        // get nearby resource from worker
        
        return Vector3.zero;
    }

    private I_Processor GetNearbyProcessor(E_Resource resource)
    {
        // get nearby processor from worker
        
        return null;
    }

    private IEnumerator DestroyTask(DestroyTask task)
    {
        // move to building
        agent.SetDestination(task.buildingPosition);
        while(!agent.pathPending && agent.remainingDistance > 0.1f)
        {
            yield return null;
        }

        // destroy
        float elapsedTime = 0f;
        while(elapsedTime < task.timeToDestroy)
        {
            elapsedTime += Time.deltaTime;

            // Update UI

            yield return null;
        }

        // complete
        Debug.Log("Build complete");
    }

    private IEnumerator StoreTask(StoreTask task)
    {
        // move to processor
        I_Processor processor = GetNearbyProcessor(task.resource);
        agent.SetDestination(processor.transform.position);
        while(!agent.pathPending && agent.remainingDistance > 0.1f)
        {
            yield return null;
        }

        // give processed resources
        currentAmount = processor.Unload();

        // move to storage
        I_Storage storage = GetNearbyStorage(task.resource);
        agent.SetDestination(storage.transform.position);
        while(!agent.pathPending && agent.remainingDistance > 0.1f)
        {
            yield return null;
        }

        // store resources
        storage.AddResources(currentAmount);
        currentAmount = 0;

        // complete
        Debug.Log("Store resources complete");
    }

    private I_Storage GetNearbyStorage(E_Resource resource)
    {
        // get nearby storage from worker
        
        return null;
    }
        
    #endregion
}