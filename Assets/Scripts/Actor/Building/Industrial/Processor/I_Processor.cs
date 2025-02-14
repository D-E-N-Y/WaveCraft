using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Processor : B_Industrial, IProcessor
{
    public Action UpdateProcessedAmount;
    public Action UpdateRawAmount;
    
    [SerializeField] private float factor;
    [SerializeField] private float timeProcess;

    protected bool isProcessing;
    public int rawAmount { get; private set; }
    public int processedAmount { get; private set; }

    public override void Initialize()
    {
        base.Initialize();
    }

    public void Initialize(E_Resource resource, float factor, float timeProcess, List<Transform> actorPositions)
    {
        this.resource = resource;
        this.factor = factor;
        this.timeProcess = timeProcess;
        this.actorPositions = actorPositions;

        rawAmount = 0;
        processedAmount = 0;
    }

    public override void Built()
    {
        base.Built();

        ProcessorSystem.current.AddProcessor(resource, this);
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

    protected virtual  void CompleteProcess()
    {
        rawAmount--;
        UpdateRawAmount?.Invoke();

        processedAmount += (int)(1 * factor);
        UpdateProcessedAmount?.Invoke();

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
        UpdateRawAmount?.Invoke();

        if(!isProcessing) StartProcess();
    }

    public virtual int Unload()
    {
        int amount = processedAmount;
        processedAmount = 0;
        UpdateProcessedAmount?.Invoke();

        return amount;
    }
}
