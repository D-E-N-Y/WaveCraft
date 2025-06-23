using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ProfessionVillageFilter : UIPanel 
{
    public Action update;

    [SerializeField] private TextMeshProUGUI ui_label;

    [SerializeField] private Toggle ui_worker;
    [SerializeField] private Toggle ui_warrior;
    [SerializeField] private Toggle ui_archer;
    [SerializeField] private Toggle ui_mage;

    public Dictionary<EVillageProfession, bool> values { get; private set; }

    public void Initialize()
    {
        values = new Dictionary<EVillageProfession, bool>();
        values[EVillageProfession.Worker] = ui_worker.isOn;
        values[EVillageProfession.Warrior] = ui_warrior.isOn;
        values[EVillageProfession.Archer] = ui_archer.isOn;
        values[EVillageProfession.Mage] = ui_mage.isOn;

        ui_worker.onValueChanged.AddListener(delegate { UpdateToggle(EVillageProfession.Worker, ui_worker); });
        ui_warrior.onValueChanged.AddListener(delegate { UpdateToggle(EVillageProfession.Warrior, ui_warrior); });
        ui_archer.onValueChanged.AddListener(delegate { UpdateToggle(EVillageProfession.Archer, ui_archer); });
        ui_mage.onValueChanged.AddListener(delegate { UpdateToggle(EVillageProfession.Mage, ui_mage); });
    }

    private void UpdateToggle(EVillageProfession profession, Toggle toggle)
    {
        values[profession] = toggle.isOn;
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