using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskSystem : GameSystem
{
    static public TaskSystem current;

    public Action UpdateTasks;

    private Dictionary<E_TaskState, List<Task>> tasks;
    private List<UP_Worker> workers;    

    public override void Initialize()
    {
        current = this;

        tasks = new Dictionary<E_TaskState, List<Task>>();
        tasks[E_TaskState.Pending] = new List<Task>();
        tasks[E_TaskState.Execured] = new List<Task>();

        workers = new List<UP_Worker>();
    }

    public void AddTask(Task task)
    {
        tasks[E_TaskState.Pending].Add(task);
        UpdateTasks?.Invoke();
        
        DoTask(task);
    }

    public void DoTask(Task task)
    {
        UP_Worker worker = FindFreeWorker();
        if(worker)
        {
            worker.AddTask(task);
            tasks[E_TaskState.Execured].Add(task);
            tasks[E_TaskState.Pending].Remove(task);

            UpdateTasks?.Invoke();

            Debug.Log($"{worker} do {task}");
        }
        else
        {
            Debug.Log("Not have free workers");
        }
    }

    public void DoTask(Task task, UP_Worker worker)
    {
        worker.AddTask(task);
        tasks[E_TaskState.Execured].Add(task);
        tasks[E_TaskState.Pending].Remove(task);

        UpdateTasks?.Invoke();
    }

    private UP_Worker FindFreeWorker()
    {
        foreach(UP_Worker worker in workers)
        {
            if(worker.state == E_WorkerState.Idle && worker.isAutoGetTask)
            {
                return worker;
            }
        }

        foreach(UP_Worker worker in workers)
        {
            if(worker.HasFreeTaskSpace() && worker.isAutoGetTask)
            {
                return worker;
            }
        }

        return null;
    }

    public void CompleteTask(Task task)
    {
        tasks[E_TaskState.Execured].Remove(task);
        UpdateTasks?.Invoke();

        if(tasks[E_TaskState.Pending].Count > 0)
        {
            DoTask(tasks[E_TaskState.Pending][0]);
        }
    }

    public void CancelTask(Task task, E_TaskState state)
    {
        tasks[state].Remove(task);
        UpdateTasks?.Invoke();
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

    public List<Task> GetTasks(E_TaskState state) => tasks[state];
}