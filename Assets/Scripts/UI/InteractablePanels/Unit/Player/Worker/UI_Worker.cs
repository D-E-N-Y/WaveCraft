using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Worker : UI_InteractablePanel 
{
    public override Type PanelType => typeof(UV_Worker);
    private UV_Worker worker;
    
    [SerializeField] private TextMeshProUGUI ui_proffesion;

    [SerializeField] private TextMeshProUGUI ui_armor; 
    [SerializeField] private TextMeshProUGUI ui_damage;
    [SerializeField] private TextMeshProUGUI ui_speed;
    [SerializeField] private TextMeshProUGUI ui_storage;

    [SerializeField] private GameObject ui_taskContainer;
    [SerializeField] private List<UI_TaskWorker> ui_tasks;

    [SerializeField] private TextMeshProUGUI ui_stateTask;

    [SerializeField] private UI_TasksListPanel ui_tasksListPanel;

    [SerializeField] private Image ui_trueImage;
    [SerializeField] private Image ui_falseImage;

    public override void Initialize(Actor _actor)
    {
        base.Initialize(_actor);

        worker = (UV_Worker)_actor;

        ui_proffesion.text = worker.Profession().ToString();

        ui_armor.text = worker.GetArmor().ToString();
        ui_damage.text = worker.GetDamage().ToString();
        ui_speed.text = worker.GetSpeed().ToString();
        ui_storage.text = worker.GetMaxMineAmount().ToString();
        
        ui_trueImage.gameObject.SetActive(worker.isAutoGetTask);
        ui_falseImage.gameObject.SetActive(!worker.isAutoGetTask);

        RefreshTasks();
        worker.UpdateTasks += RefreshTasks;
        worker.UpdateState += RefreshState;

        RefreshState();
    }

    void OnDisable()
    {
        worker.UpdateTasks -= RefreshTasks;
        worker.UpdateState -= RefreshState;
        ui_tasksListPanel.Hide();
    }

    private void RefreshTasks()
    {
        ui_tasks.ForEach(t => t.gameObject.SetActive(false));

        for(int i = 0; i < worker.tasks.Count; i++)
        {
            ui_taskContainer.transform.GetChild(i).gameObject.SetActive(true);
            ui_taskContainer.transform.GetChild(i).gameObject.GetComponent<UI_TaskWorker>().Initialize(worker, i + 1, worker.tasks[i]);
        }
    }

    public void RefreshState()
    {
        if(worker.isStopTask)
        {
            ui_stateTask.text = "Continue";
        }
        else
        {
            ui_stateTask.text = "Stop";
        }
    }

    public void SetStateTask()
    {
        if(!worker.isStopTask)
        {
            worker.StopTask();
        }
        else
        {
            worker.ContinueTask();
        }
    }

    public void CheckAutoGetTasks()
    {
        worker.ChangeAutoGetTasks();

        ui_trueImage.gameObject.SetActive(worker.isAutoGetTask);
        ui_falseImage.gameObject.SetActive(!worker.isAutoGetTask);
    }

    public void ShowTasksListPanel()
    {
        ui_tasksListPanel.gameObject.SetActive(true);
        ui_tasksListPanel.Initialize(worker);
    }
}