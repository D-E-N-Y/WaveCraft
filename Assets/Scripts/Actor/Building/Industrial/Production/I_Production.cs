using System;
using System.Collections;
using UnityEngine;

public class I_Production : B_Industrial
{
    [SerializeField] private int amountProduce;
    [SerializeField] private float timeProduce;
    
    [SerializeField] private int maxStorage;
    private int currentAmount;

    private void StartProduce()
    {
        StartCoroutine(nameof(Produce));
    }

    private IEnumerator Produce()
    {
        yield return new WaitForSeconds(timeProduce);

        CompleteProduce();
    }

    private void CompleteProduce()
    {
        currentAmount = Math.Min(currentAmount + amountProduce, maxStorage);

        if(currentAmount < maxStorage)
            StartProduce();
    }

    public int RemoveResources(int amount)
    {
        currentAmount -= amount;

        if(currentAmount < 0)
        {
            return amount - currentAmount;
        }
        else
        {
            return amount;
        }
    }
}
