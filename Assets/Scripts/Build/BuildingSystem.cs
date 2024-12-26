using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;

    public GridLayout gridLayout;
    private Grid grid;

    [SerializeField] private Tilemap mainTilemap;
    [SerializeField] private TileBase notCanPlaceTile;
    [SerializeField] private TileBase CanPlaceTile;

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
        

        if(!objectToPlace)
            return;
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(CanBePlaced(objectToPlace))
            {
                objectToPlace.Place();
                Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
                TakeArea(start, objectToPlace.Size);
            }
            else
            {
                Destroy(objectToPlace.gameObject);
            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(objectToPlace.gameObject);
        }
    }

    #endregion

    #region Utils

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

    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(GetMouseWorldPosition());

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
    }

    private bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = placeableObject.Size;

        TileBase[] baseArray = GetTilesBlock(area, mainTilemap);

        foreach (var b in baseArray)
        {
            if(b == notCanPlaceTile)
                return false;
        }

        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        mainTilemap.BoxFill(start, notCanPlaceTile, start.x, start.y, 
                            start.x + size.x, start.y + size.y);
    }
        
    #endregion
}
