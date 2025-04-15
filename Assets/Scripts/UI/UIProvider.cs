using System.Collections.Generic;
using UnityEngine;

public class UIProvider : MonoBehaviour 
{
    [SerializeField] private List<UIPanel> ui_prefabs;
    public Dictionary<string, UIPanel> panels;

    void Start()
    {
        panels = new Dictionary<string, UIPanel>();
        
        foreach(UIPanel ui_panel in ui_prefabs)
        {
            panels.Add(ui_panel.name, ui_panel);
        }

        UISystem.current.Initialize(this);
    }

    public UIPanel GetPanelByName(string name)
    {
        return panels.ContainsKey(name) ? panels[name] : null;
    }
}