using System;
using TMPro;
using UnityEngine;

public abstract class UI_TooltipActor : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI ui_health;

    public abstract Type PanelType { get; }

    public virtual void SetContent(Actor actor, string content, string header = "")
    {
        ui_health.text = actor.GetMaxHP().ToString();

        SetText(content, header);
        SetPosition();
    }
}