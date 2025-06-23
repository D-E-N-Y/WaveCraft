using UnityEngine;

public class UI_VillageMenu : UIMenu
{
    [SerializeField] private UI_ProfessionVillageFilter ui_professionVillageFilter;
    [SerializeField] private UI_VillageContainer ui_villageContainer;

    private VillageSystem villageSystem;

    public void Initialize()
    {
        villageSystem = VillageSystem.current;

        ui_professionVillageFilter.Initialize();
        ui_villageContainer.Initialize(this, villageSystem, ui_professionVillageFilter);
    }
}
