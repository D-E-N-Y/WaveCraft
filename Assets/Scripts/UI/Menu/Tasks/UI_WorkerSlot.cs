using TMPro;
using UnityEngine;

public class UI_WorkerSlot : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI ui_name;
    [SerializeField] private TextMeshProUGUI ui_freeSlots;

    public UP_Worker worker { get; private set; }

    public void Initialize(UP_Worker worker)
    {
        this.worker = worker;

        ui_name.text = worker.nameActor;
        ui_freeSlots.text = worker.GetFreeSlots().ToString();
    }
}