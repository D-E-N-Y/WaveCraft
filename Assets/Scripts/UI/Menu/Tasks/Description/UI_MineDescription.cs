using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MineDescription : UI_TaskDescription 
{
    [SerializeField] private TextMeshProUGUI ui_resource;
    [SerializeField] private Button ui_focusToResource;

    [SerializeField] private TextMeshProUGUI ui_processor;
    [SerializeField] private Button ui_focusToProcessor;

    protected override void UpdateInfo()
    {
        base.UpdateInfo();

        MiningTask miningTask = (MiningTask)task;

        ui_resource.text = $"{miningTask.resourceAmount} {miningTask.resourceType}";
        
        ui_focusToResource.onClick.RemoveAllListeners();
        ui_focusToResource.onClick.AddListener(() => FocusTo(miningTask.resource));

        ui_processor.text = "none";
        ui_focusToProcessor.interactable = false;
        if(miningTask.processor != null)
        {
            ui_processor.text = miningTask.processor.nameActor;

            ui_focusToProcessor.onClick.RemoveAllListeners();
            ui_focusToProcessor.onClick.AddListener(() => FocusTo(miningTask.processor));
            ui_focusToProcessor.interactable = true;
        }
    }
}