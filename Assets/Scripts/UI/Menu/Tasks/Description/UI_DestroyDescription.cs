using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DestroyDescription : UI_TaskDescription 
{
    [SerializeField] private TextMeshProUGUI ui_building;
    [SerializeField] private Button ui_focusToBuilding;
    
    protected override void UpdateInfo()
    {
        base.UpdateInfo();

        DestroyTask destroyTask = (DestroyTask)task;

        ui_building.text = destroyTask.building.nameActor;
        
        ui_focusToBuilding.onClick.RemoveAllListeners();
        ui_focusToBuilding.onClick.AddListener(() => FocusTo(destroyTask.building));
    }
}