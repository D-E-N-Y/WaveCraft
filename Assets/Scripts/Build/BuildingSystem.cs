using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;

    public GridLayout gridLayout;
    private Grid grid;

    [SerializeField] private Tilemap mainTilemap;
    [SerializeField] private TileBase whiteTile;

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

    #endregion

    #region Building Placemment

    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
    }
        
    #endregion
}
