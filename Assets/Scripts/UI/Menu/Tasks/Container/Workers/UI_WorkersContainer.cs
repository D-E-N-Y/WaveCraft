using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_WorkersContainer : MonoBehaviour 
{
    [SerializeField] private GameObject ui_workerPref;
    private List<UI_WorkerSlot> workers;
    private Task openTask;

    private TaskSystem taskSystem;
    private VillageSystem villageSystem;

    public void Initialize()
    {
        taskSystem = TaskSystem.current;
        villageSystem = VillageSystem.current;
    }

    private void OnEnable()
    {
        taskSystem.UpdateWorkers += UpdateWorkers;
        taskSystem.UpdateTasks += UpdateWorkers;
        openTask = null;

        UpdateWorkers();
    }
    private void OnDisable()
    {
        taskSystem.UpdateWorkers -= UpdateWorkers;
        taskSystem.UpdateTasks -= UpdateWorkers;
    }

    private void UpdateWorkers()
    {
        workers = new List<UI_WorkerSlot>();
        
        int dif = villageSystem.GetCount(EVillageProfession.Worker) - transform.GetComponentsInChildren<UI_WorkerSlot>(true).Length;
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

        if(villageSystem.GetCount(EVillageProfession.Worker) == 0) return;

        for(int i = 0; i < villageSystem.GetCount(EVillageProfession.Worker); i++)
        {
            UV_Worker _worker = (UV_Worker)villageSystem.GetVillages(EVillageProfession.Worker)[i];
            
            
            if(!_worker.HasFreeTaskSpace())
            {
                if(!(openTask != null && openTask.worker == _worker))
                {
                    continue;
                }
            }

            workers[i].Initialize(_worker, openTask);
            workers[i].gameObject.SetActive(true);
        }

        workers = workers.
            Where(x => x.worker != null).
            OrderByDescending(x => x.worker.GetFreeSlots()).
            ToList();

        int start = 0;
        if(openTask != null)
        {
            UI_WorkerSlot usedWorker = workers
                .Where(x => x.worker == openTask.worker)
                .FirstOrDefault();

            if(usedWorker != null)
            {
                workers.Remove(usedWorker);

                usedWorker.Select();
                usedWorker.transform.SetSiblingIndex(start);
                start++;
            }
        }

        for(int i = 0; i < workers.Count; i++)
        {   
            workers[i].transform.SetSiblingIndex(start + i);
            workers[i].UnSelect();
        }
    }

    public void SetTask(Task task)
    {
        openTask = task;
        UpdateWorkers();
    }
}