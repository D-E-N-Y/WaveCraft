using UnityEngine;

public class DestroyTask : Task
{
    public Vector3 buildingPosition { private set; get; }
    public float timeToDestroy { private set; get; }
    public S_Cost[] buildingCost { private set; get; }

    public DestroyTask(Vector3 buildingPosition, float timeToBuild, S_Cost[] buildingCost)
    {
        taskType = E_TaskType.Destroy;

        this.buildingPosition = buildingPosition;
        this.timeToDestroy = timeToBuild / 2;
        this.buildingCost = buildingCost;
    }
}