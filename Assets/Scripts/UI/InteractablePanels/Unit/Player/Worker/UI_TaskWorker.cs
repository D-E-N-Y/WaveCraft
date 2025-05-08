using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TaskWorker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI taskOrderText;
    [SerializeField] private Image taskImage;

    [Serializable]
    private struct STask
    {
        public Sprite sprite;
        public E_TaskType task;
    }
    [SerializeField] private List<STask> taskImages; 

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contexText;

    private UP_Worker worker;
    private Task task;

    public void Initialize(UP_Worker worker, int order, Task task)
    {
        this.worker = worker;
        this.task = task;

        taskOrderText.text = order.ToString();        
        taskImage.sprite = taskImages.FirstOrDefault(t => t.task == task.type).sprite;
        titleText.text = $"{task.type} task";

        switch (task)
        {
            case BuildTask buildTask:
                contexText.text = $"{task.type} {buildTask.building.nameActor}";
                break;

            case DestroyTask destroyTask:
                contexText.text = $"{task.type} {destroyTask.building.nameActor}";
                break;

            case MiningTask miningTask:
                contexText.text = $"{task.type} {miningTask.resourceType} {miningTask.goal}";
                break;

            case StoreTask storeTask:
                contexText.text = $"{task.type} {storeTask.resource}";
                break;

            default:
                contexText.text = $"{task.type} (Unknown Task)";
                break;
        }

    }

    public void CancelTask()
    {
        worker.CancelTask(task);
    }
}
