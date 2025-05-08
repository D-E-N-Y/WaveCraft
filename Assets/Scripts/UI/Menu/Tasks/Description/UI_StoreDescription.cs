using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StoreDescription : UI_TaskDescription 
{
    [SerializeField] private TextMeshProUGUI ui_amount;
    [SerializeField] private TextMeshProUGUI ui_resource;

    [SerializeField] private TextMeshProUGUI ui_source;
    [SerializeField] private Button ui_focusToSource;

    [SerializeField] private TextMeshProUGUI ui_storage;
    [SerializeField] private Button ui_focusToStorage;

    [System.Serializable]
    private struct SUIExecutingState
    {
        public GameObject ui_panel;
        public EStoreExecutingState state;
    }
    [SerializeField] private List<SUIExecutingState> ui_executingState;

    protected override void UpdateInfo()
    {
        base.UpdateInfo();

        StoreTask storeTask = (StoreTask)task;

        ui_source.text = storeTask.source.nameActor;

        if(storeTask.amount > 0)
        {
            ui_amount.text = storeTask.amount.ToString();
            ui_amount.gameObject.SetActive(true);
        }

        ui_resource.text = storeTask.resource.ToString();

        ui_executingState.ForEach(x => x.ui_panel.SetActive(false));
        if(storeTask.executingState == EStoreExecutingState.none)
        {
            GameObject ui_status = ui_executingState
            .Where(x => x.state == EStoreExecutingState.none)
                .FirstOrDefault()
                .ui_panel;

            SetStatus(ui_status);
        }
        else
        {
            ui_executingState
                .Where(x => x.state == storeTask.executingState)
                .FirstOrDefault()
                .ui_panel
                .SetActive(true);
        }

        ui_progress.text += "%";
        ui_goal.text += "%";

        ui_focusToSource.onClick.RemoveAllListeners();
        ui_focusToSource.onClick.AddListener(() => FocusTo(storeTask.source));

        ui_storage.text = "none";
        ui_focusToStorage.interactable = false;
        if(storeTask.storage)
        {
            ui_storage.text = storeTask.storage.nameActor;

            ui_focusToStorage.onClick.RemoveAllListeners();
            ui_focusToStorage.onClick.AddListener(() => FocusTo(storeTask.storage));
            ui_focusToStorage.interactable = true;
        }
    }
}