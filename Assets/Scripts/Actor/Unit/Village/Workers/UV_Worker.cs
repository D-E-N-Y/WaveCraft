using System;
using System.Collections.Generic;
using UnityEngine;

public class UV_Worker : U_Village
{
    public Action UpdateTasks;
    public Action UpdateState;
    
    [SerializeField, Range(1, 10)] private int limitTasks;
    public List<Task> tasks { get; private set; }
    public bool isStopTask { get; private set; }
    public bool isAutoGetTask { get; private set; }
    private TaskManager taskManager;
    private Coroutine currentTask;

    public E_WorkerState state { private set; get; }

    [SerializeField, Range(1, 20)] private int maxMineAmount;
    private Dictionary<E_Resource, int> currentMineAmount;
    private Dictionary<E_Resource, int> currentStoreAmount;

    [SerializeField] GameObject hammerPrefab;
    [SerializeField] GameObject pickaxePrefab;
    private Dictionary<E_Instrument, GameObject> instruments;

    [SerializeField] private List<Material> materials;

    public override void Initialize()
    {
        base.Initialize();

        taskManager = new TaskManager();
        tasks = new List<Task>();
        
        isStopTask = false;
        isAutoGetTask = true;

        // initialize mine amount
        currentMineAmount = new Dictionary<E_Resource, int>();
        currentMineAmount.Add(E_Resource.Wood, 0);
        currentMineAmount.Add(E_Resource.Stone, 0);
        currentMineAmount.Add(E_Resource.Food, 0);

        // initialize store amount 
        currentStoreAmount = new Dictionary<E_Resource, int>();
        currentStoreAmount.Add(E_Resource.Wood, 0);
        currentStoreAmount.Add(E_Resource.Stone, 0);
        currentStoreAmount.Add(E_Resource.Food, 0);

        // initialize instruments
        pickaxePrefab.GetComponent<Mining>().Initialize(this);
        instruments = new Dictionary<E_Instrument, GameObject>();
        instruments.Add(E_Instrument.Hammer, hammerPrefab);
        instruments.Add(E_Instrument.Pickaxe, pickaxePrefab);

        // random material
        Material currentMaterial = materials[UnityEngine.Random.Range(0, materials.Count)];
        foreach(SkinnedMeshRenderer renderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            renderer.material = currentMaterial;
            renderer.UpdateGIMaterials();
        }

        state = E_WorkerState.Idle;
        TaskSystem.current.AddWorker(this);
    }
    
    #region Control tasks
        
    public void AddTask(Task task)
    {
        tasks.Add(task);
        task.SetWorker(this);
        
        UpdateTasks?.Invoke();

        DoTask();
    }

    public void DoTask()
    {
        if (state != E_WorkerState.Idle || tasks.Count == 0 || isStopTask)
            return;

        movement.StartMove();
        state = E_WorkerState.Busy;

        Task _task = tasks[0];
        ITaskHandler handler = taskManager.GetHandler(_task);
        if (handler != null)
        {
            currentTask = StartCoroutine(handler.ExecuteTask(this, _task, () => CompleteTask(_task)));
        }
        else
        {
            Debug.LogWarning($"No handler found for task: {currentTask.GetType()}");
        }
    }

    public void StopTask()
    {
        isStopTask = true;
        
        StopCoroutine(currentTask);
            
        movement.StopMove();
        
        animator.SetBool("isMove", false);
        animator.Play("Idle");

        hammerPrefab.SetActive(false);
        pickaxePrefab.SetActive(false);

        state = E_WorkerState.Idle;

        tasks[0].Update?.Invoke();
        UpdateState?.Invoke();
    }

    public void ContinueTask()
    {
        isStopTask = false;
        
        DoTask();

        tasks[0].Update?.Invoke();
        UpdateState?.Invoke();
    }

    public void CancelTask(Task task)
    {
        if(tasks[0] == task)
        {
            tasks.Remove(task);
            UpdateTasks?.Invoke();

            StopCoroutine(currentTask);
            
            movement.StopMove();
            
            animator.SetBool("isMove", false);
            animator.Play("Idle");

            hammerPrefab.SetActive(false);
            pickaxePrefab.SetActive(false);

            state = E_WorkerState.Idle;

            DoTask();
        }
        else
        {
            tasks.Remove(task);
            UpdateTasks?.Invoke();
        }

        TaskSystem.current.CancelTask(task);
    }

    private void CompleteTask(Task task)
    {
        tasks.Remove(task);
        UpdateTasks?.Invoke();
        
        state = E_WorkerState.Idle;

        TaskSystem.current.CompleteTask(task);
        DoTask();
    }

    #endregion

    public bool HasFreeTaskSpace() => limitTasks > tasks.Count;
    public int GetFreeSlots() => limitTasks - tasks.Count;
    public Task GetCurrentTask() => tasks.Count > 0 ? tasks[0] : null;

    public bool CheckFreeSpaceMineAmount() => maxMineAmount > GetCurrentMineAmount();
    public int GetMaxMineAmount() => maxMineAmount;

    public int GetCurrentMineAmount()
    {
        int result = 0;
        
        foreach(E_Resource resource in Enum.GetValues(typeof(E_Resource)))
        {
            result += currentMineAmount[resource];
        }

        return result;
    }

    public int GetCurrentStoreAmount()
    {
        int result = 0;
        
        foreach(E_Resource resource in Enum.GetValues(typeof(E_Resource)))
        {
            result += currentStoreAmount[resource];
        }

        return result;
    }

    public int GetCurrentMineAmountByResource(E_Resource resource) => currentMineAmount[resource];
    public int GetCurrentStoreAmountByResource(E_Resource resource) => currentStoreAmount[resource];

    public void AddCurrentMineAmount(E_Resource resource, int value) => currentMineAmount[resource] += value;
    public void ClearCurrentMineAmount(E_Resource resource) => currentMineAmount[resource] = 0;

    public void AddCurrentStoreAmount(E_Resource resource, int value) => currentStoreAmount[resource] += value;
    public void ClearCurrentStoreAmount(E_Resource resource) => currentStoreAmount[resource] = 0;

    public void ChangeAutoGetTasks() => isAutoGetTask = !isAutoGetTask;

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
}