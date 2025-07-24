using System;
using TMPro;
using UnityEngine;

public class UI_Pillar : UI_Building
{
    public override Type PanelType => typeof(D_Pillar);
    private D_Pillar pillar;

    public override void InitializeInfo(Actor _actor)
    {
        base.InitializeInfo(_actor);

        pillar = (D_Pillar)_actor;
    }

    public void ContinueWall()
    {
        if(pillar.isBuild)
        {
            BuildSystem.current.ContinueWall(pillar);
        }
    }
}
