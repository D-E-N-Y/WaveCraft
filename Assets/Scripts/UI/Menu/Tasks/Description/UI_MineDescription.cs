using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MineDescription : UI_TaskDescription 
{
    [SerializeField] private TextMeshProUGUI ui_resource;
    [SerializeField] private Button ui_focusToResource;

    protected override void UpdateInfo()
    {
        base.UpdateInfo();

        MiningTask miningTask = (MiningTask)task;

        ui_resource.text = $"{miningTask.resourceAmount} {miningTask.resourceType}";
        ui_focusToResource.onClick.AddListener(() => FocusTo(miningTask.resource));
    }
}