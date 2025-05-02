using System;

public class Task
{
    public Action Update;
    
    public E_TaskType type { protected set; get; }
    public E_TaskState state { protected set; get; }
    public UP_Worker worker { protected set; get; }
    public bool isAutoWorker { protected set; get; } = true;

    public void SetState(E_TaskState state)
    {
        this.state = state;
        Update?.Invoke();
    }
    
    public void SetWorker(UP_Worker worker)
    {
        this.worker = worker;
        Update?.Invoke();
    }
    
    public void ResetWorker() 
    {
        worker = null;
        Update?.Invoke();
    }
    
    public void SetAutoWorker(bool value) 
    {
        isAutoWorker = value;
        Update?.Invoke();
    }
}
