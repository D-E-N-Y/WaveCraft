public class Task
{
    public E_TaskType type { protected set; get; }
    public ETaskStatus status { protected set; get; }

    public void SetStatus(ETaskStatus status) => this.status = status;
}
