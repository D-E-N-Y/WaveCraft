using System.Collections;
using UnityEngine;

public class P_Mill : I_Processor
{
    [SerializeField] private GameObject mill;
    private float speedMill;

    private Coroutine rotateMill;

    public override string nameActor => "Mill";

    public override void Initialize()
    {
        base.Initialize();

        speedMill = 7f;
    }

    public override void Built()
    {
        base.Built();

        rotateMill = StartCoroutine(RotateMill());
    }

    protected override void StartProcess()
    {
        base.StartProcess();

        speedMill = 30f;
    }

    protected override void CompleteProcess()
    {
        base.CompleteProcess();

        if(!isProcessing)
        {
            speedMill = 7f;
        }
    }

    public override int Unload()
    {
        return base.Unload();
    }

    private IEnumerator RotateMill()
    {
        while (true)
        {
            mill.transform.Rotate(Vector3.forward * speedMill * Time.deltaTime);
            yield return null;
        }
    }
}
