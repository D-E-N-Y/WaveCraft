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
        if(targetActor) return;
        base.OnTriggerEnter(other);

        Resource resource = other.GetComponent<Resource>();

        worker.AddCurrentMineAmount(resource.Type(), (int)damage);
    }
}