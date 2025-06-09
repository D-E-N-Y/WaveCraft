using UnityEngine;

public class D_Pillar : B_Defence
{
    public D_Pillar connectPillar { get; private set; }
    public bool isConnect;

    public override string nameActor => "Pillar";
    
    private void OnTriggerEnter(Collider other)
    {
        Transform actor = other.transform;
            
        while (true)
        {
            if(actor.gameObject.GetComponent<D_Pillar>() != null)
            {
                break;
            }
            
            if(actor.transform.parent == null)
            {
                return;
            }
            
            actor = actor.transform.parent;
        }

        connectPillar = actor.gameObject.GetComponent<D_Pillar>();
    }

    private void OnTriggerExit(Collider other)
    {
        if(!connectPillar) return;

        Transform actor = other.transform;
        while (true)
        {
            if(actor.gameObject.GetComponent<D_Pillar>() != null)
            {
                break;
            }
            
            if(actor.transform.parent == null)
            {
                return;
            }
            
            actor = actor.transform.parent;
        }

        if(actor.gameObject == connectPillar.gameObject)
        {
            ResetConnect();
        }
    }

    public void ResetConnect() => connectPillar = null;
}