public class Task
{
    public E_TaskType type { protected set; get; }
    public E_TaskState state { protected set; get; }

    public UP_Worker worker { protected set; get; }

    public void SetState(E_TaskState state) => this.state = state;
    
    public void SetWorker(UP_Worker worker) => this.worker = worker;
    public void ResetWorker() => worker = null;
}
