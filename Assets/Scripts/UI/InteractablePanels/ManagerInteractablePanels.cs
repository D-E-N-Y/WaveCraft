using System;
using System.Collections.Generic;
using UnityEngine;

public class ManagerInteractablePanels : MonoBehaviour
{
    [SerializeField] private List<UI_InteractablePanel> ui_panels;
    private Dictionary<Type, UI_InteractablePanel> panelByType = new();
    private UI_InteractablePanel openPanel;

    private void Start() 
    {
        foreach(UI_InteractablePanel panel in ui_panels)
        {
            if (panel.PanelType != null && !panelByType.ContainsKey(panel.PanelType))
            {
                panelByType.Add(panel.PanelType, panel);
            }
        }
    }

    private void OnEnable()
    {
        InteractionSystem.current.Select += OpenPanel;
        InteractionSystem.current.UnSelect += ClosePanel;
    }

    private void OnDisable() 
    {
        InteractionSystem.current.Select -= OpenPanel;
        InteractionSystem.current.UnSelect -= ClosePanel;
    }

    private void OpenPanel(Actor actor)
    {
        Type _type = actor.GetType();

        if(actor is I_Processor)
        {
            _type = typeof(I_Processor);
        }

        if(actor is I_Storage)
        {
            _type = typeof(I_Storage);
        }

        if(actor is I_Production)
        {
            _type = typeof(I_Production);
        }

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