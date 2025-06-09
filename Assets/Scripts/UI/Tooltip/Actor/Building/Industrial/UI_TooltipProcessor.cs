using System;
using TMPro;
using UnityEngine;

public class UI_TooltipProcessor : UI_TooltipIndustrial 
{
    [SerializeField] private TextMeshProUGUI ui_factor;
    [SerializeField] private TextMeshProUGUI ui_timeProcess;

    public override Type PanelType => typeof(I_Processor);

    public override void SetContent(Actor actor, string content, string header = "")
    {
        base.SetContent(actor, content, header);

        I_Processor processor = (I_Processor)actor;

        ui_factor.text = processor.GetFactor().ToString();
        ui_timeProcess.text = processor.GetTimeProcess().ToString();
    }
}