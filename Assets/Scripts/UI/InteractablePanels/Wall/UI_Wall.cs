using System;
using TMPro;
using UnityEngine;

public class UI_Wall : UI_InteractablePanel
{
    public override Type PanelType => typeof(D_Wall);
    private D_Wall wall;

    public override void Initialize(Actor _actor)
    {
        base.Initialize(_actor);

        wall = (D_Wall)_actor;
    }
}
