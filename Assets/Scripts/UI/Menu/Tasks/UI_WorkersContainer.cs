using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_WorkersContainer : MonoBehaviour 
{
    [SerializeField] private GameObject ui_workerPref;
    private List<UI_WorkerSlot> workers;

    private void OnEnable()
    {
        TaskSystem.current.UpdateWorkers += UpdateWorkers;
        TaskSystem.current.UpdateTasks += UpdateWorkers;

        UpdateWorkers();
    }
    private void OnDisable()
    {
        TaskSystem.current.UpdateWorkers -= UpdateWorkers;
        TaskSystem.current.UpdateTasks -= UpdateWorkers;
    }

    private void UpdateWorkers()
    {
        workers = new List<UI_WorkerSlot>();
        
        int dif = VillageSystem.current.GetCount(EVillageType.Worker) - transform.GetComponentsInChildren<UI_WorkerSlot>(true).Length;
        if(dif > 0)
        {
            for(int i = 0; i < dif; i++)
            {
                Instantiate(ui_workerPref, transform).GetComponent<UI_WorkerSlot>();
            }
        }

        foreach(UI_WorkerSlot worker in transform.GetComponentsInChildren<UI_WorkerSlot>(true))
        {
            workers.Add(worker);
            worker.gameObject.SetActive(false);
        }

        if(VillageSystem.current.GetCount(EVillageType.Worker) == 0) return;

        for(int i = 0; i < VillageSystem.current.GetCount(EVillageType.Worker); i++)
        {
            UP_Worker _worker = (UP_Worker)VillageSystem.current.GetVillages(EVillageType.Worker)[i];
            
            if(!_worker.HasFreeTaskSpace()) continue;

            workers[i].Initialize(_worker);
            workers[i].gameObject.SetActive(true);
        }

        workers = workers.
            Where(x => x.worker != null).
            OrderByDescending(x => x.worker.GetFreeSlots()).
            ToList();

        for(int i = 0; i < workers.Count; i++)
        {   
            workers[i].transform.SetSiblingIndex(i);
        }
    }
}