using System.Collections;
using UnityEngine;

public class P_Crusher : TH_Processor
{
    [SerializeField] private GameObject crusher;
    private float speedCrusher;

    private Coroutine rotateCrusher;

    public override void Initialize()
    {
        base.Initialize();
        speedCrusher = 7f;
    }

    protected override void StartProcess()
    {
        base.StartProcess();

        rotateCrusher = StartCoroutine(RotateCrusher());
    }

    protected override void CompleteProcess()
    {
        base.CompleteProcess();
        StopCoroutine(rotateCrusher);
    }

    private IEnumerator RotateCrusher()
    {
        while (true)
        {
            crusher.transform.Rotate(Vector3.up * speedCrusher * Time.deltaTime);
            yield return null;
        }
    }
}
