using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuildDescription : UI_TaskDescription
{
    [SerializeField] private List<TextMeshProUGUI> ui_building;
    [SerializeField] private Button ui_focusToBuilding;

    [System.Serializable]
    private struct SUIExecutingState
    {
        public GameObject ui_panel;
        public EBuildExecutingState state;
    }
    [SerializeField] private List<SUIExecutingState> ui_executingState;

    protected override void UpdateInfo()
    {
        base.UpdateInfo();

        BuildTask buildTask = (BuildTask)task;

        ui_building.ForEach(x => x.text = buildTask.building.nameActor);

        ui_executingState.ForEach(x => x.ui_panel.SetActive(false));
        if (buildTask.executingState == EBuildExecutingState.none)
        {
            GameObject ui_status = ui_executingState
                .Where(x => x.state == EBuildExecutingState.none)
                .FirstOrDefault()
                .ui_panel;

            SetStatus(ui_status);
        }
        else
        {
            ui_executingState
                .Where(x => x.state == buildTask.executingState)
                .FirstOrDefault()
                .ui_panel
                .SetActive(true);
        }

        ui_progress.text += "%";
        ui_goal.text += "%";

        ui_focusToBuilding.onClick.RemoveAllListeners();
        ui_focusToBuilding.onClick.AddListener(() => FocusTo(buildTask.building));
    }
    
    public override E_TaskType TaskType() => E_TaskType.Build;
}