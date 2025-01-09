using UnityEngine;

public class BuildTask : Task
{
    public Building building { private set; get; }

    public BuildTask(Building building)
    {
        taskType = E_TaskType.Build;

        this.building = building;
    }
}