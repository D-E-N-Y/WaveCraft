using UnityEngine;
using UnityEngine.AI;

public abstract class Unit : Actor
{
    public bool Died { private set; get; }

    [SerializeField] protected float armor;
    private float coefArmor = 1.5f;
    [SerializeField] protected float damage;

    public UnitMovement movement { get; protected set; }
    public Animator animator { get; protected set; }

    public override void Initialize()
    {
        base.Initialize();

        movement = gameObject.AddComponent<UnitMovement>();
        movement.Initialize();

        animator = gameObject.GetComponent<Animator>();
    }

    public override void TakeDamage(float damage)
    {
        currentHP -= Mathf.Max(damage - (armor * coefArmor), 1f);

        if(currentHP <= 0)
        {
            Died = true;
        }
    }

    public float GetDamage() => damage;
    public float GetArmor() => armor;
    public float GetSpeed() => GetComponent<NavMeshAgent>().speed;
}
