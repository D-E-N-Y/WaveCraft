using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatusFilter : UIPanel 
{
    public Action update;

    [SerializeField] private TextMeshProUGUI ui_label;

    [SerializeField] private Toggle ui_execured;
    [SerializeField] private Toggle ui_pending;
    [SerializeField] private Toggle ui_canceled;
    [SerializeField] private Toggle ui_completed;

    public Dictionary<E_TaskState, bool> values { get; private set; }

    public void Initialize()
    {
        values = new Dictionary<E_TaskState, bool>();
        values[E_TaskState.Execured] = ui_execured.isOn;
        values[E_TaskState.Pending] = ui_pending.isOn;
        values[E_TaskState.Canceled] = ui_canceled.isOn;
        values[E_TaskState.Completed] = ui_completed.isOn;

        ui_execured.onValueChanged.AddListener(delegate { UpdateToggle(E_TaskState.Execured, ui_execured); });
        ui_pending.onValueChanged.AddListener(delegate { UpdateToggle(E_TaskState.Pending, ui_pending); });
        ui_canceled.onValueChanged.AddListener(delegate { UpdateToggle(E_TaskState.Canceled, ui_canceled); });
        ui_completed.onValueChanged.AddListener(delegate { UpdateToggle(E_TaskState.Completed, ui_completed); });
    }

    private void UpdateToggle(E_TaskState type, Toggle toggle)
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