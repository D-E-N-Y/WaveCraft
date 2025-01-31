using UnityEngine;

public class Resource : Actor
{
    [SerializeField] private E_Resource resource;
    
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        currentHP = Mathf.Max(currentHP - damage, 0);

        if(currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
