using UnityEngine;

public class Mining : Attack
{
    private UV_Worker worker;
    
    public override void Initialize(Unit unit)
    {
        base.Initialize(unit);
        worker = (UV_Worker)unit;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag(target) || targetActor) return;

        Transform actor = other.transform;

        while (true)
        {
            if (actor.transform.parent == null || actor.gameObject.GetComponent<Actor>() != null)
            {
                break;
            }

            actor = actor.transform.parent;
        }

        Resource resource = actor.GetComponent<Resource>();
        int residue = (int)(resource.GetCurrentHP() - damage);
        int minedResources = residue >= 0 ? (int)damage : (int)damage + residue;

        resource.TakeDamage(damage);
        worker.AddCurrentMineAmount(resource.Type(), minedResources);
    }
}