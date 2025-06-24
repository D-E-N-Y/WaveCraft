using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WorkerDescription : UI_VillageDescription
{
    [SerializeField] private TextMeshProUGUI ui_currentMineStoreAmount;
    [SerializeField] private TextMeshProUGUI ui_maxMineStoreAmount;

    [SerializeField] private TextMeshProUGUI ui_woodMineAmount;
    [SerializeField] private TextMeshProUGUI ui_stoneMineAmount;
    [SerializeField] private TextMeshProUGUI ui_foodMineAmount;

    [SerializeField] private TextMeshProUGUI ui_woodCarryingAmount;
    [SerializeField] private TextMeshProUGUI ui_stoneCarryingAmount;
    [SerializeField] private TextMeshProUGUI ui_foodCarryingAmount;

    [SerializeField] private GameObject ui_taskContainer;
    [SerializeField] private List<UI_TaskWorker> ui_tasks;

    [SerializeField] private UI_TasksListPanel ui_tasksListPanel;

    [SerializeField] private Toggle ui_autoGetTaskToggle;

    [SerializeField] private Button ui_stopButton;
    [SerializeField] private Button ui_continueButton;
    [SerializeField] private Button ui_addTaskButton;

    private UV_Worker worker;

    public override void InitializeVillage(U_Village village)
    {
        base.InitializeVillage(village);

        if (worker != null)
        {
            RemoveSubscriptions();
            ui_tasksListPanel.Hide();

            worker = null;
        }

        worker = (UV_Worker)village;
        AddSubscriptions();

        UpdateResourceAmount();
        UpdateTasks();
        UpdateAutoGetTask();
        UpdateStateTask();
    }

    private void OnDisable()
    {
        RemoveSubscriptions();
        ui_tasksListPanel.Hide();
    }

    private void UpdateResourceAmount()
    {
        ui_currentMineStoreAmount.text = worker.GetCurrentMineAmount().ToString();
        ui_maxMineStoreAmount.text = worker.GetMaxMineAmount().ToString();

        // mined resources
        ui_woodMineAmount.text = worker.GetCurrentMineAmountByResource(E_Resource.Wood).ToString();
        ui_stoneMineAmount.text = worker.GetCurrentMineAmountByResource(E_Resource.Stone).ToString();
        ui_foodMineAmount.text = worker.GetCurrentMineAmountByResource(E_Resource.Food).ToString();

        // carrying resources
        ui_woodCarryingAmount.text = worker.GetCurrentCarryingAmountByResource(E_Resource.Wood).ToString();
        ui_stoneCarryingAmount.text = worker.GetCurrentCarryingAmountByResource(E_Resource.Stone).ToString();
        ui_foodCarryingAmount.text = worker.GetCurrentCarryingAmountByResource(E_Resource.Food).ToString();
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

    private void UpdateAutoGetTask()
    {
        ui_autoGetTaskToggle.isOn = worker.isAutoGetTask;
    }

    private void UpdateStateTask()
    {
        ui_continueButton.gameObject.SetActive(worker.isStopTask);
        ui_stopButton.gameObject.SetActive(!worker.isStopTask);
    }

    private void ShowTasksListPanel()
    {
        ui_tasksListPanel.Show();
        ui_tasksListPanel.Initialize(worker);
    }

    private void AddSubscriptions()
    {
        worker.UpdateResourceAmount += UpdateResourceAmount;

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
        worker.UpdateResourceAmount -= UpdateResourceAmount;

        worker.UpdateTasks -= UpdateTasks;

        worker.UpdateAutoGetTaskTask -= UpdateAutoGetTask;
        ui_autoGetTaskToggle.onValueChanged.RemoveAllListeners();

        worker.UpdateStateTask -= UpdateStateTask;
        ui_stopButton.onClick.RemoveAllListeners();
        ui_continueButton.onClick.RemoveAllListeners();

        ui_addTaskButton.onClick.RemoveAllListeners();
    }
}
