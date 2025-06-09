using TMPro;
using UnityEngine;

public abstract class UI_TooltipBuilding : UI_TooltipActor
{
    [SerializeField] private TextMeshProUGUI ui_timeToBuild;

    public override void SetContent(Actor actor, string content, string header = "")
    {
        base.SetContent(actor, content, header);

        Building building = (Building)actor;

        ui_timeToBuild.text = building.GetTimeToBuild().ToString();
    }
}