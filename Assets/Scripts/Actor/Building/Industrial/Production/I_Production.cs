using System;
using System.Collections;
using UnityEngine;

public abstract class I_Production : B_Industrial, IProduction
{
    public Action UpdateCountResources;

    [SerializeField] private int amountProduce;
    [SerializeField] private float timeProduce;

    [SerializeField] private int maxStorage;
    private int currentAmount;

    public override void Initialize()
    {
        base.Initialize();
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

        if (currentAmount < maxStorage)
            StartProduce();
    }

    public int Unload(int value)
    {
        int amount = currentAmount;
        currentAmount = Mathf.Max(0, currentAmount - value);
        UpdateCountResources?.Invoke();

        StartCoroutine(nameof(Produce));

        return amount - currentAmount;
    }

    public int GetMaxAmount() => maxStorage;
    public int GetProduceAmount() => currentAmount;
    public float GetTimeProduce() => timeProduce;
}
