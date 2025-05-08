using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuildDescription : UI_TaskDescription 
{
    [SerializeField] private TextMeshProUGUI ui_building;
    [SerializeField] private Button ui_focusToBuilding;
    
    protected override void UpdateInfo()
    {
        base.UpdateInfo();

        BuildTask buildTask = (BuildTask)task;

        ui_building.text = buildTask.building.nameActor;
        
        ui_focusToBuilding.onClick.RemoveAllListeners();
        ui_focusToBuilding.onClick.AddListener(() => FocusTo(buildTask.building));
    }
}