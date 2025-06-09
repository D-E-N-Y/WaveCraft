using System;
using TMPro;
using UnityEngine;

public class UI_TooltipProduction : UI_TooltipIndustrial 
{
    [SerializeField] private TextMeshProUGUI ui_storageResource;
    [SerializeField] private TextMeshProUGUI ui_timeProduce;

    public override Type PanelType => typeof(I_Production);

    public override void SetContent(Actor actor, string content, string header = "")
    {
        base.SetContent(actor, content, header);

        I_Production production = (I_Production)actor;

        ui_storageResource.text = production.GetMaxAmount().ToString();
        ui_timeProduce.text = production.GetTimeProduce().ToString();
    }
}