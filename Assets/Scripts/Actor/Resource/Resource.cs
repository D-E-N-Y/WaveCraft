using System;
using UnityEngine;

public class Resource : Actor
{
    [SerializeField] private E_Resource resource;

    public override void Initialize()
    {
        base.Initialize();
        
        switch(resource)
        {
            case E_Resource.Wood:
                nameActor = "Tree";
                break;
            
            case E_Resource.Stone:
                nameActor = "Rock";
                break;
        }

        maxHP = MathF.Round(maxHP * transform.localScale.x * 0.4f);
        currentHP = maxHP;

        BuildSystem.current.BusyTakeArea(this);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        currentHP = Mathf.Max(currentHP - damage, 0);
        UpdateCurrentHP?.Invoke(currentHP);

        if(currentHP <= 0)
        {
            BuildSystem.current.ClearBusyTilemap(this);
            gameObject.SetActive(false);
        }
    }

    public E_Resource Type() => resource;
}
