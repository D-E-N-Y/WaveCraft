using System;
using System.Collections.Generic;
using UnityEngine;

public class TH_Storage : MonoBehaviour, IStorage
{
    public Action<E_Resource> UpdateCurrentAmount;
    
    public E_Resource resource { private set; get; }
    private int maxAmount;
    private int currentAmount;
    private List<Transform> actorPositions;

    public void Initialize(E_Resource resource, int maxAmount, List<Transform> actorPositions)
    {
        this.resource = resource;
        this.maxAmount = maxAmount;
        this.actorPositions = actorPositions;
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

    public int GetCurrentAmount() => currentAmount;
    public int GetMaxAmount() => maxAmount;
    public List<Transform> GetPosition() => actorPositions;
}
