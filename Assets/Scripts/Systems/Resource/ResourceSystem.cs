using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSystem : GameSystem
{
    public static ResourceSystem current;
    public Action<E_Resource> UpdateCurrentCount;

    public Dictionary<E_Resource, int> resources { private set; get; }

    public override void Initialize()
    { 
        current = this;

        resources = new Dictionary<E_Resource, int>();
    }

    public void AddResources(S_Cost[] costs)
    {
        foreach (S_Cost cost in costs)
        {
            AddResourceByType(cost.resourse, cost.count);
        }
    }

    public void AddResourceByType(E_Resource resourceType, int amount)
    {
        if (StorageSystem.current.CheckFreeSpace(resourceType))
        {
            int residue = StorageSystem.current.FindStorageToAddResource(resourceType).AddResources(amount);
            UpdateCurrentCount?.Invoke(resourceType);

            if (residue != -1)
            {
                if (!resources.ContainsKey(resourceType))
                {
                    resources[resourceType] = 0;
                }

                if (residue > 0)
                {
                    resources[resourceType] += residue;

                    AddResourceByType(resourceType, residue);
                }
                else
                {
                    resources[resourceType] += amount;
                }
            }
            else
            {
                Debug.Log($"ResourceSystem - AddResources - Null");
            }
        }
    }

    public int AddResourceByType(IStorage storage, E_Resource resource, int amount)
    {
        int residue = storage.AddResources(amount);
        resources[resource] += amount - residue;
        UpdateCurrentCount?.Invoke(resource);
        
        return residue;
    }

    public void RemoveResourceByType(E_Resource resourceType, int amount)
    {
        if(StorageSystem.current.CheckCountResurces(resourceType) >= amount)
        {
            int residue = StorageSystem.current.FindStorageToRemoveResource(resourceType).RemoveResources(amount);
            UpdateCurrentCount?.Invoke(resourceType);

            if(residue > 0)
            {
                resources[resourceType] -= residue;
                RemoveResourceByType(resourceType, residue);
            }
            else if(residue == 0)
            {
                resources[resourceType] -= amount;
            }
        }
    }

    public void RemoveResources(S_Cost[] costs)
    {
        foreach (S_Cost cost in costs)
        {
            RemoveResourceByType(cost.resourse, cost.count);
        }
    }
}