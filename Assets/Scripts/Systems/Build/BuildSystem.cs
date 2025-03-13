using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class BuildSystem : GameSystem
{
    public static BuildSystem current;

    [SerializeField] private BaseBuild defaultBuild;
    [SerializeField] private WallBuild wallBuild;

    [SerializeField] private Tilemap gridTilemap;
    [SerializeField] private Tilemap freeTilemap;
    [SerializeField] private Tilemap busyTilemap;

    [SerializeField] private TileBase notCanPlaceTile;
    [SerializeField] private TileBase canPlaceTile;

    public GridLayout gridLayout;
    private Grid grid;

    private void Awake() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();

        current = this;

        defaultBuild.Initialize(this);
        wallBuild.Initialize(this);
    }

    private void Start()
    {
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }

    public void InitializeWithObject(Building building)
    {
        switch(building)
        {
            case D_Wall:
                wallBuild.enabled = true;
                wallBuild.InitializeBuilding(building.gameObject);
                break;

            default:
                defaultBuild.enabled = true;
                defaultBuild.InitializeBuilding(building.gameObject);
                break;
        }
    }

    #region Placement
        
    public void ActiveTilemap(bool active)
    {
        gridTilemap.gameObject.SetActive(active);
        freeTilemap.gameObject.SetActive(active);
        busyTilemap.gameObject.SetActive(active);
    }
    
    public bool CanBePlaced(Building building)
    {
        BoxCollider collider = building.GetComponent<BoxCollider>();

        Bounds bounds = collider.bounds;
        
        Vector3Int minCell = gridLayout.WorldToCell(bounds.min);
        Vector3Int maxCell = gridLayout.WorldToCell(bounds.max);

        for (int x = minCell.x; x <= maxCell.x; x++)
        {
            for (int y = minCell.y; y <= maxCell.y; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                Vector3 closestPoint = collider.ClosestPoint(gridLayout.CellToWorld(cellPos));

                if(busyTilemap.GetTile(gridLayout.WorldToCell(closestPoint)) == notCanPlaceTile)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void FreeTakeArea(Actor building, TileBase tile)
    {
        BoxCollider collider = building.GetComponent<BoxCollider>();
        if (!collider) return;

        Bounds bounds = collider.bounds;
        
        Vector3Int minCell = gridLayout.WorldToCell(bounds.min);
        Vector3Int maxCell = gridLayout.WorldToCell(bounds.max);

        for (int x = minCell.x; x <= maxCell.x; x++)
        {
            for (int y = minCell.y; y <= maxCell.y; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                Vector3 closestPoint = collider.ClosestPoint(gridLayout.CellToWorld(cellPos));

                freeTilemap.SetTile(gridLayout.WorldToCell(closestPoint), tile);
            }
        }
    }

    public void ClearFreeTilemap()
    {
        freeTilemap.ClearAllTiles();
    }

    public void BusyTakeArea(Actor building)
    {
        BoxCollider collider = building.GetComponent<BoxCollider>();
        if (!collider) return;

        Bounds bounds = collider.bounds;
        
        Vector3Int minCell = gridLayout.WorldToCell(bounds.min);
        Vector3Int maxCell = gridLayout.WorldToCell(bounds.max);

        for (int x = minCell.x; x <= maxCell.x; x++)
        {
            for (int y = minCell.y; y <= maxCell.y; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                Vector3 closestPoint = collider.ClosestPoint(gridLayout.CellToWorld(cellPos));

                busyTilemap.SetTile(gridLayout.WorldToCell(closestPoint), notCanPlaceTile);
            }
        }
    }

    public static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach(var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    public TileBase NotCanPlaceTile() => notCanPlaceTile;
    public TileBase CanPlaceTile() => canPlaceTile;

    #endregion
}
