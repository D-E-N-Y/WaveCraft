public class StoreTask : Task
{
    public E_Resource resource { private set; get; }
    public I_Processor processor { private set; get; }

    public StoreTask(E_Resource resource, I_Processor processor)
    {
        taskType = E_TaskType.Store;

        this.resource = resource;
        this.processor = processor;
    }
}