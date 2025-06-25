using System.Linq;

public class DestroyTask : Task
{
    public Building building{ private set; get; }
    public float timeToDestroy { private set; get; }
    public S_Cost[] buildingCost { private set; get; }

    public EDestroyExecutingState executingState { private set; get; } = EDestroyExecutingState.none;

    public DestroyTask(Building building)
    {
        type = E_TaskType.Destroy;

        this.building = building;
        timeToDestroy = building.GetTimeToBuild() / 2;

        buildingCost = building.GetCost().Select(x => new S_Cost { resourse = x.resourse, count = (int)(x.count * 0.8f) }).ToArray(); ;
        goal = 100;
        
        nameTask = $"Destroy {building.nameActor}";
    }

    public void SetExecutingState(EDestroyExecutingState state)
    {
        executingState = state;
        Update?.Invoke();
    }
}