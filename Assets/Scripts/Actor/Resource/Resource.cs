using System;
using System.Collections;
using UnityEngine;

public class Resource : Actor
{
    [SerializeField] private E_Resource resource;
    private Collider _collider;

    public override string nameActor => resource.ToString();

    public override void Initialize()
    {
        base.Initialize();

        maxHP = MathF.Round(maxHP * transform.localScale.x * 0.2f);
        currentHP = maxHP;

        BuildSystem.current.BusyTakeArea(this);

        _collider = GetComponent<Collider>();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        currentHP = Mathf.Max(currentHP - damage, 0);
        UpdateCurrentHP?.Invoke(currentHP);

        if (currentHP <= 0)
        {
            Death();
        }
    }

    protected override void Death()
    {
        if (resource == E_Resource.Stone)
        {
            BoxCollider _box = (BoxCollider)_collider;
            _box.size = new Vector3(
                _box.size.x * 3,
                _box.size.y,
                _box.size.z * 3
            );
        }
        else if (resource == E_Resource.Wood)
        {
            CapsuleCollider _capsule = (CapsuleCollider)_collider;
            _capsule.radius = _capsule.radius * 3;
        }

        BuildSystem.current.ClearBusyTilemap(this);
        BuildSystem.current.RedrawNeighborsBusyArea(this);

        base.Death();
    }

    public E_Resource Type() => resource;
}
