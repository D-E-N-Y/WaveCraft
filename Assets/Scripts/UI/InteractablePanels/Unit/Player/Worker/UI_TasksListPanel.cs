using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_TasksListPanel : MonoBehaviour
{
    [SerializeField] private List<UI_Task> taskPrefabs;
    private UV_Worker worker;

    public void Initialize(UV_Worker worker)
    {
        this.worker = worker;
        
        RefreshTasks();
        TaskSystem.current.UpdateTasks += RefreshTasks;
    }

    private void OnDisable()
    {
        TaskSystem.current.UpdateTasks -= RefreshTasks;
    }

    public void TakeTask(UI_Task ui_task)
    {
        if(!worker.HasFreeTaskSpace()) return;

        TaskSystem.current.DoTask(ui_task.task, worker);
    }

    private void RefreshTasks()
    {
        taskPrefabs.ForEach(x => x.gameObject.SetActive(false));

        List<Task> avaliableTasks = TaskSystem.current.GetTasks(E_TaskState.Pending).Concat(TaskSystem.current.GetTasks(E_TaskState.Canceled)).ToList();
        for(int i = 0; i < avaliableTasks.Count; i++)
        {
            taskPrefabs[i].gameObject.SetActive(true);
            taskPrefabs[i].Initialize(avaliableTasks[i]);
        }
    }

    public void Hide() => gameObject.SetActive(false);
}
