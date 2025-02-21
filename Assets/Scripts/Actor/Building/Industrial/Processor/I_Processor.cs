using System;
using System.Collections;
using UnityEngine;

public class I_Processor : B_Industrial, IProcessor
{
    public Action UpdateProcessedAmount;
    public Action UpdateRawAmount;
    
    [SerializeField] protected float factor;
    [SerializeField] protected float timeProcess;

    protected bool isProcessing;
    public int rawAmount { get; protected set; }
    public int processedAmount { get; protected set; }

    public override void Initialize()
    {
        base.Initialize();
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

    protected virtual IEnumerator Processing()
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
