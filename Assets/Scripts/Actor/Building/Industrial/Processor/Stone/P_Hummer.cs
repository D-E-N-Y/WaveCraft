using System.Collections;
using UnityEngine;

public class P_Hummer : I_Processor
{
    [SerializeField] private GameObject hummer;
    [SerializeField] private GameObject stone;
    private float speedHummer = 10f;

    private Coroutine rotateHummer; 

    public override void Initialize()
    {
        base.Initialize();

        nameActor = "HUmmer";

        speedHummer = 10f;
    }

    protected override void StartProcess()
    {
        base.StartProcess();

        stone.SetActive(true);

        if(rotateHummer == null) rotateHummer = StartCoroutine(RotateHummer());
    }

    protected override void CompleteProcess()
    {
        base.CompleteProcess();

        if(!isProcessing)
        {
            stone.SetActive(false);
            if (rotateHummer != null) StopCoroutine(rotateHummer);
        }
    }

    private IEnumerator RotateHummer()
    {
        while(true)
        {
            while (hummer.transform.localEulerAngles.x <= 45)
            {
                hummer.transform.Rotate(Vector3.right * speedHummer * Time.deltaTime);
                yield return null;
            }

            speedHummer = 250f;

            while (hummer.transform.localEulerAngles.x >= 5)
            {
                hummer.transform.Rotate(Vector3.right * -speedHummer * Time.deltaTime);
                yield return null;
            }

            speedHummer = 10f;
        }
    }
}
