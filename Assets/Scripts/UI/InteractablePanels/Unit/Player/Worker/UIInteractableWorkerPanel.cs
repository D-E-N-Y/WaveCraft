using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInteractableWorkerPanel : MonoBehaviour 
{
    private UP_Worker worker;
    
    [SerializeField] private TextMeshProUGUI proffesionText;
    [SerializeField] private TextMeshProUGUI nameText;
    
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI armorText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI storageText;

    [SerializeField] private GameObject taskContainer;
    [SerializeField] private List<UI_TaskWorker> tasks;

    [SerializeField] private TextMeshProUGUI stateTaskText;

    [SerializeField] private UI_TasksListPanel tasksListPanel;

    [SerializeField] private Image ui_trueImage;
    [SerializeField] private Image ui_falseImage;

    public void Initialize(UP_Worker worker)
    {
        this.worker = worker;

        proffesionText.text = worker.proffesion;
        nameText.text = worker.nameActor;

        hpText.text = worker.GetCurrentHP().ToString();
        armorText.text = worker.GetArmor().ToString();
        damageText.text = worker.GetDamage().ToString();
        speedText.text = worker.GetSpeed().ToString();
        storageText.text = worker.GetMaxMineAmount().ToString();
        
        ui_trueImage.gameObject.SetActive(worker.isAutoGetTask);
        ui_falseImage.gameObject.SetActive(!worker.isAutoGetTask);

        RefreshTasks();
        worker.UpdateTasks += RefreshTasks;
        
        if(worker.isStopTask)
        {
            stateTaskText.text = "Continue";
        }
        else
        {
            stateTaskText.text = "Stop";
        }
    }

    void OnDisable()
    {
        worker.UpdateTasks -= RefreshTasks;
        tasksListPanel.Hide();
    }

    private void RefreshTasks()
    {
        tasks.ForEach(t => t.gameObject.SetActive(false));

        for(int i = 0; i < worker.tasks.Count; i++)
        {
            taskContainer.transform.GetChild(i).gameObject.SetActive(true);
            taskContainer.transform.GetChild(i).gameObject.GetComponent<UI_TaskWorker>().Initialize(worker, i + 1, worker.tasks[i]);
        }
    }

    public void SetStateTask()
    {
        if(!worker.isStopTask)
        {
            stateTaskText.text = "Continue";
            worker.StopTask();
        }
        else
        {
            stateTaskText.text = "Stop";
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
        tasksListPanel.gameObject.SetActive(true);
        tasksListPanel.Initialize(worker);
    }
}