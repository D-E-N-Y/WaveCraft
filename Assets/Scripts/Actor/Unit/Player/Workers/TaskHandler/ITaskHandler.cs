using System;
using System.Collections;

public interface ITaskHandler
{
    IEnumerator ExecuteTask(UP_Worker worker, Task task, Action onComplete);
}