using System.Collections;
using UnityEngine;

public class P_Saw : I_Processor
{
    [SerializeField] private GameObject saw;
    private float speedSaw = 10f;

    private Coroutine rotateSaw;

    void Start()
    {
        Debug.Log(saw.transform.localEulerAngles.z);
    }

    public override void Initialize()
    {
        base.Initialize();

        nameActor = "Saw";

        speedSaw = 10f;
    }

    protected override void StartProcess()
    {
        base.StartProcess();

        if(rotateSaw == null) rotateSaw = StartCoroutine(RotateSaw());
    }

    protected override void CompleteProcess()
    {
        base.CompleteProcess();

        if(!isProcessing)
        {
            if (rotateSaw != null) StopCoroutine(rotateSaw);
        }
    }

    private IEnumerator RotateSaw()
    {
        bool condition = saw.transform.localEulerAngles.z <= 8;
        
        while (true)
        {
            while (condition)
            {
                saw.transform.Rotate(Vector3.forward * speedSaw * Time.deltaTime);
                saw.transform.Rotate(Vector3.right * speedSaw * Time.deltaTime);
                yield return null;
            }
            
            speedSaw *= -1;
            
            if(speedSaw > 0)
            {
                condition = saw.transform.localEulerAngles.z <= 8;
            }
            else
            {
                condition = saw.transform.localEulerAngles.z >= 0;
            }
        }
    }
}
