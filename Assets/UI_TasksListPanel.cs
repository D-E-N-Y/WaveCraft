using System.Collections.Generic;
using UnityEngine;

public class UI_TasksListPanel : MonoBehaviour
{
    [SerializeField] private List<UI_Task> taskPrefabs;
    private UP_Worker worker;

    public void Initialize(UP_Worker worker)
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

        List<Task> pendingTasks = TaskSystem.current.GetTasks(E_TaskState.Pending);
        for(int i = 0; i < pendingTasks.Count; i++)
        {
            taskPrefabs[i].gameObject.SetActive(true);
            taskPrefabs[i].Initialize(pendingTasks[i]);
        }
    }

    public void Hide() => gameObject.SetActive(false);
}
