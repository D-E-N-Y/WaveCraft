using System;
using TMPro;
using UnityEngine;

public class UI_TooltipStorage : UI_TooltipIndustrial
{
    [SerializeField] private TextMeshProUGUI ui_storageResource;

    public override Type PanelType => typeof(I_Storage);

    public override void SetContent(Actor actor, string content, string header = "")
    {
        base.SetContent(actor, content, header);

        I_Storage storage = (I_Storage)actor;

        ui_storageResource.text = storage.GetMaxAmount().ToString();
    }
}