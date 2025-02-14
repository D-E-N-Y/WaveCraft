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

    public override void Built()
    {
        base.Built();

        ProcessorSystem.current.AddProcessor(resourse, this);
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

    public List<Transform> GetPosition() => actorPositions;
}
