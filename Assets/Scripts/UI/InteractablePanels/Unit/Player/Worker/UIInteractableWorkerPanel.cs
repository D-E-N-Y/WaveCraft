using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    public void Initialize(UP_Worker worker)
    {
        this.worker = worker;

        proffesionText.text = worker.proffesion;
        nameText.text = worker.nameActor;

        hpText.text = worker.GetCurrentHP().ToString();
        armorText.text = worker.GetArmor().ToString();
        damageText.text = worker.GetDamage().ToString();
        speedText.text = worker.GetSpeed().ToString();
        storageText.text = worker.GetMaxAmount().ToString();
        
        RefreshTasks();
        worker.UpdateTasks += RefreshTasks;
    }

    private void RefreshTasks()
    {
        tasks.ForEach(t => t.gameObject.SetActive(false));
        
        for(int i = 0; i < worker.tasks.Count; i++)
        {
            taskContainer.transform.GetChild(i).gameObject.SetActive(true);
            taskContainer.transform.GetChild(i).gameObject.GetComponent<UI_TaskWorker>().Initialize(i + 1, worker.tasks[i]);
        }
    }
}