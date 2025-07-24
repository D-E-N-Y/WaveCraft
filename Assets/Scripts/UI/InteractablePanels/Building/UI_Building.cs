using UnityEngine;
using UnityEngine.UI;

public abstract class UI_Building : UI_InteractablePanel
{
    [SerializeField] Button ui_destroy;
    [SerializeField] Image ui_cancelDestroy;

    protected TaskSystem taskSystem;
    protected Building building;

    public override void InitializeInfo(Actor _actor)
    {
        base.InitializeInfo(_actor);

        building = (Building)_actor;

        if (taskSystem != null)
            taskSystem.UpdateTasks -= UpdateDestroyInfo;

        taskSystem = TaskSystem.current;
        taskSystem.UpdateTasks += UpdateDestroyInfo;

        UpdateDestroyInfo();
    }

    private void UpdateDestroyInfo()
    {
        if (!ui_destroy) return;

        ui_destroy.onClick.RemoveAllListeners();

        if (taskSystem.HasBuildingInDestroyTask(building))
        {
            ui_destroy.onClick.AddListener(() => CancelDestroy());
            ui_cancelDestroy.gameObject.SetActive(true);
        }
        else
        {
            ui_destroy.onClick.AddListener(() => DoDestroy());
            ui_cancelDestroy.gameObject.SetActive(false);
        }
    }

    private void DoDestroy()
    {
        DestroyTask task = new DestroyTask(building);
        taskSystem.AddTask(task);
    }

    private void CancelDestroy()
    {
        DestroyTask task = taskSystem.GetDestroyTaskForBuilding(building);
        taskSystem.RemoveTask(task);
    }

    protected override void UnsubscriptionActions()
    {
        base.UnsubscriptionActions();

        taskSystem.UpdateTasks -= UpdateDestroyInfo;
    }
}
