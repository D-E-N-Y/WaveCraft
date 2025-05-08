using System;
using System.Collections.Generic;
using UnityEngine;

public class TH_Storage : BuildingModule, IStorage, IPosition, IIndustrial
{
    public Action<E_Resource> UpdateCurrentAmount;
    
    [SerializeField] private E_Resource resource;
    [SerializeField] private int maxAmount;
    private int currentAmount;

    [SerializeField] private GameObject[] resourcePrefabs;
    [SerializeField] private List<Transform> actorPositions;

    public override void Initialize(Building building)
    {
        base.Initialize(building);

        currentAmount = 0;
    }

    public int AddResources(int amount)
    {
        currentAmount += amount;
        UpdatePrefabs();

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
        UpdatePrefabs();

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

    public E_Resource GetTypeResource() => resource;
    public bool isFreeSpace() => maxAmount - currentAmount != 0;
    public int GetCurrentAmount() => currentAmount;
    public int GetMaxAmount() => maxAmount;
    public List<Transform> GetPosition() => actorPositions;
}
