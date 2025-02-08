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

    [SerializeField] GameObject hammerPrefab;
    [SerializeField] GameObject pickaxePrefab;
    private Dictionary<E_Instrument, GameObject> instruments;

    public override void Initialize()
    {
        base.Initialize();

        taskManager = new TaskManager();
        tasks = new List<Task>();
        
        currentAmount = 0;

        state = E_WorkerState.Idle;

        TaskSystem.current.AddWorker(this);

        instruments = new Dictionary<E_Instrument, GameObject>();
        instruments.Add(E_Instrument.Hammer, hammerPrefab);
        instruments.Add(E_Instrument.Pickaxe, pickaxePrefab);

        pickaxePrefab.GetComponent<Mining>().Initialize(this);
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

    public enum E_Instrument
    {
        Hammer,
        Pickaxe
    }

    public void ActiceInsturcent(E_Instrument instrument)
    {
        instruments[instrument].SetActive(true);
    }

    public void DisactiveInstument(E_Instrument instrument)
    {
        instruments[instrument].SetActive(false);
    }

    public void EnableCollisionPickaxe()
    {
        pickaxePrefab.GetComponent<BoxCollider>().enabled = true;
    }

    public void DisableCollisionPickaxe()
    {
        pickaxePrefab.GetComponent<BoxCollider>().enabled = false;
        pickaxePrefab.GetComponent<Mining>().NullTarget();
    }

    #region Do tasks

        
    #endregion
}