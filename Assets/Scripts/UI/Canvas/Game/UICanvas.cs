using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [SerializeField] private UIResourcePanel ui_resourcePanel;
    [SerializeField] private ManagerInteractablePanels managerInteractablePanels;

    [SerializeField] private UI_BuildMenu ui_buildMenu;
    [SerializeField] private UI_TaskMenu ui_taskMenu;
    [SerializeField] private UIGameMenu ui_GameMenu;

    [SerializeField] private UIProvider uiProvider;
    [SerializeField] private UIInput uiInput;

    public void Initialize(UIBlackout ui_Blackout)
    {
        ui_resourcePanel.Initialize();
        managerInteractablePanels.Initialize();

        ui_buildMenu.Initialize();
        ui_taskMenu.Initialize();
        ui_GameMenu.Initialize(ui_Blackout);

        uiProvider.Initialize();
        uiInput.Initialize();

        gameObject.SetActive(true);
    }
}