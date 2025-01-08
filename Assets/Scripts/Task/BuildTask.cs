using UnityEngine;

public class BuildTask : Task
{
    public Vector3 buildingPosition { private set; get; }
    public float timeToBuild { private set; get; }

    public BuildTask(Vector3 buildingPosition, float timeToBuild)
    {
        taskType = E_TaskType.Build;

        this.buildingPosition = buildingPosition;
        this.timeToBuild = timeToBuild;
    }
}