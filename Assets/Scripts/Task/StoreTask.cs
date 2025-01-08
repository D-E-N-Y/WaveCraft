public class StoreTask : Task
{
    public E_Resource resource { private set; get; }
    public I_Processors processor { private set; get; }
    public I_Storage storage { private set; get; }

    public StoreTask(E_Resource resource, I_Processors processor, I_Storage storage)
    {
        taskType = E_TaskType.Store;

        this.resource = resource;
        this.processor = processor;
        this.storage = storage;
    }
}