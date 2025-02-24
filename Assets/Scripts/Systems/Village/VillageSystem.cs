using System;
using System.Collections.Generic;
using UnityEngine;

public class VillageSystem : GameSystem
{
    public static VillageSystem current;
    public Action UpdateMaxAmount;
    public Action UpdateCurrentAmount;

    private int maxAmount;
    private List<U_Player> villages;
    private List<IResidential> residentials;

    public override void Initialize()
    {
        base.Initialize();

        current = this;

        villages = new List<U_Player>();
        residentials = new List<IResidential>();
        maxAmount = 0;
    }

    public bool CheckFreeSpace() => maxAmount > villages.Count;
    public int GetCurrentAmount() => villages.Count;
    public int GetMaxAmount() => maxAmount;

    public void AddVillage(U_Player village) 
    {
        villages.Add(village);
        UpdateCurrentAmount?.Invoke();
    }

    public void RemoveVillage(U_Player village) 
    {
        villages.Remove(village);
        UpdateCurrentAmount?.Invoke();
    }
    
    public void AddResidential(IResidential residential)
    {
        residentials.Add(residential);
        maxAmount += residential.GetVillageAmount();
        UpdateMaxAmount?.Invoke();
    }

    public void RemoveResidential(IResidential residential)
    {
        residentials.Add(residential);
        maxAmount -= residential.GetVillageAmount();
        UpdateMaxAmount?.Invoke();
    }
}
