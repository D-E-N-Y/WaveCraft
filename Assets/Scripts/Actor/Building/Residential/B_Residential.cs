using UnityEngine;

public class B_Residential : Building, IResidential
{
    [SerializeField] private int villageAmount;

    public override void Built()
    {
        base.Built();

        VillageSystem.current.AddResidential(this);
    }

    public override void Initialize()
    {
        base.Initialize();

        nameActor = "Residential";
    }

    public int GetVillageAmount()
    {
        return villageAmount;
    }
}
