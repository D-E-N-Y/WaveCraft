using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskSystem : GameSystem
{
    static public TaskSystem current;

    public Action UpdateTasks;
    public Action UpdateWorkers;

    private Dictionary<E_TaskState, List<Task>> tasks;
    private List<UV_Worker> workers;

    public override void Initialize()
    {
        current = this;

        tasks = new Dictionary<E_TaskState, List<Task>>();
        tasks[E_TaskState.Pending] = new List<Task>();
        tasks[E_TaskState.Execured] = new List<Task>();
        tasks[E_TaskState.Canceled] = new List<Task>();
        tasks[E_TaskState.Completed] = new List<Task>();

        workers = new List<UV_Worker>();
    }

    public void AddTask(Task task)
    {
        tasks[E_TaskState.Pending].Add(task);
        UpdateTasks?.Invoke();

        DoTask(task);
    }

    public void DoTask(Task task)
    {
        UV_Worker worker = FindFreeWorker();
        if (worker)
        {
            worker.AddTask(task);

            tasks[E_TaskState.Execured].Add(task);
            task.SetState(E_TaskState.Execured);

            if (tasks[E_TaskState.Pending].Contains(task))
            {
                tasks[E_TaskState.Pending].Remove(task);
            }
            if (tasks[E_TaskState.Canceled].Contains(task))
            {
                tasks[E_TaskState.Canceled].Remove(task);
            }

            UpdateTasks?.Invoke();

            Debug.Log($"{worker} do {task}");
        }
        else
        {
            Debug.Log("Not have free workers");
        }
    }

    public void DoTask(Task task, UV_Worker worker)
    {
        worker.AddTask(task);

        tasks[E_TaskState.Execured].Add(task);
        task.SetState(E_TaskState.Execured);

        if (tasks[E_TaskState.Pending].Contains(task))
        {
            tasks[E_TaskState.Pending].Remove(task);
        }
        if (tasks[E_TaskState.Canceled].Contains(task))
        {
            tasks[E_TaskState.Canceled].Remove(task);
        }

        UpdateTasks?.Invoke();
    }

    private UV_Worker FindFreeWorker()
    {
        foreach (UV_Worker worker in workers)
        {
            if (worker.state == E_WorkerState.Idle && worker.isAutoGetTask)
            {
                return worker;
            }
        }

        foreach (UV_Worker worker in workers)
        {
            if (worker.HasFreeTaskSpace() && worker.isAutoGetTask)
            {
                return worker;
            }
        }

        return null;
    }

    public void CompleteTask(Task task)
    {
        tasks[E_TaskState.Execured].Remove(task);

        tasks[E_TaskState.Completed].Add(task);
        task.SetState(E_TaskState.Completed);

        UpdateTasks?.Invoke();

        if (tasks[E_TaskState.Pending].Count > 0)
        {
            DoTask(tasks[E_TaskState.Pending][0]);
        }
    }

    public void CancelTask(Task task)
    {
        tasks[E_TaskState.Execured].Remove(task);

        tasks[E_TaskState.Canceled].Add(task);
        task.SetState(E_TaskState.Canceled);
        task.SetAutoWorker(false);
        task.ResetWorker();

        UpdateTasks?.Invoke();
    }

    public void RemoveTask(Task task)
    {
        if (task.worker != null && task.state != E_TaskState.Completed)
        {
            task.worker.CancelTask(task);
        }

        foreach (E_TaskState state in Enum.GetValues(typeof(E_TaskState)))
        {
            if (tasks[state].Contains(task))
            {
                tasks[state].Remove(task);
            }
        }

        UpdateTasks?.Invoke();
        task.Update?.Invoke();
    }

    public void AddWorker(UV_Worker worker)
    {
        workers.Add(worker);
        UpdateWorkers?.Invoke();

        if (tasks.ContainsKey(E_TaskState.Pending) && tasks[E_TaskState.Pending].Count > 0)
        {
            Task freeTask = tasks[E_TaskState.Pending]
                .Where(x => x.isAutoWorker)
                .FirstOrDefault();

            if (freeTask != null)
            {
                DoTask(freeTask);
            }
        }
    }

    public void RemoveWorker(UV_Worker worker)
    {
        workers.Remove(worker);
        UpdateWorkers?.Invoke();
    }

    public List<Task> GetTasks(E_TaskState state) => tasks[state];

    public int GetCount()
    {
        int count = 0;

        foreach (var current in tasks)
        {
            count += current.Value.Count;
        }

        return count;
    }

    public bool HasBuildingInBuildTask(Building _building)
    {
        foreach (var current in tasks)
        {
            foreach (Task task in current.Value)
            {
                if (task is BuildTask buildTask && buildTask.building == _building)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public BuildTask GetBuildTaskForBuilding(Building _building)
    {
        foreach (var current in tasks)
        {
            foreach (Task task in current.Value)
            {
                if (task is BuildTask buildTask && buildTask.building == _building)
                {
                    return buildTask;
                }
            }
        }

        return null;
    }

    public bool HasBuildingInDestroyTask(Building _building)
    {
        foreach (var current in tasks)
        {
            foreach (Task task in current.Value)
            {
                if (task is DestroyTask destroyTask && destroyTask.building == _building)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public DestroyTask GetDestroyTaskForBuilding(Building _building)
    {
        foreach (var current in tasks)
        {
            foreach (Task task in current.Value)
            {
                if (task is DestroyTask destroyTask && destroyTask.building == _building)
                {
                    return destroyTask;
                }
            }
        }

        return null;
    }

    public bool HasTask(Task _task)
    {
        foreach (var current in tasks)
        {
            if (current.Value.Contains(_task))
            {
                return true;
            }
        }

        return false;
    }
}