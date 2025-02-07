using System;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public Action UpdateCurrentHP;
    
    [SerializeField] protected GameObject interactionMenuUI;
    
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
        UpdateCurrentHP?.Invoke();
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
}
