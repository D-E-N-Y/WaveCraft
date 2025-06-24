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

    [SerializeField] private Toggle ui_autoGetTaskToggle;

    [SerializeField] private Button ui_stopButton;
    [SerializeField] private Button ui_continueButton;
    [SerializeField] private Button ui_addTaskButton;

    [SerializeField] private UI_TasksListPanel ui_tasksListPanel;

    public override void Initialize(Actor _actor)
    {
        base.Initialize(_actor);

        if (worker != null)
        {
            RemoveSubscriptions();
            worker = null;

            ui_tasksListPanel.Hide();
        }

        worker = (UV_Worker)_actor;
        AddSubscriptions();

        UpdateData();
        UpdateTasks();
        UpdateAutoGetTask();
        UpdateStateTask();
    }

    void OnDisable()
    {
        RemoveSubscriptions();
        ui_tasksListPanel.Hide();
    }

    private void UpdateData()
    {
        ui_proffesion.text = worker.Profession().ToString();

        ui_armor.text = worker.GetArmor().ToString();
        ui_damage.text = worker.GetDamage().ToString();
        ui_speed.text = worker.GetSpeed().ToString();
        ui_storage.text = worker.GetMaxMineAmount().ToString();
    }

    private void UpdateTasks()
    {
        ui_tasks.ForEach(x => x.Hide());

        for (int i = 0; i < worker.tasks.Count; i++)
        {
            ui_taskContainer.transform.GetChild(i).gameObject.SetActive(true);
            ui_taskContainer.transform.GetChild(i).gameObject.GetComponent<UI_TaskWorker>().Initialize(worker, i + 1, worker.tasks[i]);
        }

        bool isHasTask = worker.tasks.Count > 0;
        ui_continueButton.interactable = isHasTask;
        ui_stopButton.interactable = isHasTask;
    }

    private void UpdateStateTask()
    {
        ui_continueButton.gameObject.SetActive(worker.isStopTask);
        ui_stopButton.gameObject.SetActive(!worker.isStopTask);
    }

    private void UpdateAutoGetTask()
    {
        ui_autoGetTaskToggle.isOn = worker.isAutoGetTask;
    }

    private void ShowTasksListPanel()
    {
        ui_tasksListPanel.Show();
        ui_tasksListPanel.Initialize(worker);
    }
    
    private void AddSubscriptions()
    {
        worker.UpdateTasks += UpdateTasks;

        worker.UpdateAutoGetTaskTask += UpdateAutoGetTask;
        ui_autoGetTaskToggle.onValueChanged.AddListener(worker.ChangeAutoGetTasks);

        worker.UpdateStateTask += UpdateStateTask;
        ui_stopButton.onClick.AddListener(() => worker.StopTask());
        ui_continueButton.onClick.AddListener(() => worker.ContinueTask());

        ui_addTaskButton.onClick.AddListener(() => ShowTasksListPanel());
    }

    private void RemoveSubscriptions()
    {
        worker.UpdateTasks -= UpdateTasks;

        worker.UpdateAutoGetTaskTask -= UpdateAutoGetTask;
        ui_autoGetTaskToggle.onValueChanged.RemoveAllListeners();

        worker.UpdateStateTask -= UpdateStateTask;
        ui_stopButton.onClick.RemoveAllListeners();
        ui_continueButton.onClick.RemoveAllListeners();

        ui_addTaskButton.onClick.RemoveAllListeners();
    }
}