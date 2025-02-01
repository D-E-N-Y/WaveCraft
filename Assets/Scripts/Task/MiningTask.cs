using UnityEngine;

public class MiningTask : Task
{
    public E_Resource resource { private set; get; }
    public Vector3 resourcePosition { private set; get; }
    public int resourceAmount { private set; get; }

    public MiningTask(Resource resource)
    {
        taskType = E_TaskType.Mining;

        this.resource = resource.GetType();
        this.resourcePosition = resource.transform.position;
        this.resourceAmount = (int)resource.GetCurrentHP();
    }

    public MiningTask(E_Resource resource, int resourceAmount)
    {
        taskType = E_TaskType.Mining;

        this.resource = resource;
        this.resourceAmount = resourceAmount;
    }
}
