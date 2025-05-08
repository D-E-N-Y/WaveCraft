using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DestroyDescription : UI_TaskDescription 
{
    [SerializeField] private TextMeshProUGUI ui_building;
    [SerializeField] private Button ui_focusToBuilding;
    
    [System.Serializable]
    private struct SUIExecutingState
    {
        public GameObject ui_panel;
        public EDestroyExecutingState state;
    }
    [SerializeField] private List<SUIExecutingState> ui_executingState;

    protected override void UpdateInfo()
    {
        base.UpdateInfo();

        DestroyTask destroyTask = (DestroyTask)task;

        ui_building.text = destroyTask.building.nameActor;
        
        ui_executingState.ForEach(x => x.ui_panel.SetActive(false));
        if(destroyTask.executingState == EDestroyExecutingState.none)
        {
            GameObject ui_status = ui_executingState
            .Where(x => x.state == EDestroyExecutingState.none)
                .FirstOrDefault()
                .ui_panel;

            SetStatus(ui_status);
        }
        else
        {
            ui_executingState
                .Where(x => x.state == destroyTask.executingState)
                .FirstOrDefault()
                .ui_panel
                .SetActive(true);
        }

        ui_progress.text += "%";
        ui_goal.text += "%";

        ui_focusToBuilding.onClick.RemoveAllListeners();
        ui_focusToBuilding.onClick.AddListener(() => FocusTo(destroyTask.building));
    }
}