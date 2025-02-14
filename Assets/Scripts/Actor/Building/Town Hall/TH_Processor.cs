using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TH_Processor : MonoBehaviour, IProcessor
{
    public Action<E_Resource> UpdadeProcessedAmount;
    public Action<E_Resource> UpdateRawAmount;

    public E_Resource resource { get; private set; }
    public float factor { get; private set; }
    public float timeProcess { get; private set; }
    private List<Transform> actorPositions;

    private bool isProcessing;
    public int rawAmount { get; private set; }
    public int processedAmount { get; private set; }

    public void Initialize(E_Resource resource, float factor, float timeProcess, List<Transform> actorPositions)
    {
        this.resource = resource;
        this.factor = factor;
        this.timeProcess = timeProcess;
        this.actorPositions = actorPositions;

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

    public List<Transform> GetPosition() => actorPositions;
}