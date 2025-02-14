using System.Collections.Generic;
using UnityEngine;

public class MiningTask : Task
{
    public E_Resource resource { private set; get; }
    public List<Transform> resourcePosition { private set; get; }
    public int resourceAmount { private set; get; }

    public MiningTask(Resource resource)
    {
        taskType = E_TaskType.Mining;

        this.resource = resource.GetType();
        resourcePosition = new List<Transform>();
        resourcePosition.Add(resource.transform);
        resourceAmount = (int)resource.GetCurrentHP();
    }

    public MiningTask(E_Resource resource, int resourceAmount)
    {
        taskType = E_TaskType.Mining;

        this.resource = resource;
        this.resourceAmount = resourceAmount;
    }
}
