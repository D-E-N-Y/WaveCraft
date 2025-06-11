using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UI_TaskDescription : UIPanel
{
    private UI_TaskMenu ui_taskMenu;

    private TaskSystem taskSystem;
    private FocusSystem focusSystem;
    private UISystem uiSystem;

    [SerializeField] protected TextMeshProUGUI ui_progress;
    [SerializeField] protected TextMeshProUGUI ui_goal;

    [SerializeField] private TextMeshProUGUI ui_nameWorker;
    [SerializeField] private Toggle ui_autoWorker;
    [SerializeField] private Button ui_focusToWorker;

    [SerializeField] private Button ui_stop;
    [SerializeField] private Button ui_continue;
    [SerializeField] private Button ui_cancel;
    [SerializeField] private Button ui_remove;

    [SerializeField] private UI_TypeFilter ui_typeFilter;
    [SerializeField] private UI_StatusFilter ui_statusFilter;

    protected Task task;

    private void OnEnable()
    {
        ui_typeFilter.update += VisibleForFilter;
        ui_statusFilter.update += VisibleForFilter;
    }

    private void OnDisable()
    {
        task.Update -= UpdateInfo;
        ui_typeFilter.update -= VisibleForFilter;
        ui_statusFilter.update -= VisibleForFilter;
    }

    public virtual void Initialize(UI_TaskMenu ui_taskMenu)
    {
        this.ui_taskMenu = ui_taskMenu;

        taskSystem = TaskSystem.current;
        focusSystem = FocusSystem.current;
        uiSystem = UISystem.current;
    }

    public virtual void InitializeTask(Task task)
    {
        if (this.task != null) this.task.Update -= UpdateInfo;

        this.task = task;
        this.task.Update += UpdateInfo;

        UpdateInfo();

        ui_remove.onClick.RemoveAllListeners();
        ui_remove.onClick.AddListener(() => taskSystem.RemoveTask(task));
        ui_remove.onClick.AddListener(() => ui_taskMenu.CloseCurrentSection());
    }

    protected virtual void UpdateInfo()
    {
        if (!taskSystem.HasTask(task))
            gameObject.SetActive(false);

        ui_progress.text = task.progress.ToString("F0");
        ui_goal.text = task.goal.ToString();

        UpdateFocusWorkerButton();
        UpdateControls();
    }

    private void UpdateFocusWorkerButton()
    {
        if (task.worker != null)
        {
            ui_nameWorker.text = task.worker.nameActor;

            ui_focusToWorker.onClick.AddListener(() => FocusTo(task.worker));
            ui_focusToWorker.interactable = true;
        }
        else
        {
            ui_nameWorker.text = "none";

            ui_focusToWorker.onClick.RemoveAllListeners();
            ui_focusToWorker.interactable = false;
        }
    }

    private void UpdateControls()
    {
        ui_continue.onClick.RemoveAllListeners();
        ui_continue.interactable = false;

        ui_stop.onClick.RemoveAllListeners();
        ui_stop.interactable = false;

        ui_cancel.onClick.RemoveAllListeners();
        ui_cancel.interactable = false;

        ui_autoWorker.onValueChanged.RemoveAllListeners();
        ui_autoWorker.interactable = false;
        ui_autoWorker.isOn = task.isAutoWorker;
        if (task.state != E_TaskState.Completed)
        {
            ui_autoWorker.interactable = true;
            ui_autoWorker.onValueChanged.AddListener(delegate
            {
                ToggleValueChanged(ui_autoWorker);
            });
        }

        if (task.worker != null)
        {
            if (task.worker.GetCurrentTask() != task) return;

            if (task.worker.isStopTask)
            {
                ui_continue.onClick.AddListener(() => task.worker.ContinueTask());
                ui_continue.gameObject.SetActive(true);
                ui_continue.interactable = true;

                ui_stop.gameObject.SetActive(false);
            }
            else
            {
                ui_stop.onClick.AddListener(() => task.worker.StopTask());
                ui_stop.gameObject.SetActive(true);
                ui_stop.interactable = true;

                ui_continue.gameObject.SetActive(false);
            }

            ui_cancel.onClick.AddListener(() => task.worker.CancelTask(task));
            ui_cancel.interactable = true;
        }
    }

    private void ToggleValueChanged(Toggle change)
    {
        task.SetAutoWorker(ui_autoWorker.isOn);
    }

    protected void FocusTo(Actor actor)
    {
        if (actor == null) return;

        focusSystem.FocusToObject(actor);
        uiSystem.CloseAllPanels();
    }

    protected void SetStatus(GameObject ui_status)
    {
        if (task.state != E_TaskState.Completed)
        {
            ui_status.GetComponent<TextMeshProUGUI>().text = E_TaskState.Pending.ToString();
        }
        else
        {
            ui_status.GetComponent<TextMeshProUGUI>().text = task.state.ToString();
        }

        ui_status.SetActive(true);
    }

    private void VisibleForFilter()
    {
        if (!ui_typeFilter.values[TaskType()] || !ui_statusFilter.values[task.state])
        {
            ui_taskMenu.CloseCurrentSection();
        }
    }
    
    public abstract E_TaskType TaskType();
}