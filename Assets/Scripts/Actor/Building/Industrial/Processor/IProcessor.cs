using UnityEngine;

public interface IProcessor
{
    void AddResources(int amount);
    int Unload();

    Vector3 GetPosition();
}