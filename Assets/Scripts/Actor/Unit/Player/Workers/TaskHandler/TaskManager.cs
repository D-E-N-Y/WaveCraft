using System;
using System.Collections.Generic;

public class TaskManager
{
    private Dictionary<Type, ITaskHandler> taskHandlers;

    public TaskManager()
    {  
        taskHandlers = new Dictionary<Type, ITaskHandler>();

        taskHandlers.Add(typeof(BuildTask), new BuildTaskHandler());
        taskHandlers.Add(typeof(MiningTask), new MiningTaskHandler());
    }

    public ITaskHandler GetHandler(Task task)
    {
        Type taskType = task.GetType();
        return taskHandlers.ContainsKey(taskType) ? taskHandlers[taskType] : null;
    }
}