using System;
using System.Collections.Generic;
using UnityEngine;

public class VillageSystem : GameSystem
{
    public static VillageSystem current;
    public Action UpdateMaxAmount;
    public Action UpdateCurrentAmount;

    private int maxAmount;
    private Dictionary<EVillageType, List<U_Player>> villages;
    private List<IResidential> residentials;

    public override void Initialize()
    {
        current = this;

        villages = new Dictionary<EVillageType, List<U_Player>>();
        villages[EVillageType.Worker] = new List<U_Player>();
        villages[EVillageType.Warrior] = new List<U_Player>();
        villages[EVillageType.Archer] = new List<U_Player>();
        villages[EVillageType.Mage] = new List<U_Player>();

        residentials = new List<IResidential>();
        maxAmount = 0;
    }

    public bool CheckFreeSpace() => maxAmount > villages.Count;
    public int GetCurrentAmount()
    {
        int count = 0;

        foreach (EVillageType type in Enum.GetValues(typeof(EVillageType)))
        {
            count += villages[type].Count;
        }

        return count;
    }
    public int GetMaxAmount() => maxAmount;

    public void AddVillage(U_Player village)
    {
        villages[village.Type()].Add(village);
        UpdateCurrentAmount?.Invoke();
    }

    public void RemoveVillage(EVillageType type, U_Player village) 
    {
        villages[type].Remove(village);
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

    public int GetCount(EVillageType type) => villages[type].Count;
    public List<U_Player> GetVillages(EVillageType type) => villages[type];
}
