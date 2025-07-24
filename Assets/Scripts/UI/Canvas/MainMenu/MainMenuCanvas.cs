using UnityEngine;

public class MainMenuCanvas : MonoBehaviour
{
    [SerializeField] private UIBlackout ui_blackout;
    [SerializeField] private UIMainMenu ui_mainMenu;

    public void Initialize()
    {
        ui_blackout.Initialize();
        ui_mainMenu.Initialize(ui_blackout);

        ui_blackout.Down();
    }
}
