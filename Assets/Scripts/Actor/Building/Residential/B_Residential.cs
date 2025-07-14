using UnityEngine;

public class B_Residential : Building, IResidential
{
    [SerializeField] private EResidentialHouseSize houseSize;
    [SerializeField] private int villageAmount;

    public override string nameActor => $"{houseSize} House";

    public override void Built()
    {
        base.Built();

        VillageSystem.current.AddResidential(this);
    }

    public EResidentialHouseSize GetHouseSize() => houseSize;
    public int GetVillageAmount() => villageAmount;
}
