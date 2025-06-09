using UnityEngine;

public class B_Residential : Building, IResidential
{
    [SerializeField] private int villageAmount;

    public override string nameActor => "Residential";

    public override void Built()
    {
        base.Built();

        VillageSystem.current.AddResidential(this);
    }

    public int GetVillageAmount() => villageAmount;
}
