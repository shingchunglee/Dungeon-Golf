using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Grid grid;

    public bool ReadFromTilemap = true;

    private List<Tilemap> _tilemaps;
    public List<Tilemap> tilemaps
    {
        get
        {
            if (_tilemaps == null)
            {
                _tilemaps = new List<Tilemap>();
                _tilemaps.Add(map_wall);
                _tilemaps.Add(map_floor);
                _tilemaps.Add(map_obstacles);
                _tilemaps.Add(map_trap_damamge);
                _tilemaps.Add(map_trap_void);
                _tilemaps.Add(map_trap_floor);
                _tilemaps.Add(map_trap_chest);
                _tilemaps.Add(map_chest);
                _tilemaps.Add(map_enemy_spawn);
            }
            return _tilemaps;
        }
        private set { }
    }

    public Tilemap map_wall;
    public Tilemap map_floor;
    public Tilemap map_obstacles;
    public Tilemap map_trap_damamge;
    public Tilemap map_trap_void;
    public Tilemap map_trap_floor;
    public Tilemap map_trap_chest;
    public Tilemap map_chest;
    public Tilemap map_enemy_spawn;

    public TileType[,] gridDungeon;
    public EntityType[,] gridEntities;

    public Vector2Int gridSize = Vector2Int.zero;

    // Note that a tilemap is always at least measured from [0,0], even if the first tile is at [1000,1000].
    // The origin can go below this if the tile map has tiles below [0,0].
    public Vector2Int gridOrigin = Vector2Int.zero;

    public GameObject marker;

    public GameObject startObjectToCalculatePath;
    public GameObject TargetObjectToCalculatePathTo;

    private void Start()
    {
        ScanMap();

        // TestLogGrid();
        // TestPathfinding();
    }



    private void ScanMap()
    {
        CreateGrid();
        ScanTilemaps();
        ScanGameobjects();
    }

    private void CreateGrid()
    {
        if (ReadFromTilemap)
        {
            foreach (var tilemap in tilemaps)
            {
                BoundsInt gridBounds = tilemap.cellBounds;

                if (gridBounds.size.x > gridSize.x)
                {
                    gridSize.x = gridBounds.size.x;
                }
                if (gridBounds.size.y > gridSize.y)
                {
                    gridSize.y = gridBounds.size.y;
                }

                if (gridBounds.x < gridOrigin.x)
                {
                    gridOrigin.x = gridBounds.x;
                }
                if (gridBounds.y < gridOrigin.y)
                {
                    gridOrigin.y = gridBounds.y;
                }
            }

            grid = new Grid(gridSize, gridOrigin);
        }
    }

    private void ScanTilemaps()
    {
        ScanTilemapByLayer(map_floor, FloorType.FLOOR);
        ScanTilemapByLayer(map_wall, FloorType.WALL);
        ScanTilemapByLayer(map_obstacles, FloorType.OBSTACLE);
        ScanTilemapByLayer(map_trap_damamge, entityType: EntityType.TRAP_DAMAGE);
        ScanTilemapByLayer(map_trap_void, FloorType.VOID);
        ScanTilemapByLayer(map_trap_floor, FloorType.TRAP_FLOOR);
        ScanTilemapByLayer(map_trap_chest, entityType: EntityType.TRAP_CHEST);
        ScanTilemapByLayer(map_chest, entityType: EntityType.CHEST);
        ScanTilemapByLayer(map_enemy_spawn, entityType: EntityType.ENEMY_SPAWN);
    }

    /// <summary>
    /// Scans a tilemap and adds a FloorType or an EntityType to the Node at the corresponding location for each
    /// tile on the tilemap.
    /// </summary>
    /// <param name="tilemap"></param>
    /// <param name="floorType"></param>
    private void ScanTilemapByLayer(Tilemap tilemap, FloorType? floorType = null, EntityType? entityType = null)
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        // Debug.Log($"Map: {tilemap.gameObject.name} | Origin: [{bounds.x},{bounds.y}], Size: [{bounds.size.x}, {bounds.size.y}] ==================================================================");

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];

                int offsetX = 0;
                int offsetY = 0;

                if (bounds.x != gridOrigin.x)
                {
                    offsetX = Mathf.Abs(gridOrigin.x) - Mathf.Abs(bounds.x);
                }

                if (bounds.y != gridOrigin.y)
                {
                    offsetY = Mathf.Abs(gridOrigin.y) - Mathf.Abs(bounds.y);
                }

                if (tile != null)
                {
                    // Debug.Log($"Map: {tilemap.gameObject.name}, Relative: [{x},{y}], "
                    // + $"Grid: [{x + offsetX},{y + offsetY}] tile: {tile.name}");

                    if (floorType != null)
                    {
                        grid.nodes[x + offsetX, y + offsetY].floorType = (FloorType)floorType;
                    }

                    if (entityType != null)
                    {
                        grid.nodes[x + offsetX, y + offsetY].entitiesOnTile.Add((EntityType)entityType);
                    }

                }
                else
                {
                    // Debug.Log($"Map: {tilemap.gameObject.name}, Relative: [{x},{y}], "
                    // + "Grid: [{x + offsetX},{y + offsetY}] tile: null");
                }
            }
        }
    }

    private void ScanGameobjects()
    {

        // Record Player position on EntityMap
        int playerX = Mathf.FloorToInt(PlayerManager.Instance.playerWizard.transform.position.x);
        int playerY = Mathf.FloorToInt(PlayerManager.Instance.playerWizard.transform.position.y);

        Vector2Int playerPos = new Vector2Int(playerX, playerY);

        AddEntityByWorldPosition(EntityType.PLAYER, playerPos);

        //Recorder EnemyPositions on EntityMap
        foreach (var enemy in GameManager.Instance.enemyManager.enemyUnitsOnLevel)
        {
            int enemyX = Mathf.FloorToInt(enemy.transform.position.x);
            int enemyY = Mathf.FloorToInt(enemy.transform.position.y);

            Vector2Int enemyPos = new Vector2Int(enemyX, enemyY);

            AddEntityByWorldPosition(EntityType.ENEMY, enemyPos);
        }

    }

    public Node GetNodeByWorldPosition(Vector2Int position)
    {
        int offsetX = Mathf.Abs(gridOrigin.x);
        int offsetY = Mathf.Abs(gridOrigin.y);

        return grid.nodes[position.x + offsetX, position.y + offsetY];
    }

    public Node GetNodeByWorldPosition(int xPos, int yPos)
    {
        int offsetX = Mathf.Abs(gridOrigin.x);
        int offsetY = Mathf.Abs(gridOrigin.y);

        return grid.nodes[xPos + offsetX, yPos + offsetY];
    }

    public Vector2Int GetWorldPositionByNode(Node node)
    {
        int offsetX = Mathf.Abs(gridOrigin.x);
        int offsetY = Mathf.Abs(gridOrigin.y);

        return new Vector2Int(node.position.x - offsetX, node.position.y - offsetY);
    }

    public Vector2Int GetWorldPositionByGridPosition(int xPos, int yPos)
    {
        int offsetX = Mathf.Abs(gridOrigin.x);
        int offsetY = Mathf.Abs(gridOrigin.y);

        return new Vector2Int(xPos - offsetX, yPos - offsetY);
    }

    public Vector2Int GetWorldPositionByGridPosition(Vector2Int position)
    {
        int offsetX = Mathf.Abs(gridOrigin.x);
        int offsetY = Mathf.Abs(gridOrigin.y);

        return new Vector2Int(position.x - offsetX, position.y - offsetY);
    }

    public void AddEntityByWorldPosition(EntityType type, Vector2Int position)
    {
        int offsetX = Mathf.Abs(gridOrigin.x);
        int offsetY = Mathf.Abs(gridOrigin.y);

        grid.nodes[position.x + offsetX, position.y + offsetY].entitiesOnTile.Add(type);
    }

    private void TestLogGrid()
    {
        Debug.Log($"Largest size tilemap is [{gridSize}]. Grid origin is: {gridOrigin}");

        LogGridDungeon();
        LogGridEntities();
    }

    private void LogGridDungeon()
    {
        string mapString = "";

        //GetLength(1) gets the number of elements on the y axis.
        for (int y = grid.gridSize.y - 1; y > -1; y--)
        {
            string mapLine = "";
            for (int x = 0; x < grid.gridSize.x; x++)
            {
                switch (grid.nodes[x, y].floorType)
                {
                    case FloorType.EMPTY:
                        mapLine += " -";
                        break;
                    case FloorType.FLOOR:
                        mapLine += " _ ";
                        break;
                    case FloorType.WALL:
                        mapLine += " #";
                        break;
                    case FloorType.OBSTACLE:
                        mapLine += " O";
                        break;
                    case FloorType.TRAP_FLOOR:
                        mapLine += " T";
                        break;
                    case FloorType.VOID:
                        mapLine += " V";
                        break;
                    default:
                        mapLine += " ?";
                        break;
                }
            }

            mapString += mapLine + "\n";

        }
        Debug.Log("Map scan results: \n" +
            mapString);
    }

    private void LogGridEntities()
    {
        string mapString = "";

        //GetLength(1) gets the number of elements on the y axis.
        for (int y = grid.gridSize.y - 1; y > -1; y--)
        {
            string mapLine = "";
            for (int x = 0; x < grid.gridSize.x; x++)
            {
                if (grid.nodes[x, y].entitiesOnTile.Count == 0)
                {
                    mapLine += " -";
                    continue;
                }

                foreach (EntityType entity in grid.nodes[x, y].entitiesOnTile)
                {
                    switch (entity)
                    {
                        case EntityType.EMPTY:
                            mapLine += " -";
                            break;
                        case EntityType.PLAYER:
                            mapLine += " @";
                            break;
                        case EntityType.ENEMY:
                            mapLine += " X";
                            break;
                        case EntityType.TRAP_DAMAGE:
                            mapLine += " T";
                            break;
                        case EntityType.TRAP_CHEST:
                            mapLine += " H";
                            break;
                        case EntityType.CHEST:
                            mapLine += " C";
                            break;
                        case EntityType.ENEMY_SPAWN:
                            mapLine += " !";
                            break;
                        default:
                            mapLine += " ?";
                            break;
                    }
                }
            }

            mapString += mapLine + "\n";

        }
        Debug.Log("Entity scan results: \n" +
            mapString);
    }


    /// <summary>
    /// This generates a list of Nodes that represents the path from a starting node to a target node.
    /// The first Node on the list is the starting node, the last is the target node, and the rest are path.
    /// 
    ///  Adapted from Tarodev Pathfinding - Understanding A* https://youtu.be/i0x5fj4PqP4?si=7GlfcaXwQ5OXd64d
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="targetNode"></param>
    /// <returns></returns>
    public static List<Node> FindPath(Node startNode, Node targetNode, bool doEnemiesBlock)
    {
        var toSearch = new List<Node>() { startNode };
        var processed = new List<Node>();

        while (toSearch.Any())
        {
            var current = toSearch[0];
            foreach (var t in toSearch)
            {
                if (t.F < current.F || t.F == current.F && t.H < current.H)
                {
                    current = t;
                }
            }

            processed.Add(current);
            toSearch.Remove(current);


            // This is section assigns a list of Nodes to a list that defines the path.
            if (current == targetNode)
            {
                var currentPathTile = targetNode;
                var path = new List<Node>();
                while (currentPathTile != startNode)
                {
                    path.Add(currentPathTile);
                    currentPathTile = currentPathTile.Connection;
                }

                // Calculation doesn't add start node to list, so it is manually added here.
                path.Add(startNode);

                // Reverse path so it starts with startNode
                path.Reverse();

                // Add target node so list ends with targetNode.
                // path.Add(targetNode);

                return path;
            }

            if (doEnemiesBlock)
            {
                foreach (var neighbor in current.Neighbors
                                        .Where(t => t.IsWalkableForEnemyAtLocation(startNode.position) &&
                                        !processed.Contains(t)))
                {
                    bool inSearch = toSearch.Contains(neighbor);

                    var costToNeighbor = current.G + current.GetDistanceToOtherNode(neighbor);

                    if (!inSearch || costToNeighbor < neighbor.G)
                    {
                        neighbor.SetG(costToNeighbor);
                        neighbor.SetConnection(current);

                        if (!inSearch)
                        {
                            neighbor.SetH(neighbor.GetDistanceToOtherNode(targetNode));
                            toSearch.Add(neighbor);
                        }
                    }
                }
            }
            else
            {
                foreach (var neighbor in current.Neighbors
                                        .Where(t => t.IsWalkableIgnoringEnemies() &&
                                        !processed.Contains(t)))
                {
                    bool inSearch = toSearch.Contains(neighbor);

                    var costToNeighbor = current.G + current.GetDistanceToOtherNode(neighbor);

                    if (!inSearch || costToNeighbor < neighbor.G)
                    {
                        neighbor.SetG(costToNeighbor);
                        neighbor.SetConnection(current);

                        if (!inSearch)
                        {
                            neighbor.SetH(neighbor.GetDistanceToOtherNode(targetNode));
                            toSearch.Add(neighbor);
                        }
                    }
                }
            }


        }
        return null;
    }

    public void TestPathfinding()
    {
        var worldPosStartObj = new Vector2Int(
                    Mathf.FloorToInt(startObjectToCalculatePath.transform.position.x),
                    Mathf.FloorToInt(startObjectToCalculatePath.transform.position.y)
                );

        var worldPosTargetObj = new Vector2Int(
            Mathf.FloorToInt(TargetObjectToCalculatePathTo.transform.position.x),
            Mathf.FloorToInt(TargetObjectToCalculatePathTo.transform.position.y)
        );

        var node2 = GetNodeByWorldPosition(worldPosTargetObj);
        var node1 = GetNodeByWorldPosition(worldPosStartObj);

        var pathToPrint = FindPath(node1, node2, true);

        DrawPathIndicators(pathToPrint);
    }

    private List<GameObject> pathMarkers = new List<GameObject>();

    public void DrawPathIndicators(List<Node> pathToPrint)
    {
        foreach (var obj in pathMarkers)
        {
            Destroy(obj);
        }

        if (pathToPrint == null)
        {
            Debug.Log("Can't draw path because path doesn't exist.");
            return;
        }

        foreach (var node in pathToPrint)
        {
            var worldPos = GetWorldPositionByNode(node);

            // Debug.Log($"Path node: {worldPos}");

            pathMarkers.Add(Instantiate(marker, new Vector3(worldPos.x, worldPos.y, 1), marker.transform.rotation));
        }
    }
}
