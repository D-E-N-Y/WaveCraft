using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInput : MonoBehaviour 
{
    [SerializeField] private InputActionReference returnAction;

    private UISystem uiSystem;
    private InteractionSystem interactionSystem;
    private BuildSystem buildSystem;

    public void Initialize()
    {
        uiSystem = UISystem.current;
        interactionSystem = InteractionSystem.current;
        buildSystem = BuildSystem.current;
    }

    void OnEnable()
    {
        returnAction.action.started += Return;
    }

    void OnDisable()
    {
        returnAction.action.started -= Return;
    }

    private void Return(InputAction.CallbackContext context)
    {
        if (uiSystem.HasOpenPanels())
        {
            uiSystem.CloseCurrentPanel();
        }
        else if (interactionSystem.HasSelectedActor())
        {
            interactionSystem.UnSelectActor();
        }
        else if (buildSystem.IsPlacing())
        {
            return;
        }
        else
        {
            uiSystem.OpenLastMenu();
        }
    }
}