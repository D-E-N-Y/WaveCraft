using UnityEngine;

public class B_Residential : Building, IResidential
{
    [SerializeField] private int villageAmount;

    public int GetVillageAmount()
    {
        return villageAmount;
    }
}
