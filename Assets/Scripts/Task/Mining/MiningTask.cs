public class MiningTask : Task
{
    public Resource resource { private set; get; }
    public E_Resource resourceType { private set; get; }
    
    public Building processor;

    public EMiningExecutingState executingState { private set; get; } = EMiningExecutingState.none;

    public MiningTask(Resource resource)
    {
        type = E_TaskType.Mining;

        this.resource = resource;
        resourceType = resource.Type();
        
        goal = resource.GetCurrentHP();
    }

    public MiningTask(E_Resource resource, int resourceAmount)
    {
        type = E_TaskType.Mining;

        this.resourceType = resource;
        goal = resourceAmount;
    }

    public void SetProcessor(Building processor)
    {
        this.processor = processor;
        Update?.Invoke();
    }

    public void SetExecutingState(EMiningExecutingState state)
    {
        executingState = state;
        Update?.Invoke();
    }
}
