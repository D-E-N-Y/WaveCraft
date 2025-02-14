using System.Collections.Generic;
using UnityEngine;

public interface IProduction
{
    int Unload();

    List<Transform> GetPosition();
}