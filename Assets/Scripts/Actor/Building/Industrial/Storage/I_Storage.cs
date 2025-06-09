using System;
using System.Collections.Generic;
using UnityEngine;

public class I_Storage : B_Industrial, IStorage
{
    public Action UpdateCurrentAmount;
    
    [SerializeField] private GameObject[] resourcePrefabs;
    [SerializeField] private int maxAmount;
    private int currentAmount;

    public override string nameActor => resource + " storage";

    public override void Initialize()
    {
        base.Initialize();

        currentAmount = 0;
    }

    public override void Built()
    {
        base.Built();

        StorageSystem.current.AddStorage(this, resource);
    }

    public int AddResources(int amount)
    {
        currentAmount += amount;
        // UpdatePrefabs();

        if(currentAmount > maxAmount)
        {
            int residue = currentAmount - maxAmount;
            currentAmount = maxAmount;
            UpdateCurrentAmount?.Invoke();
            
            return residue;
        }
        else
        {
            UpdateCurrentAmount?.Invoke();
            return 0;
        }
    }

    public int RemoveResources(int amount)
    {
        currentAmount -= amount;
        // UpdatePrefabs();

        if(currentAmount < 0)
        {
            int residue = Mathf.Abs(currentAmount);
            currentAmount = 0;
            UpdateCurrentAmount?.Invoke();
            
            return residue;
        }
        else
        {
            UpdateCurrentAmount?.Invoke();
            return 0;
        }
    }

    private void UpdatePrefabs()
    {
        if(resourcePrefabs == null) return; 
        
        foreach(GameObject current in resourcePrefabs)
            current.SetActive(false);
        
        if(currentAmount > 0)
            resourcePrefabs[0].SetActive(true);
        
        if(currentAmount >= maxAmount * 0.3)
            resourcePrefabs[1].SetActive(true);

        if(currentAmount >= maxAmount * 0.6)
            resourcePrefabs[2].SetActive(true);

        if(currentAmount >= maxAmount * 1)
            resourcePrefabs[3].SetActive(true);
    }

    public bool isFreeSpace() => maxAmount - currentAmount != 0;
    public int GetCurrentAmount() => currentAmount;
    public int GetMaxAmount() => maxAmount;
}
