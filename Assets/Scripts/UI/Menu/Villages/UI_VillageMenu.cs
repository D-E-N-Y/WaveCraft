using System.Collections.Generic;
using UnityEngine;

public class UI_VillageMenu : UIMenu
{
    [SerializeField] private UI_ProfessionVillageFilter ui_professionVillageFilter;
    [SerializeField] private UI_VillageContainer ui_villageContainer;

    [System.Serializable]
    private struct UIProfessionInfo
    {
        public ProfessionInfo professionInfo;
        public EVillageProfession profession;
    }

    [SerializeField] private List<UIProfessionInfo> ui_info;
    private Dictionary<EVillageProfession, ProfessionInfo> infoProfession;

    private VillageSystem villageSystem;

    public void Initialize()
    {
        villageSystem = VillageSystem.current;

        infoProfession = new Dictionary<EVillageProfession, ProfessionInfo>();
        foreach (UIProfessionInfo current in ui_info)
        {
            infoProfession.Add(current.profession, current.professionInfo);
            current.professionInfo.ui_description?.Initialize(this, villageSystem);
            current.professionInfo.ui_description?.Hide();
        }

        ui_professionVillageFilter.Initialize();
        ui_villageContainer.Initialize(this, villageSystem, ui_professionVillageFilter, infoProfession);
    }
}
