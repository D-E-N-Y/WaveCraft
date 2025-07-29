using System;
using System.Collections.Generic;
using System.Linq;
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
        UpdatePrefabs();
    }

    public int AddResources(int amount)
    {
        currentAmount += amount;

        if(currentAmount > maxAmount)
        {
            int residue = currentAmount - maxAmount;
            currentAmount = maxAmount;
            UpdateCurrentAmount?.Invoke();
            UpdatePrefabs();

            return residue;
        }
        else
        {
            UpdateCurrentAmount?.Invoke();
            UpdatePrefabs();

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
            UpdateCurrentAmount?.Invoke();
            UpdatePrefabs();
            
            return residue;
        }
        else
        {
            UpdateCurrentAmount?.Invoke();
            UpdatePrefabs();
            
            return 0;
        }
    }

    private void UpdatePrefabs()
    {
        if(resourcePrefabs == null) return; 
        
        foreach(GameObject current in resourcePrefabs)
            current.SetActive(false);

        for (int i = 0; i < resourcePrefabs.Length; i++)
        {
            float factor = i / ((float)resourcePrefabs.Length - 1f);
            float value = maxAmount * factor;

            resourcePrefabs[i].SetActive(currentAmount >= value && currentAmount > 0);
        }
    }

    public bool isFreeSpace() => maxAmount - currentAmount != 0;
    public int GetCurrentAmount() => currentAmount;
    public int GetMaxAmount() => maxAmount;
}
