using System.Collections.Generic;
using UnityEngine;

public class TaskSystem : MonoBehaviour
{
    static public TaskSystem current;

    private Dictionary<E_TaskState, List<Task>> tasks;
    private List<UP_Worker> workers;    

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
        tasks = new Dictionary<E_TaskState, List<Task>>();
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
    }

    public void RemoveWorker(UP_Worker worker)
    {
        workers.Remove(worker);
    }
}