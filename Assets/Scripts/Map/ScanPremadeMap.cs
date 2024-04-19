using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Tilemaps;

public class ScanPremadeTilemap : MonoBehaviour
{
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

    private void Start()
    {
        ScanMap();
        LogGrid();
    }

    private void ScanMap()
    {
        ScanTilemaps();
        ScanEntityMap();
    }

    private void ScanTilemaps()
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

            gridDungeon = new TileType[gridSize.x, gridSize.y];



            ScanTilemapByLayer(map_floor, TileType.FLOOR);
            ScanTilemapByLayer(map_wall, TileType.WALL);
            ScanTilemapByLayer(map_obstacles, TileType.OBSTACLE);
            ScanTilemapByLayer(map_trap_damamge, TileType.TRAP_DAMAGE);
            ScanTilemapByLayer(map_trap_void, TileType.TRAP_VOID);
            ScanTilemapByLayer(map_trap_floor, TileType.TRAP_FLOOR);
            ScanTilemapByLayer(map_trap_chest, TileType.TRAP_CHEST);
            ScanTilemapByLayer(map_chest, TileType.CHEST);
            ScanTilemapByLayer(map_enemy_spawn, TileType.ENEMY_SPAWN);


        }
    }

    private void ScanTilemapByLayer(Tilemap tilemap, TileType tileType)
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

                    gridDungeon[x + offsetX, y + offsetY] = tileType;
                }
                else
                {
                    // Debug.Log($"Map: {tilemap.gameObject.name}, Relative: [{x},{y}], "
                    // + "Grid: [{x + offsetX},{y + offsetY}] tile: null");
                }
            }
        }
    }

    private void ScanEntityMap()
    {
        gridEntities = new EntityType[gridSize.x, gridSize.y];

        // Record Player position on EntityMap
        int playerX = Mathf.FloorToInt(PlayerManager.Instance.playerWizard.transform.position.x);
        int playerY = Mathf.FloorToInt(PlayerManager.Instance.playerWizard.transform.position.y);

        Vector2Int playerPos = new Vector2Int(playerX, playerY);

        AddEntityByWorldPosition(EntityType.Player, playerPos);

        //Recorder EnemyPositions on EntityMap
        foreach (var enemy in GameManager.Instance.enemyManager.enemyUnitsOnLevel)
        { 
            int enemyX = Mathf.FloorToInt(enemy.transform.position.x);
            int enemyY = Mathf.FloorToInt(enemy.transform.position.y);

            Vector2Int enemyPos = new Vector2Int(playerX, playerY);

            AddEntityByWorldPosition(EntityType.Enemy, enemyPos);
        }

    }

    public void AddEntityByWorldPosition(EntityType type, Vector2Int position)
    {
        int offsetX = Mathf.Abs(gridOrigin.x);
        int offsetY = Mathf.Abs(gridOrigin.y);

        gridEntities[position.x + offsetX, position.y + offsetY] = type;
    }

    private void LogGrid()
    {
        Debug.Log($"Largest size tilemap is [{gridSize}].");

        LogGridDungeon();
        LogGridEntities();
    }

    private void LogGridDungeon()
    {
        string mapString = "";

        //GetLength(1) gets the number of elements on the y axis.
        for (int y = gridDungeon.GetLength(1) - 1; y > -1; y--)
        {
            string mapLine = "";
            for (int x = 0; x < gridDungeon.GetLength(0); x++)
            {
                switch (gridDungeon[x, y])
                {
                    case TileType.EMPTY:
                        mapLine += " X";
                        break;
                    case TileType.FLOOR:
                        mapLine += " _ ";
                        break;
                    case TileType.WALL:
                        mapLine += " #";
                        break;
                    case TileType.OBSTACLE:
                        mapLine += " O";
                        break;
                    case TileType.TRAP_CHEST:
                        mapLine += " C";
                        break;
                    case TileType.TRAP_DAMAGE:
                        mapLine += " D";
                        break;
                    case TileType.TRAP_FLOOR:
                        mapLine += " D";
                        break;
                    case TileType.TRAP_VOID:
                        mapLine += " V";
                        break;
                    case TileType.ENEMY_SPAWN:
                        mapLine += " S";
                        break;
                    case TileType.CHEST:
                        mapLine += " C";
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
        for (int y = gridEntities.GetLength(1) - 1; y > -1; y--)
        {
            string mapLine = "";
            for (int x = 0; x < gridEntities.GetLength(0); x++)
            {
                switch (gridEntities[x, y])
                {
                    case EntityType.Empty:
                        mapLine += " 0";
                        break;
                    case EntityType.Player:
                        mapLine += " P";
                        break;
                    case EntityType.Enemy:
                        mapLine += " X";
                        break;
                    case EntityType.Neutral:
                        mapLine += " N";
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
}
