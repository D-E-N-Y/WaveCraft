using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Quarry : I_Processor
{
    [SerializeField] List<GameObject> resourcePrefabs;
    [SerializeField] List<GameObject> miners;

    [SerializeField] GameObject rock;

    [SerializeField] GameObject mechanism;
    private float speedMechanism;

    [SerializeField] GameObject elevator;
    private float speedElevator;

    public override void Initialize()
    {
        base.Initialize();

        nameActor = "Quarry";

        speedMechanism = 20f;
        speedElevator = 10f;
    }

    public override void Built()
    {
        base.Built();

        foreach(GameObject miner in miners)
            miner.SetActive(true);
    }

    protected override void StartProcess()
    {
        base.StartProcess();

        rock.SetActive(true);

        foreach(GameObject miner in miners)
            miner.GetComponent<Animator>().SetBool("isMine", true);
    }

    protected override IEnumerator Processing()
    {
        yield return new WaitForSeconds(timeProcess);

        if(rawAmount - 1 == 0)
        {
            rock.SetActive(false);
            
            foreach(GameObject miner in miners)
            {
                miner.GetComponent<Animator>().SetBool("isMine", false);
            }
        }

        resourcePrefabs[0].SetActive(true);

        yield return RotateMechanism(180f, 1f);
        yield return RotateElevator(35f, -1f);

        yield return new WaitForSeconds(1f);

        resourcePrefabs[0].SetActive(false);
        
        rawAmount--;
        UpdateRawAmount?.Invoke();

        processedAmount += (int)(1 * factor);
        UpdateProcessedAmount?.Invoke();

        UpdatePrefabs();

        yield return RotateElevator(35f, 1f);
        yield return RotateMechanism(180f, -1f);
        CompleteProcess();
    }

    protected override void CompleteProcess()
    {
        if(rawAmount > 0) 
        {
            StartProcess();
        }
        else
        {
            isProcessing = false;
            rock.SetActive(false);
            
            foreach(GameObject miner in miners)
            {
                miner.GetComponent<Animator>().SetBool("isMine", false);
            }
        }
    }

    public override int Unload()
    {
        int value = base.Unload();
        UpdatePrefabs();
        
        return value;
    }

    private IEnumerator RotateMechanism(float angleY, float direction)
    {
        float remainingAngle = Mathf.Abs(angleY);

        while (remainingAngle > 0)
        {
            float deltaAngle = speedMechanism * Time.deltaTime;
            
            deltaAngle = Mathf.Min(deltaAngle, remainingAngle);

            mechanism.transform.Rotate(Vector3.up * direction * deltaAngle);

            remainingAngle -= deltaAngle;

            yield return null;
        }
    }

    private IEnumerator RotateElevator(float angleY, float direction)
    {
        float remainingAngle = Mathf.Abs(angleY);

        while (remainingAngle > 0)
        {
            float deltaAngle = speedElevator * Time.deltaTime;
            
            deltaAngle = Mathf.Min(deltaAngle, remainingAngle);

            elevator.transform.Rotate(Vector3.right * direction * deltaAngle);

            remainingAngle -= deltaAngle;

            yield return null;
        }
    }

    private void UpdatePrefabs()
    {
        if(resourcePrefabs == null) return; 
        
        for(int i = 1; i < resourcePrefabs.Count; i++)
            resourcePrefabs[i].SetActive(false);

        if(processedAmount > 0)
            resourcePrefabs[1].SetActive(true);
        
        if(processedAmount >= 25)
            resourcePrefabs[2].SetActive(true);

        if(processedAmount >= 50)
            resourcePrefabs[3].SetActive(true);

        if(processedAmount >= 75)
            resourcePrefabs[4].SetActive(true);
    }
}