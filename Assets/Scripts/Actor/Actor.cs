using System;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour, IPosition
{
    public Action UpdateCurrentHP;
    public Action DestroyActor;

    [SerializeField] protected GameObject interactionMenuUI;
    [SerializeField] protected List<Transform> actorPositions;    

    public string nameActor { get; protected set; }

    [SerializeField] protected float maxHP;
    protected float currentHP;
    
    public float GetMaxHP()
    {
        return maxHP;
    }

    public float GetCurrentHP()
    {
        return currentHP;
    }

    public virtual void TakeDamage(float damage)
    {
        
    }

    public virtual void Interaction()
    {
        interactionMenuUI.SetActive(true);
    }

    public virtual void DisInteraction()
    {
        interactionMenuUI.SetActive(false);
    }

    public virtual void Initialize()
    {
        currentHP = maxHP;
    }

    private void OnDestroy() 
    {
        DestroyActor?.Invoke();
    }

    public List<Transform> GetPosition() => actorPositions;
}
