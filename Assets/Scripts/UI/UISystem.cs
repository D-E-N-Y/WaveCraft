using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UISystem : GameSystem
{
    [SerializeField] private InputActionReference inputAction;
    [SerializeField] private GameObject ui_gameMenu;
    private bool isOpenGameMenu;

    private GameObject openPanel;
    
    public override void Initialize()
    {
        base.Initialize();
    }

    void OnEnable()
    {
        inputAction.action.started += Return;
    }

    private void Return(InputAction.CallbackContext context)
    {
        Time.timeScale = isOpenGameMenu ? 1f : 0f;

        isOpenGameMenu = !isOpenGameMenu;
        ui_gameMenu.SetActive(isOpenGameMenu);
    }

    public void OpenPanel(GameObject panel)
    {
        if(openPanel == panel)
        {
            ClosePanel(openPanel);
            return;
        }
        else if(openPanel != null)
        {
            ClosePanel(openPanel);
        }
        
        panel.SetActive(true);
        openPanel = panel;
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
        openPanel = null;
    }

    public void CloseOpenPanel()
    {
        if(openPanel != null)
        {
            ClosePanel(openPanel);
        }
    }
}