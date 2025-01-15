using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] protected float maxHP;
    protected float currentHP;
    
    public float GetMaxHP()
    {
        return maxHP;
    }

    public virtual void TakeDamage(float damage)
    {
        
    }

    public virtual void Interaction()
    {
        
    }

    public virtual void Initialize()
    {

    }
}
