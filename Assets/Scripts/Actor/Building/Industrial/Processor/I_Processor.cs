using System.Collections;
using UnityEngine;

public class I_Processor : B_Industrial, IProcessing
{
    [SerializeField] private float factor;
    [SerializeField] private float timeProcess;

    private bool isProcessing;
    private int rawAmount;
    private int processedAmount;

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
        
        if(rawAmount > 0) 
            StartProcess();
        else
            isProcessing = false;

        processedAmount += (int)(1 * factor);

        // ResourceSystem.AddResource();
        
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
}
