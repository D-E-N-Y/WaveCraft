using UnityEngine;

public class TooltipSystem : GameSystem
{
    public static TooltipSystem current;

    public UI_Tooltip tooltip;

    public override void Initialize()
    {
        current = this;
    }

    public void Show(string content, string header = "")
    {
        tooltip.SetText(content, header);
        tooltip.gameObject.SetActive(true);
    }

    public void Hide()
    {
        tooltip.gameObject.SetActive(false);
    }
}