using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_VillageContainer : MonoBehaviour
{
    [SerializeField] private UI_Village ui_villagePrefab;
    [SerializeField] private List<UI_Village> ui_villages;

    private Dictionary<EVillageProfession, ProfessionInfo> infoProfession;

    private UI_VillageMenu ui_villageMenu;
    private VillageSystem villageSystem;
    private UI_ProfessionVillageFilter ui_professionVillageFilter;

    public void Initialize(UI_VillageMenu ui_villageMenu, VillageSystem villageSystem, UI_ProfessionVillageFilter ui_professionVillageFilter, Dictionary<EVillageProfession, ProfessionInfo> infoProfession)
    {
        this.ui_villageMenu = ui_villageMenu;
        this.villageSystem = villageSystem;
        this.ui_professionVillageFilter = ui_professionVillageFilter;
        this.infoProfession = infoProfession;
    }

    private void OnEnable()
    {
        villageSystem.UpdateCurrentAmount += UpdateContainer;
        ui_professionVillageFilter.update += UpdateContainer;

        UpdateContainer();
    }

    private void OnDisable()
    {
        villageSystem.UpdateCurrentAmount -= UpdateContainer;
        ui_professionVillageFilter.update -= UpdateContainer;
    }

    private void UpdateContainer()
    {
        ui_villages = new List<UI_Village>();

        int dif = villageSystem.GetCurrentAmount() - transform.GetComponentsInChildren<UI_Village>(true).Length;
        if (dif > 0)
        {
            for (int i = 0; i < dif; i++)
            {
                Instantiate(ui_villagePrefab, transform);
            }
        }

        foreach (UI_Village ui_village in transform.GetComponentsInChildren<UI_Village>(true))
        {
            ui_villages.Add(ui_village);
            ui_village.Hide();
        }

        if (villageSystem.GetCurrentAmount() == 0) return;

        int villageIndex = 0;
        foreach (EVillageProfession profession in Enum.GetValues(typeof(EVillageProfession)))
        {
            if (ui_professionVillageFilter.values[profession])
            {
                List<U_Village> villages = villageSystem.GetVillages(profession);

                foreach (U_Village village in villages)
                {
                    InitializeVillage(ui_villages[villageIndex], village);
                    villageIndex++;
                }
            }
        }
    }
    
    private void InitializeVillage(UI_Village ui_village, U_Village village)
    {
        ui_village.Initialize(village, infoProfession[village.Profession()].sprite);
        ui_village.Show();
        
        Button _btn = ui_village.transform.GetComponentInChildren<Button>();
        _btn.onClick.RemoveAllListeners();
        _btn.onClick.AddListener(() => infoProfession[village.Profession()].ui_description.Initialize(village));
        _btn.onClick.AddListener(() => ui_villageMenu.OpenSection(infoProfession[village.Profession()].ui_description.gameObject));
        _btn.onClick.AddListener(() => ui_villageMenu.SelectTabSection(ui_village.transform.GetChild(0).gameObject));
        // _btn.onClick.AddListener(() => ui_freeWorkers.SetTask(ui_task.task));
    }
}
