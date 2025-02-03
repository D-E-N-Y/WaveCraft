using UnityEngine;

public class Unit : Actor
{
    public bool Died { private set; get; }

    [SerializeField] protected float armor;
    private float coefArmor = 1.5f;
    [SerializeField] protected float damage;

    protected UnitMovement movement;

    public override void Initialize()
    {
        base.Initialize();

        movement = gameObject.AddComponent<UnitMovement>();
        movement.Initialize();
    }

    public override void TakeDamage(float damage)
    {
        currentHP -= Mathf.Max(damage - (armor * coefArmor), 1f);

        if(currentHP <= 0)
        {
            Died = true;
        }
    }
}
