using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WorkerSlot : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI ui_name;
    [SerializeField] private TextMeshProUGUI ui_freeSlots;
    
    [SerializeField] private Color normal, select;

    [SerializeField] private Button button;

    public UP_Worker worker { get; private set; }
    public Task openTask;

    public void Initialize(UP_Worker worker, Task task)
    {
        this.worker = worker;
        openTask = task;

        ui_name.text = worker.nameActor;
        ui_freeSlots.text = worker.GetFreeSlots().ToString();

        button.onClick.RemoveAllListeners();
        if(openTask != null && openTask.worker != worker)
        {
            if(openTask.worker != null)
            {
                button.onClick.AddListener(() => openTask.worker.CancelTask(openTask));
            }
            
            button.onClick.AddListener(() => TaskSystem.current.DoTask(openTask, worker));
        }
    }

    public void Select()
    {
        button.image.color = select;
    }

    public void UnSelect()
    {
        button.image.color = normal;
    }
}