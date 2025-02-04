using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceSystem : GameSystem
{
    public static ResourceSystem current;
    public Action<E_Resource> UpdateCurrentCount;

    public Dictionary<E_Resource, int> resources { private set; get; }
    private Dictionary<E_Resource, List<Resource>> naturalResources;

    public override void Initialize()
    {
        base.Initialize();
        
        current = this;

        resources = new Dictionary<E_Resource, int>();
        naturalResources = new Dictionary<E_Resource, List<Resource>>();
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

    // public int AddResources(IStorage storage, int amount)
    // {
    //     return 
    // }

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

    public void AddNaturalResources(Resource resource)
    {
        if(!naturalResources.ContainsKey(resource.GetType()))
        {
            naturalResources[resource.GetType()] = new List<Resource>();
        }

        naturalResources[resource.GetType()].Add(resource);
    }

    public void RemoveNaturalResources(Resource resource)
    {
        naturalResources[resource.GetType()].Remove(resource);
    }

    // public Resource GetNearbyNaturalResource(E_Resource type, Transform target)
    // {
    //     foreach(Resource resource in naturalResources[type])
    //     {

    //     }
    // }
}