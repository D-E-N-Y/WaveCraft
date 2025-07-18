using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITaskHandler
{
    IEnumerator ExecuteTask(UV_Worker worker, Task task, Action onComplete);
    IEnumerator Moving(UV_Worker worker, IPosition iPosition, E_MoveTo to);
}