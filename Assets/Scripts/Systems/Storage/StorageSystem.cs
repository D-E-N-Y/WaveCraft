using System.Collections.Generic;
using UnityEngine;

public class StorageSystem : MonoBehaviour
{
    public static StorageSystem current;

    private List<IStorage> woodStorage;
    private List<IStorage> stoneStorage;
    private List<IStorage> foodStorage;

    private void Awake() 
    {
        current = this;
    }

    public void AddStorage(IStorage storage, E_Resource resourceType)
    {
        switch(resourceType)
        {
            case E_Resource.Wood:
                woodStorage.Add(storage);
                break;

            case E_Resource.Stone:
                stoneStorage.Add(storage);
                break;

            case E_Resource.Food:
                foodStorage.Add(storage);
                break;
        }
    }

    public void RemoveStorages(IStorage storage, E_Resource resourceType)
    {
        switch(resourceType)
        {
            case E_Resource.Wood:
                woodStorage.Remove(storage);
                break;

            case E_Resource.Stone:
                stoneStorage.Remove(storage);
                break;

            case E_Resource.Food:
                foodStorage.Remove(storage);
                break;
        }
    }

    public bool CheckFreeSpace(E_Resource resourceType)
    {
        switch(resourceType)
        {
            case E_Resource.Wood:
                return ResultCheckFreeSpace(woodStorage);

            case E_Resource.Stone:
                return ResultCheckFreeSpace(stoneStorage);

            case E_Resource.Food:
                return ResultCheckFreeSpace(foodStorage);
        }

        return false;
    }

    private bool ResultCheckFreeSpace(List<IStorage> storages)
    {
        foreach(IStorage storage in storages)
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
        switch(resourceType)
        {
            case E_Resource.Wood:
                return CountResources(woodStorage);

            case E_Resource.Stone:
                return CountResources(stoneStorage);

            case E_Resource.Food:
                return CountResources(foodStorage);
        }

        return 0;
    }

    private int CountResources(List<IStorage> storages)
    {
        int result = 0;
        
        foreach(IStorage storage in storages)
        {
            result += storage.GetCurrentAmount();
        }

        return result;
    }

    public IStorage FindStorageToAddResource(E_Resource resourceType)
    {
        switch(resourceType)
        {
            case E_Resource.Wood:
                return GetStorageToAddResource(woodStorage);

            case E_Resource.Stone:
                return GetStorageToAddResource(stoneStorage);

            case E_Resource.Food:
                return GetStorageToAddResource(foodStorage);
        }

        return null;
    }

    private IStorage GetStorageToAddResource(List<IStorage> storages)
    {
        foreach(IStorage storage in storages)
        {
            if(storage.isFreeSpace())
                return storage;
        }

        return null;
    }
    

    public IStorage FindStorageToRemoveResource(E_Resource resourceType)
    {
        switch(resourceType)
        {
            case E_Resource.Wood:
                return GetStorageToRemoveResource(woodStorage);

            case E_Resource.Stone:
                return GetStorageToRemoveResource(stoneStorage);

            case E_Resource.Food:
                return GetStorageToRemoveResource(foodStorage);
        }

        return null;
    }

    private IStorage GetStorageToRemoveResource(List<IStorage> storages)
    {
        foreach(IStorage storage in storages)
        {
            if(storage.GetCurrentAmount() > 0)
                return storage;
        }

        return null;
    }
}
