using System.Collections.Generic;
using UnityEngine;

public class TaskSystem : GameSystem
{
    static public TaskSystem current;

    private Dictionary<E_TaskState, List<Task>> tasks;
    private List<UP_Worker> workers;    

    [SerializeField] private GameObject workerPrefab;

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            UP_Worker worker = Instantiate(workerPrefab).GetComponent<UP_Worker>();
            worker.Initialize();
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        current = this;

        tasks = new Dictionary<E_TaskState, List<Task>>();
        workers = new List<UP_Worker>();
    }

    public void AddTask(Task task)
    {
        if(!tasks.ContainsKey(E_TaskState.Pending))
        {
            tasks[E_TaskState.Pending] = new List<Task>();
        }

        tasks[E_TaskState.Pending].Add(task);
        
        DoTask(task);
    }

    public void DoTask(Task task)
    {
        if(!tasks.ContainsKey(E_TaskState.Execured))
        {
            tasks[E_TaskState.Execured] = new List<Task>();
        }

        UP_Worker worker = FindFreeWorker();
        if(worker)
        {
            worker.AddTask(task);
            tasks[E_TaskState.Execured].Add(task);
            tasks[E_TaskState.Pending].Remove(task);

            Debug.Log($"{worker} do {task}");
        }
        else
        {
            Debug.Log("Not have free workers");
        }
    }

    private UP_Worker FindFreeWorker()
    {
        foreach(UP_Worker worker in workers)
        {
            if(worker.state == E_WorkerState.Idle)
            {
                return worker;
            }
        }

        foreach(UP_Worker worker in workers)
        {
            if(worker.HasFreeTaskSpace())
            {
                return worker;
            }
        }

        return null;
    }

    public void CompleteTask(Task task)
    {
        // remove task from UI
        
        tasks[E_TaskState.Execured].Remove(task);
        Debug.Log("Complete task");

        if(tasks[E_TaskState.Pending].Count > 0)
        {
            DoTask(tasks[E_TaskState.Pending][0]);
        }
    }

    public void CancelTask(Task task)
    {
        // remove task from UI
        
        tasks[E_TaskState.Pending].Remove(task);

        Debug.Log("Cancel task");
    }

    public void AddWorker(UP_Worker worker)
    {
        workers.Add(worker);

        if(tasks.ContainsKey(E_TaskState.Pending) && tasks[E_TaskState.Pending].Count > 0)
        {
            DoTask(tasks[E_TaskState.Pending][0]);
        }
    }

    public void RemoveWorker(UP_Worker worker)
    {
        workers.Remove(worker);
    }
}