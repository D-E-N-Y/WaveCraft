using UnityEngine;

public class Mining : Attack
{
    private UP_Worker worker;
    
    public override void Initialize(Unit unit)
    {
        base.Initialize(unit);
        worker = (UP_Worker)unit;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(targetActor) return;
        base.OnTriggerEnter(other);

        Resource resource = other.GetComponent<Resource>();

        worker.AddCurrentMineAmount(resource.Type(), (int)damage);
    }
}