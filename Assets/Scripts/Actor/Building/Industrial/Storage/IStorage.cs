using System.Collections.Generic;
using UnityEngine;

public interface IStorage
{
    bool isFreeSpace();
    int AddResources(int amount);
    int RemoveResources(int amount);
    int GetCurrentAmount();
    int GetMaxAmount();

    List<Transform> GetPosition();
}