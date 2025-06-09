using System;
using Unity.VisualScripting;
using UnityEngine;

public class Resource : Actor
{
    [SerializeField] private E_Resource resource;

    public override string nameActor => resource.ToString();

    public override void Initialize()
    {
        base.Initialize();

        maxHP = MathF.Round(maxHP * transform.localScale.x * 0.4f);
        currentHP = maxHP;

        BuildSystem.current.BusyTakeArea(this);
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
        BuildSystem.current.ClearBusyTilemap(this);
        BuildSystem.current.RedrawNeighborsBusyArea(this);

        base.Death();
    }

    public E_Resource Type() => resource;
}
