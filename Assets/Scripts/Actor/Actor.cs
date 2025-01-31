using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] protected GameObject interactionMenuUI;
    
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
        interactionMenuUI.SetActive(true);
    }

    public virtual void DisInteraction()
    {
        interactionMenuUI.SetActive(false);
    }

    public virtual void Initialize()
    {

    }
}
