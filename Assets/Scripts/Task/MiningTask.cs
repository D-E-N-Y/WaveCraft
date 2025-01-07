using UnityEngine.UIElements;

public class MiningTask : Task
{
    public E_Resource resource { private set; get; }
    public Position resourcePosition { private set; get; }
    public int resourceAmount { private set; get; }
    public I_Processors processor { private set; get; }

    public MiningTask(E_Resource resource, Position resourcePosition, int resourceAmount, I_Processors processor)
    {
        taskType = E_TaskType.Mining;

        this.resource = resource;
        this.resourcePosition = resourcePosition;
        this.resourceAmount = resourceAmount;
        this.processor = processor;
    }
}
