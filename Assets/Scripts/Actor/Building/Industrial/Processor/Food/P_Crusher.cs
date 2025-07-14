using System.Collections;
using UnityEngine;

public class P_Crusher : TH_Processor
{
    [SerializeField] private GameObject crusher;
    [SerializeField] private ParticleSystem flourEffect;
    private float speedCrusher;

    private Coroutine rotateCrusher;

    public override void Initialize(Building building)
    {
        base.Initialize(building);
        speedCrusher = 7f;
    }

    protected override void StartProcess()
    {
        base.StartProcess();

        flourEffect.gameObject.SetActive(true);
        if (rotateCrusher == null)
        {
            rotateCrusher = StartCoroutine(RotateCrusher());
        }
    }

    protected override void CompleteProcess()
    {
        base.CompleteProcess();

        if (!isProcessing)
        {
            flourEffect.gameObject.SetActive(false);
            if (rotateCrusher != null)
            {
                StopCoroutine(rotateCrusher);
                rotateCrusher = null;
            }
        }
    }

    private IEnumerator RotateCrusher()
    {
        while (isProcessing)
        {
            crusher.transform.Rotate(Vector3.up * speedCrusher * Time.deltaTime);
            yield return null;
        }
    }
}
