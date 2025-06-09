using System;
using UnityEngine;

public class D_Wall : B_Defence
{
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform endTransform;

    public override string nameActor => "Wall";
    
    public float GetWallLength() => MathF.Round(Vector3.Distance(startTransform.position, endTransform.position));
}
