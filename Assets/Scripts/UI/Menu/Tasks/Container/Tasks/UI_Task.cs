using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Task : MonoBehaviour 
{
    [SerializeField] private Image taskImage;

    [Serializable]
    private struct STask
    {
        public Sprite sprite;
        public E_TaskType task;
    }
    [SerializeField] private List<STask> taskImages; 
    [SerializeField] private TextMeshProUGUI ui_contex;
    [SerializeField] private TextMeshProUGUI ui_status;

    public Task task { get; private set; }

    public void Initialize(Task task)
    {
        this.task = task;
      
        taskImage.sprite = taskImages.FirstOrDefault(t => t.task == task.type).sprite;

        switch (task)
        {
            case BuildTask buildTask:
                ui_contex.text = $"{task.type} {buildTask.building.nameActor}"; 
                break;

            case DestroyTask destroyTask:
                ui_contex.text = $"{task.type} {destroyTask.building.nameActor}";
                break;

            case MiningTask miningTask:
                ui_contex.text = $"{task.type} {miningTask.resourceType} {miningTask.resourceAmount}";
                break;

            case StoreTask storeTask:
                ui_contex.text = $"{task.type} {storeTask.resource}";
                break;

            default:
                ui_contex.text = $"(Unknown Task)";
                break;
        }

        ui_status.text = task.state.ToString();
    }
}