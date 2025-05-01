using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TypeFilter : UIPanel 
{
    public Action update;

    [SerializeField] private TextMeshProUGUI ui_label;

    [SerializeField] private Toggle ui_build;
    [SerializeField] private Toggle ui_destroy;
    [SerializeField] private Toggle ui_store;
    [SerializeField] private Toggle ui_mine;

    public Dictionary<E_TaskType, bool> values { get; private set; }

    public void Initialize()
    {
        values = new Dictionary<E_TaskType, bool>();
        values[E_TaskType.Build] = ui_build.isOn;
        values[E_TaskType.Destroy] = ui_destroy.isOn;
        values[E_TaskType.Store] = ui_store.isOn;
        values[E_TaskType.Mining] = ui_mine.isOn;

        ui_build.onValueChanged.AddListener(delegate { UpdateToggle(E_TaskType.Build, ui_build); });
        ui_destroy.onValueChanged.AddListener(delegate { UpdateToggle(E_TaskType.Destroy, ui_destroy); });
        ui_store.onValueChanged.AddListener(delegate { UpdateToggle(E_TaskType.Store, ui_store); });
        ui_mine.onValueChanged.AddListener(delegate { UpdateToggle(E_TaskType.Mining, ui_mine); });
    }

    private void UpdateToggle(E_TaskType type, Toggle toggle)
    {
        values[type] = toggle.isOn;
        update?.Invoke();

        var selected = values.Where(x => x.Value).Select(x => x.Key).ToList();

        if(selected.Count == values.Count)
        {
            ui_label.text = "All";
        }
        else if(selected.Count == 1)
        {
            ui_label.text = selected[0].ToString();
        }
        else if(selected.Count == 0)
        {
            ui_label.text = "None";
        }
        else
        {
            ui_label.text = "Mixed";
        }
    }
}