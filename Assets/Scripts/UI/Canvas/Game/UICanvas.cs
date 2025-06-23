using System.Collections;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [SerializeField] private UIResourcePanel ui_resourcePanel;
    [SerializeField] private ManagerInteractablePanels managerInteractablePanels;

    [SerializeField] private UI_BuildMenu ui_buildMenu;
    [SerializeField] private UI_TaskMenu ui_taskMenu;
    [SerializeField] private UI_VillageMenu ui_villageMenu;
    [SerializeField] private UIGameMenu ui_GameMenu;

    [SerializeField] private UIProvider uiProvider;
    [SerializeField] private UIInput uiInput;

    public IEnumerator Initializing(UIBlackout ui_Blackout, UILoadingScreen ui_loadingScreen)
    {
        ui_loadingScreen.SetMaxPartProgress(8);

        ui_resourcePanel.Initialize();
        ui_loadingScreen.AddPartProgress();
        yield return null;

        managerInteractablePanels.Initialize();
        ui_loadingScreen.AddPartProgress();
        yield return null;

        ui_buildMenu.Initialize();
        ui_loadingScreen.AddPartProgress();
        yield return null;

        ui_taskMenu.Initialize();
        ui_loadingScreen.AddPartProgress();
        yield return null;

        ui_villageMenu.Initialize();
        ui_loadingScreen.AddPartProgress();
        yield return null;

        ui_GameMenu.Initialize(ui_Blackout);
        ui_loadingScreen.AddPartProgress();
        yield return null;

        uiProvider.Initialize();
        ui_loadingScreen.AddPartProgress();
        yield return null;

        uiInput.Initialize();
        ui_loadingScreen.AddPartProgress();
        yield return null;

        gameObject.SetActive(true);
    }
}