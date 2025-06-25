public class StoreTask : Task
{
    public E_Resource resource { private set; get; }
    public int amount { private set; get; }

    public Building source { private set; get; }
    public Building storage { private set; get; }

    public EStoreExecutingState executingState { private set; get; } = EStoreExecutingState.none;

    public StoreTask(E_Resource resource, Building source)
    {
        type = E_TaskType.Store;

        this.resource = resource;
        this.source = source;

        amount = 0;
        goal = 100;

        nameTask = $"Store {resource}";
    }

    public void SetStorage(Building storage) 
    {
        this.storage = storage;
        Update?.Invoke();
    }

    public void SetExecutingState(EStoreExecutingState state)
    {
        executingState = state;
        Update?.Invoke();
    }

    public void SetAmount(int value) => amount = value;
}