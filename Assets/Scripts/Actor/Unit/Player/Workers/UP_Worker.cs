using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UP_Worker : U_Player
{
    private Dictionary<int, Task> tasks;
    private int taskPriority { get { return tasks.Count; } }
    [SerializeField, Range(1, 10)] private int limitTasks;
    
    [SerializeField] private NavMeshAgent agent;
    
    [SerializeField, Range(1, 20)] private int maxAmount;
    private int currentAmount;

    private void Start() 
    {
        Initialize();
    }

    public void Initialize()
    {
        TaskSystem.current.AddWorker(this);

        tasks = new Dictionary<int, Task>();
        currentAmount = 0;
    }
    
    #region Control tasks
        
    public void AddTask(Task task)
    {
        tasks[taskPriority] = task;

        DoTask();
    }

    public void DoTask()
    {
        // tasks[1] - first priority
    }

    public void StopTask()
    {

    }

    public void ContinueTask()
    {
        
    }

    private void CompleteTask()
    {

    }

    #endregion

    public bool HasFreeTaskSpace()
    {
        return limitTasks < tasks.Count;
    }


    #region Do tasks

    private IEnumerator BuildTask(BuildTask task)
    {
        // move to building
        agent.SetDestination(task.buildingPosition);
        while(!agent.pathPending && agent.remainingDistance > 0.1f)
        {
            yield return null;
        }

        // build
        float elapsedTime = 0f;
        while(elapsedTime < task.timeToBuild)
        {
            elapsedTime += Time.deltaTime;

            // Update UI

            yield return null;
        }

        // complete
        Debug.Log("Build complete");
    }

    private IEnumerator MiningTask(MiningTask task)
    {
        // move to resource
        Vector3 resourcePosition = task.resourcePosition;
        if(resourcePosition == null)
        {
            resourcePosition = GetNearbyResource(task.resource);
        }
        
        agent.SetDestination(resourcePosition);
        while(!agent.pathPending && agent.remainingDistance > 0.1f)
        {
            yield return null;
        }

        // mining
        // animation mining event
        while(currentAmount != maxAmount)
        {
            currentAmount++;
        }
        
        // move to processor
        I_Processors processor = GetNearbyProcessor(task.resource);
        agent.SetDestination(processor.transform.position);
        while(!agent.pathPending && agent.remainingDistance > 0.1f)
        {
            yield return null;
        }

        // store amount resources to processor
        processor.AddResource(currentAmount);
        currentAmount = 0;

        // complete
        Debug.Log("Mining complete");
    }

    private Vector3 GetNearbyResource(E_Resource resource)
    {
        // get nearby resource from worker
        
        return Vector3.zero;
    }

    private I_Processors GetNearbyProcessor(E_Resource resource)
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
        I_Processors processor = GetNearbyProcessor(task.resource);
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