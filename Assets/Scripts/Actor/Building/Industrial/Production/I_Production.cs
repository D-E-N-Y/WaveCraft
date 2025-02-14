using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Production : B_Industrial, IProduction
{
    public Action UpdateCountResources;
    
    [SerializeField] private int amountProduce;
    [SerializeField] private float timeProduce;
    
    [SerializeField] private int maxStorage;
    private int currentAmount;

    public override void Initialize()
    {
        base.Initialize();

        nameActor = resource + " prodaction";
        currentAmount = 0;
    }

    public override void Built()
    {
        base.Built();

        StartProduce();
    }

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
        UpdateCountResources?.Invoke();

        if(currentAmount < maxStorage)
            StartProduce();
    }

    public int Unload()
    {
        int amount = currentAmount;
        currentAmount = 0;
        UpdateCountResources?.Invoke();

        StartCoroutine(nameof(Produce));

        return amount;
    }

    public int GetMaxAmount() => maxStorage;
    public int GetProduceAmount() => currentAmount;
}
