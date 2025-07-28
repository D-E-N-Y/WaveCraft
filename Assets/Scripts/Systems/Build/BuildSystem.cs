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

    [SerializeField] private UI_CostBuildingPanel ui_costBuildingPanel;

    [SerializeField] private CircleZone placeZone;

    private BuildingSystems buildingSystems;

    public GridLayout gridLayout;
    private Grid grid;

    public override void Initialize()
    {
        current = this;

        defaultBuild.Initialize(this, ui_costBuildingPanel);
        wallBuild.Initialize(this, ui_costBuildingPanel);
        gateBuild.Initialize(this, ui_costBuildingPanel);

        placeZone.Initialize();

        grid = gridLayout.gameObject.GetComponent<Grid>();

        buildingSystems = BuildingSystems.current;
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }

    public void InitializeWithObject(Building building)
    {
        switch (building)
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

    public void ActivePlaceZone(bool active)
    {
        if (active)
        {
            placeZone.DrawLine(buildingSystems.GetExpansionPlaceZones());
        }

        placeZone.gameObject.SetActive(active);
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

                if (!IsPointCellInAdmissibleArea(closestPoint)) return false;

                if (busyTilemap.GetTile(gridLayout.WorldToCell(closestPoint)) == notCanPlaceTile ||
                   placedTilemap.GetTile(gridLayout.WorldToCell(closestPoint)) == canPlaceTile)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool IsPointCellInAdmissibleArea(Vector3 point)
    {
        int countIsInArea = 0;

        foreach (SCircleZone area in buildingSystems.GetExpansionPlaceZones())
        {
            if (Vector3.Distance(point, area._transform.position) <= area._radius)
            {
                countIsInArea++;
            }
        }

        return countIsInArea > 0;
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
        Collider[] hits = Physics.OverlapSphere(
            actor.transform.position,
            10f
        );

        if (hits.Length > 0)
        {
            List<Actor> _actors = hits
                .Select(x => x.gameObject.GetComponent<Actor>())
                .Where(x => x != null && x != actor &&
                    ((x is Building && !((Building)x).isPlace) || (x is Resource)))
                .ToList();

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

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    public bool IsPlacing()
    {
        if (defaultBuild.HasBuilding() || wallBuild.HasBuilding() || gateBuild.HasBuilding())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public TileBase NotCanPlaceTile() => notCanPlaceTile;
    public TileBase CanPlaceTile() => canPlaceTile;

    #endregion
}