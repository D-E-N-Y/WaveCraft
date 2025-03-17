using TMPro;
using UnityEngine;

public class D_Wall : B_Defence
{
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform endTransform;

    public float GetWallLength() => Vector3.Distance(startTransform.position, endTransform.position);
}
