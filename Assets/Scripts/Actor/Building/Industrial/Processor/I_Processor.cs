using System;
using System.Collections;
using UnityEngine;

public class I_Processor : B_Industrial, IProcessor
{
    public Action UpdateProcessedAmount;
    public Action UpdateRawAmount;
    
    [SerializeField] private float factor;
    [SerializeField] private float timeProcess;

    private bool isProcessing;
    public int rawAmount { get; private set; }
    public int processedAmount { get; private set; }

    public override void Initialize()
    {
        base.Initialize();

        nameActor = resourse + " processor";
    }

    public override void Built()
    {
        base.Built();

        ProcessorSystem.current.AddProcessor(resourse, this);
    }

    private void StartProcess()
    {
        StartCoroutine(nameof(Processing));
        isProcessing = true;
    }

    private IEnumerator Processing()
    {
        yield return new WaitForSeconds(timeProcess);

        CompleteProcess();
    }

    private void CompleteProcess()
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

    public int Unload()
    {
        int amount = processedAmount;
        processedAmount = 0;
        UpdateProcessedAmount?.Invoke();

        return amount;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
