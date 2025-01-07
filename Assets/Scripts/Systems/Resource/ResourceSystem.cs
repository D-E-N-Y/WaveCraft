using System.Collections.Generic;
using UnityEngine;

public class ResourceSystem : MonoBehaviour
{
    public static ResourceSystem current;

    public Dictionary<E_Resource, int> resources { private set; get; }

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
        resources = new Dictionary<E_Resource, int>();
        resources[E_Resource.Wood] = 100;
        resources[E_Resource.Stone] = 100;
        resources[E_Resource.Food] = 100;
    }

    public void AddResources(E_Resource resourceType, int amount)
    {
        if(StorageSystem.current.CheckFreeSpace(resourceType))
        {
            int residue = StorageSystem.current.FindStorageToAddResource(resourceType).AddResources(amount);

            if(residue != null)
            {
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
                Debug.Log($"{gameObject} - AddResources - Null");
            }
        }
    }

    public void RemoveResources(E_Resource resourceType, int amount)
    {
        if(StorageSystem.current.CheckCountResurces(resourceType) >= amount)
        {
            int residue = StorageSystem.current.FindStorageToRemoveResource(resourceType).RemoveResources(amount);
        
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
                Debug.Log($"{gameObject} - RemoveResources - Null");
            }
        }
    }
}