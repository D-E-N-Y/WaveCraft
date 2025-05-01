using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_TasksContainer : MonoBehaviour 
{
    [SerializeField] private GameObject ui_taskPref;
    private List<UI_Task> tasks;

    [SerializeField] private UIMenu taskMenu;

    [System.Serializable]
    private struct InfoType
    {
        public E_TaskType _type;
        public GameObject info;
    }

    [SerializeField] private List<InfoType> ui_info;
    private Dictionary<E_TaskType, GameObject> infoType;

    [SerializeField] private GameObject ui_freeWorkers;

    [SerializeField] private UI_StatusFilter ui_statusFilter;
    [SerializeField] private UI_TypeFilter ui_typeFilter;

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        TaskSystem.current.UpdateTasks += TasksUpdate;
        ui_statusFilter.update += TasksUpdate;
        ui_typeFilter.update += TasksUpdate;

        TasksUpdate();
    }

    private void OnDisable()
    {
        TaskSystem.current.UpdateTasks -= Initialize;
        ui_statusFilter.update -= TasksUpdate;
        ui_typeFilter.update -= TasksUpdate;
    }

    private void Initialize()
    {
        infoType = new Dictionary<E_TaskType, GameObject>();

        foreach(InfoType current in ui_info)
        {
            infoType.Add(current._type, current.info);
        }
        
        ui_statusFilter.Initialize();
        ui_typeFilter.Initialize();
    }


    private void TasksUpdate()
    {
        tasks = new List<UI_Task>();
        
        int dif = TaskSystem.current.GetCount() - transform.GetComponentsInChildren<UI_Task>().Length;
        if(dif > 0)
        {
            for(int i = 0; i < dif; i++)
            {
                Instantiate(ui_taskPref, transform).GetComponent<UI_Task>();
            }
        }

        foreach(UI_Task task in transform.GetComponentsInChildren<UI_Task>())
        {
            tasks.Add(task);
            task.gameObject.SetActive(false);
        }

        if(TaskSystem.current.GetCount() == 0) return;

        int start = 0;
        foreach(E_TaskState type in Enum.GetValues(typeof(E_TaskState)))
        {
            if(ui_statusFilter.values[type])
            {
                for(int i = start; i < TaskSystem.current.GetTasks(type).Count; i++)
                {
                    InitializeTask(tasks[i], type, i - start);
                }

                start += TaskSystem.current.GetTasks(type).Count;
            }
        }
    }

    private void InitializeTask(UI_Task ui_task, E_TaskState state, int i)
    {
        ui_task.Initialize(TaskSystem.current.GetTasks(state)[i]);
        
        if(!ui_typeFilter.values[ui_task.task.type]) return;

        Debug.Log($"{ui_task.task.type} {ui_typeFilter.values[ui_task.task.type]}");

        ui_task.gameObject.SetActive(true);
        
        Button _btn = ui_task.transform.GetComponentInChildren<Button>();
        _btn.onClick.AddListener(() => taskMenu.OpenSection(infoType[ui_task.task.type]));
        _btn.onClick.AddListener(() => taskMenu.SelectTabSection(ui_task.transform.GetChild(0).gameObject));
        // _btn.onClick.AddListener(() => taskMenu.OpenSection(ui_freeWorkers));
    }
}