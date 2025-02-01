using System;
using System.Collections.Generic;
using UnityEngine;

public class StorageSystem : GameSystem
{
    public static StorageSystem current;
    public Action<E_Resource> UpdateMaxCount;

    private Dictionary<E_Resource, List<IStorage>> storages;

    public override void Initialize()
    {
        base.Initialize();
        
        current = this;

        if(storages == null)
        {
            storages = new Dictionary<E_Resource, List<IStorage>>();
        }
    }

    public void AddStorage(IStorage storage, E_Resource resourceType)
    {
        if(!storages.ContainsKey(resourceType))
        {
            storages[resourceType] = new List<IStorage>();
        }
        
        storages[resourceType].Add(storage);
        UpdateMaxCount?.Invoke(resourceType);
    }

    public void RemoveStorages(IStorage storage, E_Resource resourceType)
    {
        storages[resourceType].Remove(storage);
        UpdateMaxCount?.Invoke(resourceType);
    }

    public bool CheckFreeSpace(E_Resource resourceType)
    {
        foreach(IStorage storage in storages[resourceType])
        {
            if(storage.isFreeSpace())
            {
                return true;
            }
        }

        return false;
    }

    public int CheckCountResurces(E_Resource resourceType)
    {
        int result = 0;
        
        foreach(IStorage storage in storages[resourceType])
        {
            result += storage.GetCurrentAmount();
        }

        return result;
    }

    public int CheckMaxCountResources(E_Resource resourceType)
    {
        int result = 0;
        
        foreach(IStorage storage in storages[resourceType])
        {
            result += storage.GetMaxAmount();
        }

        return result;
    }

    public IStorage FindStorageToAddResource(E_Resource resourceType)
    {
        foreach(IStorage storage in storages[resourceType])
        {
            if(storage.isFreeSpace())
            {
                return storage;
            }
        }

        return null;
    }
    

    public IStorage FindStorageToRemoveResource(E_Resource resourceType)
    {
        foreach(IStorage storage in storages[resourceType])
        {
            if(storage.GetCurrentAmount() > 0)
            {
                return storage;
            }
        }

        return null;
    }
}
