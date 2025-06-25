using System;
using System.Collections.Generic;
using UnityEngine;

public class VillageSystem : GameSystem
{
    public static VillageSystem current;
    public Action UpdateMaxAmount;
    public Action UpdateCurrentAmount;

    private int maxAmount;
    private Dictionary<EVillageProfession, List<U_Village>> villages;
    private List<IResidential> residentials;

    public override void Initialize()
    {
        current = this;

        villages = new Dictionary<EVillageProfession, List<U_Village>>();
        villages[EVillageProfession.Worker] = new List<U_Village>();
        villages[EVillageProfession.Warrior] = new List<U_Village>();
        villages[EVillageProfession.Archer] = new List<U_Village>();
        villages[EVillageProfession.Mage] = new List<U_Village>();

        residentials = new List<IResidential>();
        maxAmount = 0;
    }

    public bool CheckFreeSpace() => maxAmount > villages.Count;
    public int GetCurrentAmount()
    {
        int count = 0;

        foreach (EVillageProfession profession in Enum.GetValues(typeof(EVillageProfession)))
        {
            count += villages[profession].Count;
        }

        return count;
    }
    public int GetMaxAmount() => maxAmount;

    public void AddVillage(U_Village village)
    {
        villages[village.Profession()].Add(village);
        UpdateCurrentAmount?.Invoke();

        MessageSystem.current.AddMessage($"A new {village.Profession()} named {village.nameActor} has appeared");
    }

    public void RemoveVillage(U_Village village) 
    {
        villages[village.Profession()].Remove(village);
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

    public int GetCount(EVillageProfession profession) => villages[profession].Count;
    public List<U_Village> GetVillages(EVillageProfession profession) => villages[profession];
}
