using TMPro;
using UnityEngine;

public abstract class UI_TooltipIndustrial : UI_TooltipBuilding
{
    [SerializeField] private TextMeshProUGUI ui_resource;

    public override void SetContent(Actor actor, string content, string header = "")
    {
        base.SetContent(actor, content, header);

        B_Industrial industrial = (B_Industrial)actor;

        ui_resource.text = industrial.GetTypeResource().ToString();
    }
}