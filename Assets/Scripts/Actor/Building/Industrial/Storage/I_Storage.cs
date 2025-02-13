using System;
using UnityEngine;

public class I_Storage : B_Industrial, IStorage
{
    public Action UpdateCurrentAmount;
    
    [SerializeField] private int maxAmount;
    private int currentAmount;

    public override void Initialize()
    {
        base.Initialize();

        nameActor = resourse + " storage";
        currentAmount = 0;
    }

    public override void Built()
    {
        base.Built();

        StorageSystem.current.AddStorage(this, resourse);
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
