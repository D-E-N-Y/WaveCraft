using UnityEngine;

public class I_Storage : B_Industrial, IStorage
{
    [SerializeField] private int maxAmount;
    public int currentAmount { private set; get; }

    public bool isFreeSpace()
    {
        return maxAmount - currentAmount != 0;
    }

    public int AddResources(int amount)
    {
        currentAmount += amount;

        if(currentAmount > maxAmount)
        {
            int residue = currentAmount - maxAmount;
            currentAmount = maxAmount;
            return residue;
        }
        else
        {
            return 0;
        }
    }

    public int RemoveResources(int amount)
    {
        currentAmount -= amount;

        if(currentAmount < 0)
        {
            int residue = Mathf.Abs(currentAmount);
            currentAmount = 0;
            return residue;
        }
        else
        {
            return 0;
        }
    }
}
