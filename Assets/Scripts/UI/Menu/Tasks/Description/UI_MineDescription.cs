using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MineDescription : UI_TaskDescription
{
    [SerializeField] private TextMeshProUGUI ui_resource;
    [SerializeField] private TextMeshProUGUI ui_resource1;
    [SerializeField] private Button ui_focusToResource;

    [System.Serializable]
    private struct SUIExecutingState
    {
        public GameObject ui_panel;
        public EMiningExecutingState state;
    }
    [SerializeField] private List<SUIExecutingState> ui_executingState;

    [SerializeField] private TextMeshProUGUI ui_processor;
    [SerializeField] private Button ui_focusToProcessor;

    protected override void UpdateInfo()
    {
        base.UpdateInfo();

        MiningTask miningTask = (MiningTask)task;

        ui_resource.text = $"{miningTask.goal} {miningTask.resourceType}";
        ui_resource1.text = $"{miningTask.resourceType}";

        ui_executingState.ForEach(x => x.ui_panel.SetActive(false));
        if (miningTask.executingState == EMiningExecutingState.none)
        {
            GameObject ui_status = ui_executingState
            .Where(x => x.state == EMiningExecutingState.none)
                .FirstOrDefault()
                .ui_panel;

            SetStatus(ui_status);
        }
        else
        {
            ui_executingState
                .Where(x => x.state == miningTask.executingState)
                .FirstOrDefault()
                .ui_panel
                .SetActive(true);
        }

        ui_focusToResource.onClick.RemoveAllListeners();
        ui_focusToResource.onClick.AddListener(() => FocusTo(miningTask.resource));

        ui_processor.text = "none";
        ui_focusToProcessor.interactable = false;
        if (miningTask.processor != null)
        {
            ui_processor.text = miningTask.processor.nameActor;

            ui_focusToProcessor.onClick.RemoveAllListeners();
            ui_focusToProcessor.onClick.AddListener(() => FocusTo(miningTask.processor));
            ui_focusToProcessor.interactable = true;
        }
    }
    
    public override E_TaskType TaskType() => E_TaskType.Mining;
}