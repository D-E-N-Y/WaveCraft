using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UP_Worker : U_Player
{
    public E_WorkerState state { private set; get; }

    private TaskManager taskManager;
    private List<Task> tasks;
    [SerializeField, Range(1, 10)] private int limitTasks;

    [SerializeField, Range(1, 20)] private int maxAmount;
    private int currentAmount;

    public override void Initialize()
    {
        base.Initialize();

        taskManager = new TaskManager();
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
        if (state != E_WorkerState.Idle || tasks.Count == 0)
            return;

        state = E_WorkerState.Busy;
        Task currentTask = tasks[0];
        ITaskHandler handler = taskManager.GetHandler(currentTask);
        if (handler != null)
        {
            StartCoroutine(handler.ExecuteTask(this, currentTask, () => CompleteTask(currentTask)));
        }
        else
        {
            Debug.LogWarning($"No handler found for task: {currentTask.GetType()}");
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

    public int GetMaxAmount()
    {
        return maxAmount;
    }

    public int GetCurrentAmount()
    {
        return currentAmount;
    }

    public void AddCurrentAmount(int value)
    {
        currentAmount = Math.Min(currentAmount + value, maxAmount);
    }

    public void ClearCurrentAmount()
    {
        currentAmount = 0;
    }

    #region Do tasks

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
        // // move to building
        // agent.SetDestination(task.buildingPosition);
        // while(!agent.pathPending && agent.remainingDistance > 0.1f)
        // {
        //     yield return null;
        // }

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
        // // move to processor
        // I_Processor processor = GetNearbyProcessor(task.resource);
        // agent.SetDestination(processor.transform.position);
        // while(!agent.pathPending && agent.remainingDistance > 0.1f)
        // {
        //     yield return null;
        // }

        // // give processed resources
        // currentAmount = processor.Unload();

        // // move to storage
        // I_Storage storage = GetNearbyStorage(task.resource);
        // agent.SetDestination(storage.transform.position);
        // while(!agent.pathPending && agent.remainingDistance > 0.1f)
        // {
        //     yield return null;
        // }

        // // store resources
        // storage.AddResources(currentAmount);
        // currentAmount = 0;

        // // complete
        // Debug.Log("Store resources complete");

        yield return null;
    }

    private I_Storage GetNearbyStorage(E_Resource resource)
    {
        // get nearby storage from worker
        
        return null;
    }
        
    #endregion
}