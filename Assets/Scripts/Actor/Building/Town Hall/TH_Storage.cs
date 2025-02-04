using UnityEngine;

public class TH_Storage : MonoBehaviour, IStorage
{
    public E_Resource resource { private set; get; }
    private int maxAmount;
    private int currentAmount;

    public void Initialize(E_Resource resource, int maxAmount)
    {
        this.resource = resource;
        this.maxAmount = maxAmount;
    }

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

    public int GetCurrentAmount()
    {
        return currentAmount;
    }

    public int GetMaxAmount()
    {
        return maxAmount;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
