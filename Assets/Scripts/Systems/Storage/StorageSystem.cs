using System.Collections.Generic;
using UnityEngine;

public class StorageSystem : MonoBehaviour
{
    public static StorageSystem current;

    private Dictionary<E_Resource, List<IStorage>> storages;

    private void Awake() 
    {
        current = this;
    }

    private void Start() 
    {
        Initialize();
    }

    public void Initialize()
    {
        if(storages == null)
        {
            storages = new Dictionary<E_Resource, List<IStorage>>();
        }
    }

    public void AddStorage(IStorage storage, E_Resource resourceType)
    {
        Initialize(); // template decision

        if(!storages.ContainsKey(resourceType))
        {
            storages[resourceType] = new List<IStorage>();
        }
        
        storages[resourceType].Add(storage);
    }

    public void RemoveStorages(IStorage storage, E_Resource resourceType)
    {
        storages[resourceType].Remove(storage);
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
