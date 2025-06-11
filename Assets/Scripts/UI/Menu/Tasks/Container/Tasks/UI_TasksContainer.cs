using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_TasksContainer : MonoBehaviour 
{
    [SerializeField] private GameObject ui_taskPref;
    private List<UI_Task> tasks;

    private UI_TaskMenu ui_taskMenu;

    [System.Serializable]
    private struct InfoType
    {
        public E_TaskType _type;
        public UI_TaskDescription info;
    }

    [SerializeField] private List<InfoType> ui_info;
    private Dictionary<E_TaskType, UI_TaskDescription> infoType;

    private UI_WorkersContainer ui_freeWorkers;
    private UI_StatusFilter ui_statusFilter;
    private UI_TypeFilter ui_typeFilter;

    private TaskSystem taskSystem;

    private void OnEnable()
    {
        taskSystem.UpdateTasks += TasksUpdate;
        ui_statusFilter.update += TasksUpdate;
        ui_typeFilter.update += TasksUpdate;

        TasksUpdate();
    }

    private void OnDisable()
    {
        taskSystem.UpdateTasks -= TasksUpdate;
        ui_statusFilter.update -= TasksUpdate;
        ui_typeFilter.update -= TasksUpdate;
    }

    public void Initialize(UI_TaskMenu ui_taskMenu, UI_WorkersContainer ui_freeWorkers, UI_StatusFilter ui_statusFilter, UI_TypeFilter ui_typeFilter)
    {
        this.ui_taskMenu = ui_taskMenu;
        this.ui_freeWorkers = ui_freeWorkers;
        this.ui_statusFilter = ui_statusFilter;
        this.ui_typeFilter = ui_typeFilter;

        taskSystem = TaskSystem.current;
        infoType = new Dictionary<E_TaskType, UI_TaskDescription>();

        foreach(InfoType current in ui_info)
        {
            infoType.Add(current._type, current.info);
        }
    }


    private void TasksUpdate()
    {
        tasks = new List<UI_Task>();
        
        int dif = taskSystem.GetCount() - transform.GetComponentsInChildren<UI_Task>(true).Length;
        if(dif > 0)
        {
            for(int i = 0; i < dif; i++)
            {
                Instantiate(ui_taskPref, transform).GetComponent<UI_Task>();
            }
        }

        foreach(UI_Task task in transform.GetComponentsInChildren<UI_Task>(true))
        {
            tasks.Add(task);
            task.gameObject.SetActive(false);
        }

        if(taskSystem.GetCount() == 0) return;

        int taskIndex = 0;
        foreach (E_TaskState type in Enum.GetValues(typeof(E_TaskState)))
        {
            if (ui_statusFilter.values[type])
            {
                var stateTasks = taskSystem.GetTasks(type);
                
                for (int i = 0; i < stateTasks.Count; i++)
                {
                    InitializeTask(tasks[taskIndex], type, i);
                    taskIndex++;
                }
            }
        }
    }

    private void InitializeTask(UI_Task ui_task, E_TaskState state, int i)
    {
        ui_task.Initialize(taskSystem.GetTasks(state)[i]);
        
        if(!ui_typeFilter.values[ui_task.task.type]) return;

        ui_task.gameObject.SetActive(true);
        
        Button _btn = ui_task.transform.GetComponentInChildren<Button>();
        _btn.onClick.RemoveAllListeners();
        _btn.onClick.AddListener(() => infoType[ui_task.task.type].InitializeTask(ui_task.task));
        _btn.onClick.AddListener(() => ui_taskMenu.OpenSection(infoType[ui_task.task.type].gameObject));
        _btn.onClick.AddListener(() => ui_taskMenu.SelectTabSection(ui_task.transform.GetChild(0).gameObject));
        _btn.onClick.AddListener(() => ui_freeWorkers.SetTask(ui_task.task));
    }
}