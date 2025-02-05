using System;
using System.Collections;
using UnityEngine;

public class TH_Processor : MonoBehaviour, IProcessor
{
    public Action<E_Resource> UpdadeProcessedAmount;
    public Action<E_Resource> UpdateRawAmount;

    public E_Resource resource { get; private set; }
    public float factor { get; private set; }
    public float timeProcess { get; private set; }

    private bool isProcessing;
    public int rawAmount { get; private set; }
    public int processedAmount { get; private set; }

    public void Initialize(E_Resource resource, float factor, float timeProcess)
    {
        this.resource = resource;
        this.factor = factor;
        this.timeProcess = timeProcess;

        rawAmount = 0;
        processedAmount = 0;
    }

    private void StartProcess()
    {
        StartCoroutine(nameof(Processing));
        isProcessing = true;
    }

    private IEnumerator Processing()
    {
        Debug.Log("Processing");
        yield return new WaitForSeconds(timeProcess);

        CompleteProcess();
    }

    private void CompleteProcess()
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
            Debug.Log("Complete processing");
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

        return amount;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}