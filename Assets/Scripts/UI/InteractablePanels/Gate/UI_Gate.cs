using System;
using TMPro;
using UnityEngine;

public class UI_Gate : UI_InteractablePanel
{
    public override Type PanelType => typeof(D_Gate);
    private D_Gate _gate;
    
    [SerializeField] private TextMeshProUGUI ui_sateGate;

    public override void Initialize(Actor _actor)
    {
        base.Initialize(_actor);

        _gate = (D_Gate)_actor;
        
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