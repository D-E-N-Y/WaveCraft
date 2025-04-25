public class StoreTask : Task
{
    public E_Resource resource { private set; get; }
    public IProcessor processor { private set; get; }
    public IProduction production { private set; get; }

    public StoreTask(E_Resource resource, IProcessor processor)
    {
        type = E_TaskType.Store;

        this.resource = resource;
        this.processor = processor;
    }

    public StoreTask(E_Resource resource, IProduction production)
    {
        type = E_TaskType.Store;

        this.resource = resource;
        this.production = production;
    }
}