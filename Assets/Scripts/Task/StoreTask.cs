public class StoreTask : Task
{
    public E_Resource resource { private set; get; }
    public IProcessor processor { private set; get; }

    public StoreTask(E_Resource resource, IProcessor processor)
    {
        taskType = E_TaskType.Store;

        this.resource = resource;
        this.processor = processor;
    }
}