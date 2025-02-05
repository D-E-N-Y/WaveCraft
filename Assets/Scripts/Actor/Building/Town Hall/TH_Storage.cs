using System;
using UnityEngine;

public class TH_Storage : MonoBehaviour, IStorage
{
    public Action<E_Resource> UpdateCurrentAmount;
    
    public E_Resource resource { private set; get; }
    public int maxAmount { private set; get; }
    public int currentAmount { private set; get; }

    public void Initialize(E_Resource resource, int maxAmount)
    {
        this.resource = resource;
        this.maxAmount = maxAmount;
    }

    public bool isFreeSpace()
    {
        return maxAmount - currentAmount != 0;
    }

    public int AddResources(int amount)
    {
        currentAmount += amount;

        if(currentAmount > maxAmount)
        {
            int residue = currentAmount - maxAmount;
            currentAmount = maxAmount;
            UpdateCurrentAmount?.Invoke(resource);

            return residue;
        }
        else
        {
            UpdateCurrentAmount?.Invoke(resource);
            return 0;
        }
    }

    public int RemoveResources(int amount)
    {
        currentAmount -= amount;

        if(currentAmount < 0)
        {
            int residue = Mathf.Abs(currentAmount);
            currentAmount = 0;
            UpdateCurrentAmount?.Invoke(resource);
            
            return residue;
        }
        else
        {
            UpdateCurrentAmount?.Invoke(resource);
            
            return 0;
        }
    }

    public int GetCurrentAmount()
    {
        return currentAmount;
    }

    public int GetMaxAmount()
    {
        return maxAmount;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
