using System;
using System.Collections.Generic;
using UnityEngine;

public class ManagerInteractablePanels : MonoBehaviour
{
    [SerializeField] private List<UI_InteractablePanel> ui_panels;
    private Dictionary<Type, UI_InteractablePanel> panelByType = new();
    private UI_InteractablePanel openPanel;

    private void Awake() 
    {
        if(InteractionSystem.current)
        {
            InteractionSystem.current.Select += OpenPanel;
            InteractionSystem.current.UnSelect += ClosePanel;
        }
        else
        {
            Debug.Log("interaction system is null");
        }

        foreach(UI_InteractablePanel panel in ui_panels)
        {
            if (panel.PanelType != null && !panelByType.ContainsKey(panel.PanelType))
            {
                panelByType.Add(panel.PanelType, panel);
            }
        }
    }

    private void OnDisable() 
    {
        if(InteractionSystem.current)
        {
            InteractionSystem.current.Select -= OpenPanel;
            InteractionSystem.current.UnSelect -= ClosePanel;
        }
        else
        {
            Debug.Log("interaction system is null");
        }
    }

    private void OpenPanel(Actor actor)
    {
        Type _type = actor.GetType();

        if(panelByType.ContainsKey(_type))
        {
            panelByType[_type].Show();
            panelByType[_type].Initialize(actor);
            openPanel = panelByType[_type];
        }
        else
        {
            Debug.Log($"Not found panel for {_type}");
        }
    }

    private void ClosePanel()
    {
        if(openPanel)
        {
            openPanel.Hide();
            openPanel = null;
        }
    }
}