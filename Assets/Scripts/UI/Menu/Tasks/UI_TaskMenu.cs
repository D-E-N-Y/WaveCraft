using UnityEngine;

public class UI_TaskMenu : UIMenu
{
    [SerializeField] private UI_TypeFilter ui_typeFilter;
    [SerializeField] private UI_StatusFilter ui_statusFilter;

    [SerializeField] private UI_TasksContainer ui_tasksContainer;
    [SerializeField] private UI_WorkersContainer ui_workersContainer;

    [SerializeField] private UI_BuildDescription ui_buildDescription;
    [SerializeField] private UI_DestroyDescription ui_destroyDescription;
    [SerializeField] private UI_StoreDescription ui_storeDescription;
    [SerializeField] private UI_MineDescription ui_mineDescription;

    public void Initialize()
    {
        ui_typeFilter.Initialize();
        ui_statusFilter.Initialize();

        ui_workersContainer.Initialize();
        ui_tasksContainer.Initialize(this, ui_workersContainer, ui_statusFilter, ui_typeFilter);

        ui_buildDescription.Initialize(this);
        ui_destroyDescription.Initialize(this);
        ui_storeDescription.Initialize(this);
        ui_mineDescription.Initialize(this);
    }
}