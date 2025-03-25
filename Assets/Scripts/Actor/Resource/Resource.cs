using UnityEngine;

public class Resource : Actor
{
    [SerializeField] private E_Resource resource;
    
    private GridCollider gridCollider;
    private Vector3Int Size;
    private Vector3[] Vertices;

    private void Start() 
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();

        switch(resource)
        {
            case E_Resource.Wood:
                nameActor = "Tree";
                break;
            
            case E_Resource.Stone:
                nameActor = "Rock";
                break;
        }

        gridCollider = new GridCollider(gameObject);
        
        Vertices = gridCollider.GetColliderVertexPositionsLocal();
        Size = gridCollider.CalculateSizeInCells();

        BuildSystem.current.BusyTakeArea(this);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        currentHP = Mathf.Max(currentHP - damage, 0);
        UpdateCurrentHP?.Invoke();

        if(currentHP <= 0)
        {
            BuildSystem.current.ClearBusyTilemap(this);
            gameObject.SetActive(false);
        }
    }

    public E_Resource Type() => resource;
}
