using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_TooltipResidential : UI_TooltipBuilding
{
    [SerializeField] private TextMeshProUGUI ui_villageAmount;

    public override Type PanelType => typeof(B_Residential);

    public override void SetContent(Actor actor, string content, string header = "")
    {
        base.SetContent(actor, content, header);

        B_Residential residential = (B_Residential)actor;

        ui_villageAmount.text = residential.GetVillageAmount().ToString(); 
    }
}
