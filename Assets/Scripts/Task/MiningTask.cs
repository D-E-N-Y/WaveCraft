using System.Collections.Generic;
using UnityEngine;

public class MiningTask : Task
{
    public Resource resource { private set; get; }
    public E_Resource resourceType { private set; get; }
    public int resourceAmount { private set; get; }

    public MiningTask(Resource resource)
    {
        type = E_TaskType.Mining;

        this.resource = resource;
        resourceType = resource.Type();
        resourceAmount = (int)resource.GetCurrentHP();
    }

    public MiningTask(E_Resource resource, int resourceAmount)
    {
        type = E_TaskType.Mining;

        this.resourceType = resource;
        this.resourceAmount = resourceAmount;
    }
}
