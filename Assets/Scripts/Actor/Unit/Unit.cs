using UnityEngine;

public class Unit : Actor
{
    [SerializeField] protected float maxHP;
    protected float currentHP;
    public bool Died { private set; get; }

    [SerializeField] protected float armor;
    private float coefArmor = 1.5f;
    [SerializeField] protected float damage;

    [SerializeField] protected float moveSpeed;

    public void TakeDamage(float damage)
    {
        currentHP -= Mathf.Max(damage - (armor * coefArmor), 1f);

        if(currentHP <= 0)
        {
            Died = true;
        }
    }
}
