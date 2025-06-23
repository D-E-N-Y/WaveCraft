using UnityEngine;

public class UI_VillageDescription : UIPanel
{
    private U_Village village;

    public virtual void Initialize(U_Village village)
    {
        this.village = village;
    }
}
