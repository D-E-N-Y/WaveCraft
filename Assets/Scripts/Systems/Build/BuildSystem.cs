using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildSystem : GameSystem
{
    public static BuildSystem current;

    [SerializeField] private BaseBuild defaultBuild;
    [SerializeField] private WallBuild wallBuild;
    [SerializeField] private GateBuild gateBuild;

    [SerializeField] private Tilemap gridTilemap;
    [SerializeField] private Tilemap freeTilemap;
    [SerializeField] private Tilemap placedTilemap;
    [SerializeField] private Tilemap busyTilemap;

    [SerializeField] private TileBase notCanPlaceTile;
    [SerializeField] private TileBase canPlaceTile;

    public GridLayout gridLayout;
    private Grid grid;

    public override void Initialize()
    {
        current = this;

        defaultBuild.Initialize(this);
        wallBuild.Initialize(this);
        gateBuild.Initialize(this);

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
            case D_Pillar:
                wallBuild.enabled = true;
                wallBuild.InitializeBuilding(building);
                break;

            case D_Gate:
                gateBuild.enabled = true;
                gateBuild.InitializeBuilding(building);
                break;

            default:
                defaultBuild.enabled = true;
                defaultBuild.InitializeBuilding(building);
                break;
        }
    }

    public void ContinueWall(D_Pillar startPillar)
    {
        wallBuild.enabled = true;
        wallBuild.ContinueWall(startPillar);
    }

    #region Placement
        
    public void ActiveTilemap(bool active)
    {
        gridTilemap.gameObject.SetActive(active);
        freeTilemap.gameObject.SetActive(active);
        placedTilemap.gameObject.SetActive(active);
        busyTilemap.gameObject.SetActive(active);
    }
    
    public bool CanBePlaced(Building building)
    {
        Collider collider = building.GetComponent<Collider>();

        Bounds bounds = collider.bounds;
        
        Vector3Int minCell = gridLayout.WorldToCell(bounds.min);
        Vector3Int maxCell = gridLayout.WorldToCell(bounds.max);

        for (int x = minCell.x; x <= maxCell.x; x++)
        {
            for (int y = minCell.y; y <= maxCell.y; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                Vector3 closestPoint = collider.ClosestPoint(gridLayout.CellToWorld(cellPos));

                if(busyTilemap.GetTile(gridLayout.WorldToCell(closestPoint)) == notCanPlaceTile ||
                   placedTilemap.GetTile(gridLayout.WorldToCell(closestPoint)) == canPlaceTile)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void FreeTakeArea(Actor building, TileBase tile)
    {
        Collider collider = building.GetComponent<Collider>();
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

    public void BusyTakeArea(Actor actor)
    {
        Collider collider = actor.GetComponent<Collider>();
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

    public void RedrawNeighborsBusyArea(Actor actor)
    {
        RaycastHit[] hits = Physics.SphereCastAll(
            actor.transform.position,
            1f, actor.transform.forward,
            20f
        );

        if (hits.Length > 0)
        {
            List<Actor> _actors = hits
                .Where(x => x.collider.gameObject.GetComponent<Building>() != null || x.collider.gameObject.GetComponent<Resource>() != null)
                .Select(x => x.collider.gameObject.GetComponent<Actor>())
                .ToList();

            // List<Resource> _resources = hits
            //     .Where(x => x.collider.gameObject.GetComponent<Resource>() != null)
            //     .Select(x => x.collider.gameObject.GetComponent<Resource>())
            //     .ToList();

            _actors.Remove(actor);

            _actors.ForEach(x => BusyTakeArea(x));
        }
    }

    public void ClearBusyTilemap(Actor building)
    {
        Collider collider = building.GetComponent<Collider>();
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

                busyTilemap.SetTile(gridLayout.WorldToCell(closestPoint), null);
            }
        }
    }

    public void PlaceTakeArea(Actor building)
    {
        Collider collider = building.GetComponent<Collider>();
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

                placedTilemap.SetTile(gridLayout.WorldToCell(closestPoint), canPlaceTile);
            }
        }
    }

    public void ClearPlacedTilemap()
    {
        placedTilemap.ClearAllTiles();
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
