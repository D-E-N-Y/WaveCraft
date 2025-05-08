using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StoreDescription : UI_TaskDescription 
{
    [SerializeField] private TextMeshProUGUI ui_source;
    [SerializeField] private Button ui_focusToSource;

    [SerializeField] private TextMeshProUGUI ui_storage;
    [SerializeField] private Button ui_focusToStorage;

    protected override void UpdateInfo()
    {
        base.UpdateInfo();

        StoreTask storeTask = (StoreTask)task;

        ui_source.text = storeTask.source.nameActor;

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