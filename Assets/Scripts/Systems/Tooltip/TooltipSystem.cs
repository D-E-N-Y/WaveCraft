using System;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : GameSystem
{
    public static TooltipSystem current;

    [SerializeField] private UI_Tooltip tooltip;
    [SerializeField] private List<UI_TooltipActor> ui_tooltipActors;
    private Dictionary<Type, UI_TooltipActor> tooltipActors;

    private UI_Tooltip currentTooltip;

    public override void Initialize()
    {
        current = this;

        tooltip.Initialize();
        ui_tooltipActors.ForEach(x => x.Initialize());

        tooltipActors = new Dictionary<Type, UI_TooltipActor>();
        ui_tooltipActors.ForEach(x => tooltipActors.Add(x.PanelType, x));
    }

    public void Show(string content, string header = "")
    {
        tooltip.SetContent(content, header);
        tooltip.Show();

        currentTooltip = tooltip;
    }

    public void Show(Actor actor, string content, string header = "")
    {
        Type type = actor.GetType();

        foreach (var kvp in tooltipActors)
        {
            var panelType = kvp.Key;
            if (panelType.IsAssignableFrom(type))
            {
                type = panelType;

                tooltipActors[type].SetContent(actor, content, header);
                tooltipActors[type].Show();

                currentTooltip = tooltipActors[type];

                return;
            }
        }

        if (!tooltipActors.ContainsKey(type))
        {
            Debug.Log($"not found tooltip for {type} type");
        }
    }

    public void Hide()
    {
        if (currentTooltip == null) return;

        currentTooltip.Hide();
        currentTooltip = null;
    }
}