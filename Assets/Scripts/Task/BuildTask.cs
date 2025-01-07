using UnityEngine.UIElements;

public class BuildTask : Task
{
    public Position buildingPosition { private set; get; }
    public float timeToBuild { private set; get; }

    public BuildTask(Position buildingPosition, float timeToBuild)
    {
        taskType = E_TaskType.Build;

        this.buildingPosition = buildingPosition;
        this.timeToBuild = timeToBuild;
    }
}