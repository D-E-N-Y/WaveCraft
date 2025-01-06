using System.Collections;
using UnityEngine;

public class I_Processors : B_Industrial
{
    [SerializeField] float factor;
    [SerializeField] float timeProcess;

    private bool isProcessing;
    private int amount;

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
        amount--;
        
        if(amount > 0) 
            StartProcess();
        else
            isProcessing = false;

        // ResourceSystem.AddResource();
        
    }

    public void AddResource(int amount)
    {
        this.amount += amount;

        if(!isProcessing) StartProcess();
    }
}
