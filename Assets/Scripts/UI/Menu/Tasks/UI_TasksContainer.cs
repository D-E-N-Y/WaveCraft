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

    private void OnEnable()
    {
        TaskSystem.current.UpdateTasks += Initialize;
        
        Initialize();
    }

    private void OnDisable()
    {
        TaskSystem.current.UpdateTasks -= Initialize;
    }

    private void Initialize()
    {
        infoType = new Dictionary<E_TaskType, GameObject>();

        foreach(InfoType current in ui_info)
        {
            infoType.Add(current._type, current.info);
        }
        
        tasks = new List<UI_Task>();
        
        foreach(UI_Task task in transform.GetComponentsInChildren<UI_Task>())
        {
            tasks.Add(task);
            task.gameObject.SetActive(false);
        }

        if(TaskSystem.current.GetCount() == 0) return;

        int dif = TaskSystem.current.GetCount() - tasks.Count;
        if(dif > 0)
        {
            for(int i = 0; i < dif; i++)
            {
                tasks.Add(Instantiate(ui_taskPref, transform).GetComponent<UI_Task>());
            }
        }

        int start = 0;

        for(int i = start; i < TaskSystem.current.GetTasks(E_TaskState.Pending).Count; i++, start++)
        {
            InitializeTask(tasks[i], E_TaskState.Pending, i);
        }

        for(int i = start; i < TaskSystem.current.GetTasks(E_TaskState.Execured).Count; i++)
        {
            InitializeTask(tasks[i], E_TaskState.Execured, i);
        }
    }

    private void InitializeTask(UI_Task ui_task, E_TaskState state, int i)
    {
        ui_task.Initialize(TaskSystem.current.GetTasks(state)[i]);
        
        ui_task.gameObject.SetActive(true);
        
        Button _btn = ui_task.transform.GetComponentInChildren<Button>();
        
        _btn.onClick.AddListener(() => taskMenu.OpenSection(infoType[ui_task.task.type]));
        
        // _btn.onClick.AddListener(() => taskMenu.OpenSection(ui_freeWorkers));
        _btn.onClick.AddListener(() => taskMenu.SelectTabSection(ui_task.transform.GetChild(0).gameObject));
    }
}