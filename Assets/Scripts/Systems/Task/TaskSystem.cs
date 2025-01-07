using System.Collections.Generic;
using UnityEngine;

public class TaskSystem : MonoBehaviour
{
    static public TaskSystem current;

    private Dictionary<E_Task, List<Task>> tasks;
    // private List<UP_Worker> workers;    

    private void Awake() 
    {
        current = this;
    }

    private void Start() 
    {
        Initialize();
    }

    public void Initialize()
    {
        tasks = new Dictionary<E_Task, List<Task>>();
    }

    public void AddTask(Task task)
    {
        if(!tasks.ContainsKey(E_Task.Pending))
        {
            tasks[E_Task.Pending] = new List<Task>();
        }

        tasks[E_Task.Pending].Add(task);
        
        DoTask(task);
    }

    public void DoTask(Task task)
    {
        if(!tasks.ContainsKey(E_Task.Execured))
        {
            tasks[E_Task.Execured] = new List<Task>();
        }

        // Worker worker = FindFreeWorker();
        // if(worker)
        // {
        //     worker.AddTask();
        //     tasks[E_Task.Execured].Add(task);
        // }
        // else
        // {
        //     Debug.Log("Not have free workers");
        // }
    }

    private bool FindFreeWorker()
    {
        // foreach(Worker worker in workers)
        // {
        //     if(worker.HasFreeTaskSpace())
        //     {
        //         return worker;
        //     }
        // }

        // return null;

        return false;
    }

    public void CompleteTask(Task task)
    {
        // remove task from UI
        
        tasks[E_Task.Execured].Remove(task);

        Debug.Log("Complete task");
    }

    public void CancelTask(Task task)
    {
        // remove task from UI
        
        tasks[E_Task.Pending].Remove(task);

        Debug.Log("Cancel task");
    }

    public void AddWorker()
    {
        // workers.Add(worker);
    }

    public void RemoveWorker()
    {
        // workers.Remove(worker);
    }
}