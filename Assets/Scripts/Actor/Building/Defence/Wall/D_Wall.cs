using TMPro;
using UnityEngine;

public class D_Wall : B_Defence
{
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform endTransform;
    
    [SerializeField] private E_WallType wallType;
    
    public D_Wall connectColumn { get; private set; }
    public bool isConnect;

    public override void Initialize()
    {
        base.Initialize();

        nameActor = wallType.ToString();
    }
    
    public float GetWallLength() => Vector3.Distance(startTransform.position, endTransform.position);

    public E_WallType Type() => wallType;

    private void OnTriggerEnter(Collider other)
    {
        if(wallType == E_WallType.Column)
        {
            Transform actor = other.transform;
            
            while (true)
            {
                if(actor.gameObject.GetComponent<D_Wall>() != null)
                {
                    break;
                }
                
                if(actor.transform.parent == null)
                {
                    return;
                }
                
                actor = actor.transform.parent;
            }

            D_Wall _wall = actor.gameObject.GetComponent<D_Wall>();
            
            if(_wall.Type() == E_WallType.Column)
            {
                connectColumn = _wall;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!connectColumn) return;

        Transform actor = other.transform;
        while (true)
        {
            if(actor.gameObject.GetComponent<D_Wall>() != null)
            {
                break;
            }
            
            if(actor.transform.parent == null)
            {
                return;
            }
            
            actor = actor.transform.parent;
        }

        if(actor.gameObject == connectColumn.gameObject)
        {
            ResetConnect();
        }
    }

    public void ResetConnect() => connectColumn = null;
}
