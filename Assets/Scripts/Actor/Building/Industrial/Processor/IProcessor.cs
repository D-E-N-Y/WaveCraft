using System.Collections.Generic;
using UnityEngine;

public interface IProcessor
{
    void AddResources(int amount);
    int Unload();

    List<Transform> GetPosition();
}