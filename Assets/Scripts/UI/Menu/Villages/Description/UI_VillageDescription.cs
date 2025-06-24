using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_VillageDescription : UIPanel
{
    [SerializeField] private TextMeshProUGUI ui_name;
    [SerializeField] private TextMeshProUGUI ui_profession;

    [SerializeField] private TextMeshProUGUI ui_hp;
    [SerializeField] private TextMeshProUGUI ui_damage;
    [SerializeField] private TextMeshProUGUI ui_armor;
    [SerializeField] private TextMeshProUGUI ui_speed;

    [SerializeField] private Button ui_focusVillageAtMapButton;
    [SerializeField] private Button ui_dissolveButton;

    private U_Village village;

    protected UI_VillageMenu ui_villageMenu;
    protected VillageSystem villageSystem;
    protected FocusSystem focusSystem;
    protected UISystem uiSystem;

    public virtual void Initialize(UI_VillageMenu ui_villageMenu, VillageSystem villageSystem)
    {
        this.ui_villageMenu = ui_villageMenu;
        this.villageSystem = villageSystem;

        focusSystem = FocusSystem.current;
        uiSystem = UISystem.current;
    }

    public virtual void InitializeVillage(U_Village village)
    {
        this.village = village;

        focusSystem = FocusSystem.current;
        villageSystem = VillageSystem.current;

        UpdateData();
    }

    private void UpdateData()
    {
        ui_name.text = village.nameActor;
        ui_profession.text = village.Profession().ToString();

        ui_hp.text = village.GetMaxHP().ToString();
        ui_damage.text = village.GetDamage().ToString();
        ui_armor.text = village.GetArmor().ToString();
        ui_speed.text = village.GetSpeed().ToString();

        ui_focusVillageAtMapButton.onClick.RemoveAllListeners();
        ui_focusVillageAtMapButton.onClick.AddListener(() => FocusTo(village));

        ui_dissolveButton.onClick.RemoveAllListeners();
        ui_dissolveButton.onClick.AddListener(() => Dissolve());
    }

    protected void FocusTo(Actor actor)
    {
        focusSystem.FocusToObject(actor);
        uiSystem.CloseAllPanels();
    }

    private void Dissolve()
    {
        villageSystem.RemoveVillage(village);
        ui_villageMenu.CloseCurrentSection();
        village.Dissolve();
    }
}
