using TMPro;
using UnityEngine;

public class UIInteractableGatePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ui_name;
    [SerializeField] private TextMeshProUGUI ui_hp;
    [SerializeField] private TextMeshProUGUI ui_sateGate;
    private D_Gate _gate;

    public void Initialize(D_Gate gate)
    {
        _gate = gate;

        ui_name.name = _gate.nameActor;
        ui_hp.text = _gate.GetCurrentHP().ToString();
        ui_sateGate.text = _gate.state.ToString();
    }

    public void ChangeStateGate()
    {
        if(!_gate.isBuild) return;
        
        if(_gate.state == EGateState.open)
        {
            _gate.Close();
        }
        else
        {  
            _gate.Open();
        }

        ui_sateGate.text = _gate.state.ToString();
    }
}