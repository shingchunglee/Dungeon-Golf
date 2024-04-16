using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ScanPremadeTilemap : MonoBehaviour
{
    public bool ReadFromTilemap = true;

    public List<Tilemap> tilemaps;

    public Tilemap map_wall;
    public Tilemap map_floor;
    public Tilemap map_obstacles;
    public Tilemap map_trap_damamge;
    public Tilemap map_trap_void;
    public Tilemap map_trap_floor;
    public Tilemap map_trap_chest;
    public Tilemap map_chest;
    public Tilemap map_enemy_spawn;

    public TileType[,] grid;

    public Vector2Int gridSize = Vector2Int.zero;

    // Note that a tilemap is always at least measured from [0,0], even if the first tile is at [1000,1000].
    // The origin can go below this if the tile map has tiles below [0,0].
    public Vector2Int gridOrigin = Vector2Int.zero;

    private void Start()
    {
        ScanTilemaps();
        LogGrid();
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

            grid = new TileType[gridSize.x, gridSize.y];

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

                    grid[x + offsetX, y + offsetY] = tileType;
                }
                else
                {
                    // Debug.Log($"Map: {tilemap.gameObject.name}, Relative: [{x},{y}], "
                    // + "Grid: [{x + offsetX},{y + offsetY}] tile: null");
                }
            }
        }
    }

    private void LogGrid()
    {
        Debug.Log($"Largest size tilemap is [{gridSize}].");

        string mapString = "";

        //GetLength(1) gets the number of elements on the y axis.
        for (int y = grid.GetLength(1) - 1; y > -1; y--)
        {
            string mapLine = "";
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                switch (grid[x, y])
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
}
