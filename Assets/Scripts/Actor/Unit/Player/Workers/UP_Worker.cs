using System.Collections.Generic;
using UnityEngine;

public class UP_Worker : U_Player
{
    private Dictionary<int, Task> tasks;
    private int taskPriority { get { return tasks.Count; } }
    [SerializeField, Range(1, 10)] private int limitTasks;
    

    private void Start() 
    {
        Initialize();
    }

    public void Initialize()
    {
        TaskSystem.current.AddWorker(this);

        tasks = new Dictionary<int, Task>();
    }
    
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

    public bool HasFreeTaskSpace()
    {
        return limitTasks < tasks.Count;
    }
}