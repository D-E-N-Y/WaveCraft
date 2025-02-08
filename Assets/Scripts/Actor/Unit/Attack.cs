using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected string target;
    protected float damage;
    
    protected Actor targetActor;

    public virtual void Initialize(Unit unit)
    {
        damage = unit.GetDamage();
        targetActor = null;
    }

    protected virtual void OnTriggerEnter(Collider other) 
    {
        if(!other.gameObject.CompareTag(target) || targetActor) return;

        targetActor = other.gameObject.GetComponent<Actor>();
        targetActor.TakeDamage(damage);
    }

    public void NullTarget()
    {
        targetActor = null;
    }
}