using System.Collections;
using UnityEngine;

public class P_Hummer : TH_Processor
{
    [SerializeField] private GameObject hummer;
    [SerializeField] private GameObject stone;
    [SerializeField] private ParticleSystem stoneDustEffect;
    private float speedHummer = 10f;

    private Coroutine rotateHummer; 
    
    public override void Initialize(Building building)
    {
        base.Initialize(building);

        speedHummer = 10f;
    }

    protected override void StartProcess()
    {
        base.StartProcess();

        stone.SetActive(true);
        if (rotateHummer == null) 
        {
            rotateHummer = StartCoroutine(RotateHummer());
        }
    }

    protected override void CompleteProcess()
    {
        base.CompleteProcess();

        if (!isProcessing)
        {
            stone.SetActive(false);
            if (rotateHummer != null)
            {
                StopCoroutine(rotateHummer);
                rotateHummer = null;
            }
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

            stoneDustEffect.gameObject.SetActive(false);
            speedHummer = 250f;

            while (hummer.transform.localEulerAngles.x >= 5)
            {
                hummer.transform.Rotate(Vector3.right * -speedHummer * Time.deltaTime);
                yield return null;
            }

            stoneDustEffect.gameObject.SetActive(true);
            stoneDustEffect.Play();

            speedHummer = 10f;
        }
    }
}
