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
        base.Initialize();
        
        current = this;

        resources = new Dictionary<E_Resource, int>();
    }

    public void AddResources(E_Resource resourceType, int amount)
    {
        if(StorageSystem.current.CheckFreeSpace(resourceType))
        {
            int residue = StorageSystem.current.FindStorageToAddResource(resourceType).AddResources(amount);
            UpdateCurrentCount?.Invoke(resourceType);

            if(residue != null)
            {
                if(!resources.ContainsKey(resourceType))
                {
                    resources[resourceType] = 0;
                }
                
                if(residue > 0)
                {
                    resources[resourceType] += residue;

                    AddResources(resourceType, residue);
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

    public void RemoveResources(E_Resource resourceType, int amount)
    {
        if(StorageSystem.current.CheckCountResurces(resourceType) >= amount)
        {
            int residue = StorageSystem.current.FindStorageToRemoveResource(resourceType).RemoveResources(amount);
            UpdateCurrentCount?.Invoke(resourceType);

            if(residue != null)
            {
                if(residue > 0)
                {
                    resources[resourceType] -= residue;

                    AddResources(resourceType, residue);
                }
                else
                {
                    resources[resourceType] -= amount;
                }
            }
            else
            {
                Debug.Log($"ResourceSystem - RemoveResources - Null");
            }
        }
    }
}