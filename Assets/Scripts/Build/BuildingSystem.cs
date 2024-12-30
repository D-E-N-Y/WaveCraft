using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;

    public GridLayout gridLayout;
    private Grid grid;

    [SerializeField] private Tilemap gridTilemap;
    [SerializeField] private Tilemap freeTilemap;
    [SerializeField] private Tilemap busyTilemap;

    [SerializeField] private TileBase notCanPlaceTile;
    [SerializeField] private TileBase canPlaceTile;

    public GameObject[] prefabs;

    private PlaceableObject objectToPlace;

    #region Unity methods

    private void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    private void Update() 
    {
        ChooseBuilding();
        BuildingMove();
    }

    #endregion

    #region Utils

    private void ChooseBuilding()
    {
        if(objectToPlace) 
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
        if(!objectToPlace) 
            return;

        Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());

        if(!objectToPlace.Placed)
            if(CanBePlaced(objectToPlace))
                FreeTakeArea(start, objectToPlace.Size, canPlaceTile);
            else
                FreeTakeArea(start, objectToPlace.Size, notCanPlaceTile);

        if(Input.GetKeyDown(KeyCode.Return))
        {
            objectToPlace.Rotate();
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            if(CanBePlaced(objectToPlace))
            {
                objectToPlace.Place();
                BusyTakeArea(start, objectToPlace.Size);

                objectToPlace = null;
                ActiveTilemap(false);
            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(objectToPlace.gameObject);
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
        Vector3 position = SnapCoordinateToGrid(GetMouseWorldPosition());

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        objectToPlace.Initialize();
        obj.AddComponent<ObjectDrag>();

        ActiveTilemap(true);
    }

    private bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        
        area.size = placeableObject.Size + new Vector3Int(1, 1, 1);

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

        // freeTilemap.BoxFill(start, tile, start.x, start.y, 
        //                     start.x + size.x, start.y + size.y);

    }

    public void BusyTakeArea(Vector3Int start, Vector3Int size)
    {
        busyTilemap.BoxFill(start, notCanPlaceTile, start.x, start.y, 
                            start.x + size.x, start.y + size.y);
    }
        
    #endregion
}
