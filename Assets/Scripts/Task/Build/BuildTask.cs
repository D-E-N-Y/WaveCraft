public class BuildTask : Task
{
    public Building building { private set; get; }
    
    public EBuildExecutingState executingState { private set; get; } = EBuildExecutingState.none;

    public BuildTask(Building building)
    {
        type = E_TaskType.Build;

        this.building = building;
        goal = 100;

        nameTask = $"Build {building.nameActor}";
    }

    public void SetExecutingState(EBuildExecutingState state)
    {
        executingState = state;
        Update?.Invoke();
    }
}