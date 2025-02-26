using System;
using System.Collections.Generic;
using UnityEngine;

public class UP_Worker : U_Player
{
    public Action UpdateTasks;
    
    public E_WorkerState state { private set; get; }
    private TaskManager taskManager;
    public List<Task> tasks { get; private set; }
    [SerializeField, Range(1, 10)] private int limitTasks;
    private Coroutine currentTask;

    [SerializeField, Range(1, 20)] private int maxAmount;
    private Dictionary<E_Resource, int> currentAmount;

    [SerializeField] GameObject hammerPrefab;
    [SerializeField] GameObject pickaxePrefab;
    private Dictionary<E_Instrument, GameObject> instruments;

    [SerializeField] private List<Material> materials;

    public override void Initialize()
    {
        base.Initialize();

        proffesion = "Worker";

        taskManager = new TaskManager();
        tasks = new List<Task>();
        
        currentAmount = new Dictionary<E_Resource, int>();
        currentAmount.Add(E_Resource.Wood, 0);
        currentAmount.Add(E_Resource.Stone, 0);
        currentAmount.Add(E_Resource.Food, 0);

        state = E_WorkerState.Idle;

        TaskSystem.current.AddWorker(this);

        instruments = new Dictionary<E_Instrument, GameObject>();
        instruments.Add(E_Instrument.Hammer, hammerPrefab);
        instruments.Add(E_Instrument.Pickaxe, pickaxePrefab);

        pickaxePrefab.GetComponent<Mining>().Initialize(this);

        Material currentMaterial = materials[UnityEngine.Random.Range(0, materials.Count)];
        foreach(SkinnedMeshRenderer renderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            renderer.material = currentMaterial;
            renderer.UpdateGIMaterials();
        }
    }
    
    #region Control tasks
        
    public void AddTask(Task task)
    {
        tasks.Add(task);
        UpdateTasks?.Invoke();

        DoTask();
    }

    public void DoTask()
    {
        if (state != E_WorkerState.Idle || tasks.Count == 0)
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
        
    }

    public void ContinueTask()
    {
        
    }

    public void CancelTask(Task task)
    {
        TaskSystem.current.CancelTask(task, E_TaskState.Execured);

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
    }

    private void CompleteTask(Task task)
    {
        TaskSystem.current.CompleteTask(task);
        
        tasks.Remove(task);
        UpdateTasks?.Invoke();
        
        state = E_WorkerState.Idle;

        DoTask();
    }

    #endregion

    public bool HasFreeTaskSpace() => limitTasks > tasks.Count;
    
    public bool CheckFreeSpaceMiningAmount() => maxAmount > GetCurrentAmount();
    public int GetMaxAmount() => maxAmount;

    public int GetCurrentAmount() => currentAmount[E_Resource.Wood] + currentAmount[E_Resource.Stone] + currentAmount[E_Resource.Food];
    public int GetCurrentAmountByResource(E_Resource resource) => currentAmount[resource];

    public void AddCurrentAmount(E_Resource resource, int value) => currentAmount[resource] += value;
    public void ClearCurrentAmount(E_Resource resource) => currentAmount[resource] = 0;

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