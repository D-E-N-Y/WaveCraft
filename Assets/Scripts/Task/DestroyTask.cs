using UnityEngine.UIElements;

public class DestroyTask : Task
{
    public Position buildingPosition { private set; get; }
    public float timeToDestroy { private set; get; }
    public S_Cost[] buildingCost { private set; get; }

    public DestroyTask(Position buildingPosition, float timeToDestroy, S_Cost[] buildingCost)
    {
        taskType = E_TaskType.Destroy;

        this.buildingPosition = buildingPosition;
        this.timeToDestroy = timeToDestroy;
        this.buildingCost = buildingCost;
    }
}