using System;
using TMPro;
using UnityEngine;

public class UI_Wall : UI_Building
{
    public override Type PanelType => typeof(D_Wall);
    private D_Wall wall;

    public override void InitializeInfo(Actor _actor)
    {
        base.InitializeInfo(_actor);

        wall = (D_Wall)_actor;
    }
}
