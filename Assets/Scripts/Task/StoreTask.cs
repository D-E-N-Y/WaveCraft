public class StoreTask : Task
{
    public E_Resource resource { private set; get; }

    public Building source { private set; get; }
    public Building storage { private set; get; }

    public StoreTask(E_Resource resource, Building source)
    {
        type = E_TaskType.Store;

        this.resource = resource;
        this.source = source;
    }

    public void SetStorage(Building storage) 
    {
        this.storage = storage;
        Update?.Invoke();
    }
}