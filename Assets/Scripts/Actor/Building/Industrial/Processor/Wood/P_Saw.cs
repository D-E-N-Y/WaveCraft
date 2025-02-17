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
        float minLimit = -6f;
        float maxLimit = 8f;
        float currentRotation = 0f;
        
        while (true)
        {
            while (currentRotation < maxLimit)
            {
                float step = speedSaw * Time.deltaTime;
                saw.transform.Rotate(Vector3.forward * step);
                saw.transform.Rotate(Vector3.right * step);
                currentRotation += step;
                yield return null;
            }

            speedSaw *= -1;

            while (currentRotation > minLimit)
            {
                float step = speedSaw * Time.deltaTime;
                saw.transform.Rotate(Vector3.forward * step);
                saw.transform.Rotate(Vector3.right * step);
                currentRotation += step;
                yield return null;
            }

            speedSaw *= -1;
        }
    }
}
