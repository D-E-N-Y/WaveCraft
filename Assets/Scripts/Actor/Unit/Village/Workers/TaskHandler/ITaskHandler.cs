using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITaskHandler
{
    IEnumerator ExecuteTask(UV_Worker worker, Task task, Action onComplete);
    IEnumerator Moving(UV_Worker worker, IPosition iPosition, UnitMovement.E_MoveTo to);
}