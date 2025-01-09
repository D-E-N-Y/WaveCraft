using UnityEngine;

public class BuildTask : Task
{
    public Building building { private set; get; }
    public Vector3 buildingPosition { private set; get; }
    public float timeToBuild { private set; get; }

    public BuildTask(Building building, Vector3 buildingPosition, float timeToBuild)
    {
        taskType = E_TaskType.Build;

        this.building = building;
        this.buildingPosition = buildingPosition;
        this.timeToBuild = timeToBuild;
    }
}