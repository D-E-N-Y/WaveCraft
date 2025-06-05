using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlacedBuilding : UI_InteractablePanel
{
    public override Type PanelType => typeof(Building);

    [SerializeField] private Button ui_build, ui_cancel;

    private TaskSystem taskSystem;
    private Building building;

    public override void Initialize(Actor _actor)
    {
        base.Initialize(_actor);

        if (taskSystem != null)
            taskSystem.UpdateTasks -= UpdateInfo;

        taskSystem = TaskSystem.current;
        taskSystem.UpdateTasks += UpdateInfo;

        building = (Building)_actor;

        UpdateInfo();
    }

    private void UpdateInfo()
    {
        ui_build.onClick.RemoveAllListeners();
        ui_build.interactable = false;

        ui_cancel.onClick.RemoveAllListeners();
        ui_cancel.interactable = false;

        if (taskSystem.HasBuildingInBuildTask(building))
        {
            ui_cancel.onClick.AddListener(() => Cancel());
            ui_cancel.interactable = true;
        }
        else
        {
            ui_build.onClick.AddListener(() => Build());
            ui_build.interactable = true;
        }
    }

    private void Build()
    {
        BuildTask task = new BuildTask(building);
        taskSystem.AddTask(task);
    }

    private void Cancel()
    {
        taskSystem.RemoveTask(taskSystem.GetBuildTaskForBuilding(building));
        ResourceSystem.current.AddResources(building.GetCost());
        InteractionSystem.current.UnSelectActor();
        building.TakeDamage(99999);
    }
    
    private void OnDisable()
    {
        taskSystem.UpdateTasks -= UpdateInfo;
    }
}