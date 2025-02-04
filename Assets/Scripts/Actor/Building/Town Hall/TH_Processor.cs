using System.Collections;
using UnityEngine;

public class TH_Processor : MonoBehaviour, IProcessor
{
    public E_Resource resource { get; private set; }
    public float factor { get; private set; }
    public float timeProcess { get; private set; }

    private bool isProcessing;
    private int rawAmount;
    public int processedAmount { get; private set; }

    public void Initialize(E_Resource resource, float factor, float timeProcess)
    {
        this.resource = resource;
        this.factor = factor;
        this.timeProcess = timeProcess;
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
        processedAmount += (int)(1 * factor);
        
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