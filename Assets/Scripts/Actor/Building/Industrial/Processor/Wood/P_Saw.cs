using System.Collections;
using UnityEngine;

public class P_Saw : TH_Processor
{
    [SerializeField] private GameObject saw;
    [SerializeField] private ParticleSystem sliversEffect;
    private float speedSaw = 10f;

    private Coroutine rotateSaw;

    public override void Initialize(Building building)
    {
        base.Initialize(building);

        speedSaw = 10f;
    }

    protected override void StartProcess()
    {
        base.StartProcess();

        sliversEffect.gameObject.SetActive(true);
        if (rotateSaw == null) 
        {
            rotateSaw = StartCoroutine(RotateSaw());
        }
    }

    protected override void CompleteProcess()
    {
        base.CompleteProcess();

        if (!isProcessing)
        {
            sliversEffect.gameObject.SetActive(false);
            if (rotateSaw != null)
            {
                StopCoroutine(rotateSaw);
                rotateSaw = null;
            }
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
