using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Sawmill : I_Processor, IProcessor
{
    [SerializeField] private GameObject saw;
    [SerializeField] private GameObject mill;
    [SerializeField] private List<GameObject> processedWood;
    private float speedMill = 10f;
    private float speedSaw = 120f;

    private Coroutine rotateMill;
    private Coroutine rotateSaw; 

    public override void Initialize()
    {
        base.Initialize();

        nameActor = "Sawmill";

        speedMill = 10f;
        speedSaw = 120f;
    }

    public override void Built()
    {
        base.Built();

        rotateMill = StartCoroutine(RotateMill());
    }

    protected override void StartProcess()
    {
        base.StartProcess();

        speedMill = 40f;
        if(rotateSaw == null) rotateSaw = StartCoroutine(RotateSaw());
    }

    protected override void CompleteProcess()
    {
        base.CompleteProcess();

        if(processedAmount >= 0) processedWood[0].SetActive(true);
        if(processedAmount >= 25) processedWood[1].SetActive(true);
        if(processedAmount >= 50) processedWood[2].SetActive(true);
        if(processedAmount >= 75) processedWood[3].SetActive(true);
        if(processedAmount >= 100) processedWood[4].SetActive(true); 

        if(!isProcessing)
        {
            speedMill = 10f;
            if (rotateSaw != null) StopCoroutine(rotateSaw);
        }
    }

    public override int Unload()
    {
        foreach(GameObject wood in processedWood)
        {
            wood.SetActive(false);
        }
        
        return base.Unload();
    }

    private IEnumerator RotateMill()
    {
        while (true)
        {
            mill.transform.Rotate(Vector3.right * -speedMill * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator RotateSaw()
    {
        while (true)
        {
            saw.transform.Rotate(Vector3.forward * -speedSaw * Time.deltaTime);
            yield return null;
        }
    }
}
