using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildSystem : MonoBehaviour
{
    public static BuildSystem current;

    public GridLayout gridLayout;
    private Grid grid;

    [SerializeField] private Tilemap gridTilemap;
    [SerializeField] private Tilemap freeTilemap;
    [SerializeField] private Tilemap busyTilemap;

    [SerializeField] private TileBase notCanPlaceTile;
    [SerializeField] private TileBase canPlaceTile;

    public GameObject[] prefabs;

    private Building building;
    private MaterialBuilding materialBuilding;

    #region Unity methods

    private void Awake() 
    {
        current = this;
    }
    
    private void Start()
    {
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    private void Update() 
    {
        BuildingMove();
    }

    #endregion

    #region Utils

    private void ChooseBuilding()
    {
        if(building) 
            return;

        if(Input.GetKeyDown(KeyCode.Alpha1))
            InitializeWithObject(prefabs[0]);
        
        if(Input.GetKeyDown(KeyCode.Alpha2))
            InitializeWithObject(prefabs[1]);
        
        if(Input.GetKeyDown(KeyCode.Alpha3))
            InitializeWithObject(prefabs[2]);
        
        if(Input.GetKeyDown(KeyCode.Alpha4))
            InitializeWithObject(prefabs[3]);
        
        if(Input.GetKeyDown(KeyCode.Alpha5))
            InitializeWithObject(prefabs[4]);
    }

    private void BuildingMove()
    {
        if(!building) 
            return;

        Vector3Int start = gridLayout.WorldToCell(building.GetStartPosition());

        // Grid
        if(!building.isPlace)
        {
            if(CanBePlaced(building))
            {
                // materialBuilding.SetColor(MaterialBuilding.BuildColor.canPlace);
                FreeTakeArea(start, building.Size, canPlaceTile);
            }
            else
            {
                // materialBuilding.SetColor(MaterialBuilding.BuildColor.notCanPlace);
                FreeTakeArea(start, building.Size, notCanPlaceTile);
            }
        }

        // 
        if(Input.GetKeyDown(KeyCode.Return))
        {
            building.Rotate();
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            if(CanBePlaced(building))
            {
                building.Place();
                // materialBuilding.SetColor(MaterialBuilding.BuildColor.placed);
                
                BusyTakeArea(start, building.Size);
                
                BuildTask task = new BuildTask(building);
                TaskSystem.current.AddTask(task);

                building = null;
                materialBuilding = null;
                
                ActiveTilemap(false);

                // !!! add task to build for workers
                // !!! wait worker
            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(building.gameObject);
            ActiveTilemap(false);
        }
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit raycastHit))
            return raycastHit.point;
        else
            return Vector3.zero;
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
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

    #endregion

    #region Building Placemment

    private void ActiveTilemap(bool active)
    {
        gridTilemap.gameObject.SetActive(active);
        freeTilemap.gameObject.SetActive(active);
        busyTilemap.gameObject.SetActive(active);
    }
    
    public void InitializeWithObject(GameObject prefab)
    {
        if(building) return;
        
        Vector3 position = SnapCoordinateToGrid(GetMouseWorldPosition());

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        building = obj.GetComponent<Building>();
        building.Initialize();
        obj.AddComponent<ObjectDrag>();

        // materialBuilding = building.GetComponent<MaterialBuilding>();
        // materialBuilding.StartPlace();

        ActiveTilemap(true);
    }

    private bool CanBePlaced(Building building)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(building.GetStartPosition());
        
        area.size = building.Size + new Vector3Int(1, 1, 1);

        TileBase[] baseArray = GetTilesBlock(area, busyTilemap);

        foreach (var b in baseArray)
        {
            if(b == notCanPlaceTile)
                return false;
        }

        return true;
    }

    public void FreeTakeArea(Vector3Int start, Vector3Int size, TileBase tile)
    {
        freeTilemap.ClearAllTiles();

        for (int x = start.x; x <= start.x + size.x; x++)
        {
            for (int y = start.y; y <= start.y + size.y; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                freeTilemap.SetTile(position, tile);
            }
        }
    }

    public void BusyTakeArea(Vector3Int start, Vector3Int size)
    {
        for (int x = start.x; x <= start.x + size.x; x++)
        {
            for (int y = start.y; y <= start.y + size.y; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                busyTilemap.SetTile(position, notCanPlaceTile);
            }
        }
    }
        
    #endregion

    #region Destroy Building
        
    // Choose Building
    // Add task to destroy building
    // Wait worker
    // Destroy
    // Add 70% from coast resources

    #endregion
}
