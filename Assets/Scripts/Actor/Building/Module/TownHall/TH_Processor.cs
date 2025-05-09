using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TH_Processor : BuildingModule, IProcessor, IPosition, IIndustrial
{
    public Action<E_Resource> UpdadeProcessedAmount;
    public Action<E_Resource> UpdateRawAmount; 

    [SerializeField] private E_Resource resource;
    [SerializeField] private float factor;
    [SerializeField] private float timeProcess;
    [SerializeField] private List<Transform> actorPositions;

    protected bool isProcessing;
    private int rawAmount;
    private int processedAmount;

    public override void Initialize(Building building)
    {
        base.Initialize(building);

        rawAmount = 0;
        processedAmount = 0;
    }

    protected virtual void StartProcess()
    {
        StartCoroutine(nameof(Processing));
        isProcessing = true;
    }

    private IEnumerator Processing()
    {
        yield return new WaitForSeconds(timeProcess);

        CompleteProcess();
    }

    protected virtual void CompleteProcess()
    {
        rawAmount--;
        UpdateRawAmount?.Invoke(resource);

        processedAmount += (int)(1 * factor);
        UpdadeProcessedAmount?.Invoke(resource);

        if(rawAmount > 0) 
        {
            StartProcess();
        }
        else
        {
            isProcessing = false;
        }
    }

    public void AddResources(int amount)
    {
        rawAmount += amount;
        UpdateRawAmount?.Invoke(resource);

        if(!isProcessing) StartProcess();
    }

    public int Unload()
    {
        int amount = processedAmount;
        processedAmount = 0;
        UpdadeProcessedAmount?.Invoke(resource);

        return amount;
    }

    public E_Resource GetTypeResource() => resource;
    public float GetFactor() => factor;
    public float GetTimeProcess() => timeProcess;
    public int GetRawAmount() => rawAmount;
    public int GetProcessedAmount() => processedAmount;
    public List<Transform> GetPosition() => actorPositions;
}