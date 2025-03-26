using TMPro;
using UnityEngine;

public class D_Wall : B_Defence
{
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform endTransform;
    
    [SerializeField] private E_WallType wallType;
 
    public override void Initialize()
    {
        base.Initialize();

        nameActor = wallType.ToString();
    }
    
    public float GetWallLength() => Vector3.Distance(startTransform.position, endTransform.position);

    public E_WallType Type() => wallType;
}
