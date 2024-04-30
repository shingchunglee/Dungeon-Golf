using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;

public class ProcedualGeneration : MonoBehaviour
{
    // [SerializeField]
    // GameObject wallPrefab;
    // [SerializeField]
    // GameObject floorPrefab;
    [SerializeField]
    Tilemap traps;
    [SerializeField]
    Tilemap obstacles;

    [SerializeField]
    Tilemap walls;
    [SerializeField]
    Tilemap floor;
    [SerializeField]
    Tilemap enemies;
    [SerializeField]
    TileBase floorTile;
    [SerializeField]
    TileBase voidTile;
    [SerializeField]
    MapTileRules tileRules;

    public Vector2 PlayerSpawn;
    public Vector2 GoalSpawn;

    [SerializeField]
    ConvolutionRules[] convolutionRules;
    public TileType[,] Grid { private set; get; }

    [SerializeField]
    public int Width = 50;
    [SerializeField]
    public int Height = 50;

    public void Main()
    {
        ClearGrids();

        bool[,] grid = Automata(Width, Height, 0.45f, 5, 4, 10);
        Debug.Log($"Grid created w:{Width}, h:{Height}");


        FillDungeon(grid);
        Convolution(grid);

        RenderTiles(grid);

        GetPlayerGoalPositions(Grid);
    }

    private void ClearGrids()
    {
        traps.ClearAllTiles();
        obstacles.ClearAllTiles();
        walls.ClearAllTiles();
        floor.ClearAllTiles();
        enemies.ClearAllTiles();
        foreach (Transform child in traps.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in obstacles.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in walls.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in floor.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in enemies.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void ReadFromGridMethod()
    {
        // * LOOP THROUGH TileMaps wall, floor, obstacles, trap_damage, trap_void, trap_floor, trap_chest, chest, enemy_spawn
        // * FIND THE LARGEST X, Y COORDINATE IN THESE TILEMAPS, THIS INDICATES THE SIZE OF THE MAP
        // * CREATE A List<List<TileType>> matrix THE SIZE OF X AND Y (SEE METHODS BELOW FOR EXAMPLE)
        // * LOOP THROUGH THE TILEMAPS AGAIN, FOR EVERY TILE THAT EXISTS, SET THAT matrix[Y][X] = TileType ACCORDING TO WHICH TILEMAP IS BEING LOOPED
        // ! START WITH FLOOR THEN WALL, THEN OTHERS SINCE FLOOR IS ALWAYS AT THE BOTTOM, AND OTHER THINGS CAN STACK ON TOP OF IT
        // * SET PUBLIC VARIABLE Grid = matrix
    }

    // == RENDER == //

    private void RenderTiles(bool[,] grid)
    {
        // RenderWalls(grid);
        RenderWalls(grid);
    }

    private void RenderWalls(bool[,] grid)
    {
        walls.ClearAllTiles();

        int height = grid.GetLength(0);
        int width = grid.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[y, x] == true)
                {
                    walls.SetTile(new Vector3Int(x, y, 0), voidTile);
                }
            }
        }
    }

    // == CELLULAR AUTOMATA == //

    private bool[,] AutomataIteration(bool[,] grid, int birthThreshold, int survivalThreshold)
    {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);
        bool[,] newGrid = new bool[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                newGrid[y, x] = false;
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                bool newValue = NewValueAtPosition(grid, x, y, birthThreshold, survivalThreshold);
                newGrid[y, x] = newValue;
            }
        }

        return newGrid;
    }

    private bool[,] Automata(int width, int height, float initialProbability, int birthThreshold, int survivalThreshold, int iterations)
    {
        bool[,] grid = GenerateGrid(width, height, initialProbability);

        for (int i = 0; i < iterations; i++)
        {
            grid = AutomataIteration(grid, birthThreshold, survivalThreshold);
        }
        FillBorders(ref grid);

        int[,] caverns = IdentifyCaverns(grid);

        int fillNumber = GetLargestCavern(caverns);
        FillLoneCaverns(ref grid, caverns, fillNumber);

        // return (grid, caverns);
        return grid;
    }

    private void FillBorders(ref bool[,] grid)
    {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    grid[y, x] = true;
                }
            }
        }
    }

    private void FillLoneCaverns(ref bool[,] grid, int[,] caverns, int fillNumber)
    {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (caverns[y, x] != fillNumber)
                {
                    grid[y, x] = true;
                }
            }
        }
    }

    private int GetLargestCavern(int[,] caverns)
    {
        var dict = new Dictionary<int, int>();
        int height = caverns.GetLength(0);
        int width = caverns.GetLength(1);


        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int fillNumber = caverns[y, x];
                if (dict.ContainsKey(fillNumber))
                {
                    dict[fillNumber]++;
                }
                else
                {
                    dict[fillNumber] = 1;
                }
            }
        }

        return dict.FirstOrDefault(x => x.Value == dict.Values.Max()).Key;
    }

    private bool NewValueAtPosition(bool[,] grid, int x, int y, int birthThreshold, int survivalThreshold)
    {
        bool cellIsAlive = grid[y, x] == true;
        int aliveNeighbors = CountAliveNeighbors(grid, x, y);

        if (cellIsAlive && aliveNeighbors >= survivalThreshold)
        {
            return true;
        }
        else if (!cellIsAlive && aliveNeighbors >= birthThreshold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private int CountAliveNeighbors(bool[,] grid, int x, int y)
    {
        int count = 0;

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i == x && j == y)
                {
                    continue;
                }

                if (SpotIsOffGrid(grid, i, j))
                {
                    count++;
                }
                else if (grid[j, i] == true)
                {
                    count++;
                }
            }
        }

        return count;
    }

    private bool SpotIsOffGrid(bool[,] grid, int x, int y)
    {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);

        return x < 0 || x >= width || y < 0 || y >= height;
    }

    private bool[,] GenerateGrid(int width, int height, float initialProbability)
    {
        bool[,] grid = new bool[height, width];

        for (int y = 0; y < height; y++)
        {

            for (int x = 0; x < width; x++)
            {
                bool value = (SeededRandom.Range(SeededRandom.Instance.MapRandom, 0.0f, 1.0f) < initialProbability) ? true : false;
                // bool value = (Random.Range(0.0f, 1.0f) < initialProbability) ? true : false;
                grid[y, x] = value;
            }
        }

        return grid;
    }

    // private void DrawGrid(List<List<bool>> grid, List<List<int>> caverns)
    private void DrawGrid(List<List<bool>> grid)
    {
        string a = "";
        foreach (List<bool> row in grid)
        {
            foreach (bool value in row)
            {
                a += value == true ? "X" : "0";
            }
            a += "\n";
        }
        Debug.Log(a);

        // string b = "";
        // foreach (List<int> row in caverns)
        // {
        //     foreach (int value in row)
        //     {
        //         b += $"{value}";
        //     }
        //     b += "\n";
        // }
        // Debug.Log(b);

        // for (int y = 0; y < grid.Count; y++)
        // {
        //     for (int x = 0; x < grid[y].Count; x++)
        //     {
        //         GameObject prefabToUse = grid[y][x] == true ? wallPrefab : floorPrefab;
        //         GameObject gameObject = GameObject.Instantiate(prefabToUse);
        //         gameObject.transform.position = new Vector3(x, y, 0);
        //         // if (caverns[y][x] > 0)
        //         // {
        //         //     gameObject.GetComponent<SpriteRenderer>().color = new Color((float)(0.1 * caverns[y][x]), (float)(0.1 * caverns[y][x]), (float)(0.1 * caverns[y][x]), 1f);
        //         // }
        //     }
        // }
    }

    private void FloodCaverns(ref int[,] cave, bool[,] grid, int x, int y, int fillNumber)
    {
        if (grid[y, x] == true || cave[y, x] != 0) { return; }

        cave[y, x] = fillNumber;

        if (x > 0)
        {
            FloodCaverns(ref cave, grid, x - 1, y, fillNumber);
        }
        if (x < grid.GetLength(1) - 1)
        {
            FloodCaverns(ref cave, grid, x + 1, y, fillNumber);
        }
        if (y > 0)
        {
            FloodCaverns(ref cave, grid, x, y - 1, fillNumber);
        }
        if (y < grid.GetLength(0) - 1)
        {
            FloodCaverns(ref cave, grid, x, y + 1, fillNumber);
        }
    }

    private int[,] IdentifyCaverns(bool[,] grid)
    {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);

        int[,] cave = new int[height, width];

        for (int y = 0; y < height; y++)
        {

            for (int x = 0; x < width; x++)
            {
                cave[y, x] = 0;
            }
        }

        int fillNumber = 1;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (cave[y, x] == 0 && grid[y, x] == false)
                {
                    FloodCaverns(ref cave, grid, x, y, fillNumber);
                    fillNumber++;
                }
            }
        }

        return cave;
    }

    // == WAVE FUNCTION COLLAPSE == //
    private void FillDungeon(bool[,] grid)
    {
        WaveFunctionCollapseMap waveFunctionCollapseMap = new(grid, tileRules);

        var done = false;
        while (!done)
        {
            if (waveFunctionCollapseMap.WaveFunctionCollapse() == false)
            {
                done = true;
            }
        }
        for (int y = 0; y < waveFunctionCollapseMap.tiles.GetLength(0); y++)
        {
            for (int x = 0; x < waveFunctionCollapseMap.tiles.GetLength(1); x++)
            {
                floor.SetTile(new Vector3Int(x, y, 1), tileRules.Rules[waveFunctionCollapseMap.GetType(x, y)].Tile);
            }
        }
    }

    // == CONVOLUTION == //
    private void Convolution(bool[,] grid)
    {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);

        TileType[,] structures = new TileType[height, width];

        for (int y = 0; y < height; y++)
        {

            for (int x = 0; x < width; x++)
            {
                if (grid[y, x] == false)
                {
                    structures[y, x] = TileType.FLOOR;
                }
                else
                {
                    structures[y, x] = TileType.WALL;
                }
            }

            // structures.Add(row);
        }

        foreach (ConvolutionRules rule in convolutionRules)
        {
            FillStructures(ref structures, rule);
        }


        Grid = structures;
    }

    private void GetPlayerGoalPositions(TileType[,] structures)
    {
        int height = structures.GetLength(0);
        int width = structures.GetLength(1);
        Vector2 corner = GetRandomCorner(width, height);
        Vector2 opposite = GetOppositeCorner(corner, width, height);

        PlayerSpawn = FindNearestEmpty(structures, corner);
        if (PlayerSpawn == Vector2.zero)
        {
            Main();
            return;
        }
        GoalSpawn = FindNearestEmpty(structures, opposite);
        if (GoalSpawn == Vector2.zero)
        {
            Main();
            return;
        }
        PlayerSpawn = new Vector2(PlayerSpawn.x + 0.5f, PlayerSpawn.y + 0.5f);
        GoalSpawn = new Vector2(GoalSpawn.x + 0.5f, GoalSpawn.y + 0.5f);
    }

    private Vector2 FindNearestEmpty(TileType[,] structures, Vector2 corner)
    {
        int height = structures.GetLength(0);
        int width = structures.GetLength(1);
        bool foundEmptyPlayerSpace = false;

        Queue<Vector2> queue = new();
        queue.Enqueue(corner);

        bool[,] checkedTiles = new bool[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                checkedTiles[y, x] = false;
            }
        }

        while (!foundEmptyPlayerSpace)
        {
            if (queue.Count > checkedTiles.Length)
            {
                foundEmptyPlayerSpace = true;
                return Vector2.zero;
            }
            Vector2 current = Vector2.zero;
            try
            {
                current = queue.Dequeue();
            }
            catch
            {
                foundEmptyPlayerSpace = true;
                return Vector2.zero;
            }

            if (structures[(int)current.y, (int)current.x] != TileType.FLOOR)
            {
                checkedTiles[(int)current.y, (int)current.x] = true;
                if ((int)current.y > 0 && !checkedTiles[(int)current.y - 1, (int)current.x])
                {
                    if (!queue.Contains(new Vector2((int)current.x, (int)current.y - 1)))
                    {
                        queue.Enqueue(new Vector2((int)current.x, (int)current.y - 1));
                    }
                }
                if ((int)current.x < width - 1 && !checkedTiles[(int)current.y, (int)current.x + 1])
                {
                    if (!queue.Contains(new Vector2((int)current.x + 1, (int)current.y)))
                    {
                        queue.Enqueue(new Vector2((int)current.x + 1, (int)current.y));
                    }
                }
                if ((int)current.y < height - 1 && !checkedTiles[(int)current.y + 1, (int)current.x])
                {
                    if (!queue.Contains(new Vector2((int)current.x, (int)current.y + 1)))
                    {
                        queue.Enqueue(new Vector2((int)current.x, (int)current.y + 1));
                    }
                }
                if ((int)current.x > 0 && !checkedTiles[(int)current.y, (int)current.x - 1])
                {
                    if (!queue.Contains(new Vector2((int)current.x - 1, (int)current.y)))
                    {
                        queue.Enqueue(new Vector2((int)current.x - 1, (int)current.y));
                    }
                }
            }
            else
            {
                foundEmptyPlayerSpace = true;
                return current;
            }
        }
        return Vector2.zero;
    }

    private Vector2 GetOppositeCorner(Vector2 corner, int width, int height)
    {
        return new Vector2(corner.x == 0 ? width - 1 : 0, corner.y == 0 ? height - 1 : 0);
    }

    private Vector2 GetRandomCorner(int width, int height)
    {
        return new Vector2(SeededRandom.Range(SeededRandom.Instance.MapRandom, 0, 2) == 1 ? 0 : width - 1, SeededRandom.Range(SeededRandom.Instance.MapRandom, 0, 2) == 1 ? 0 : height - 1);
        // return new Vector2(Random.Range(0, 2) == 1 ? 0 : width - 1, Random.Range(0, 2) == 1 ? 0 : height - 1);
    }

    private void FillStructures(ref TileType[,] structures, ConvolutionRules rule)
    {
        int height = structures.GetLength(0);
        int width = structures.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (checkMatrix(ref structures, rule, x, y))
                {
                    float randomFloat = SeededRandom.Range(SeededRandom.Instance.MapRandom, 0f, 1f);
                    // float randomFloat = Random.Range(0f, 1f);
                    if (randomFloat <= rule.chance)
                    {
                        FillStructure(ref structures, rule, x, y);
                    }
                }
            }
        }
    }

    private void FillStructure(ref TileType[,] structures, ConvolutionRules rule, int x, int y)
    {
        for (int y1 = 0; y1 < rule.TileOutput.Count(); y1++)
        {
            for (int x1 = 0; x1 < rule.TileOutput[y1].row.Count(); x1++)
            {
                if (rule.TileOutput[y1].row[x1].Type == TileType.EMPTY)
                {
                    continue;
                }
                Tilemap tilemap = rule.TileOutput[y1].row[x1].Type switch
                {
                    TileType.FLOOR => floor,
                    TileType.WALL => walls,
                    TileType.OBSTACLE => obstacles,
                    TileType.TRAP_VOID => traps,
                    TileType.TRAP_DAMAGE => traps,
                    TileType.TRAP_FLOOR => traps,
                    TileType.TRAP_CHEST => traps,
                    TileType.CHEST => traps,
                    TileType.ENEMY_SPAWN => enemies,
                    _ => traps
                };

                tilemap.SetTile(new Vector3Int(x + x1, y + y1, 0), rule.TileOutput[y1].row[x1].Tile);
                structures[y + y1, x + x1] = rule.TileOutput[y1].row[x1].Type;
            }
        }

        for (int y1 = 0; y1 < rule.GameObjectOutput.Count(); y1++)
        {
            for (int x1 = 0; x1 < rule.GameObjectOutput[y1].row.Count(); x1++)
            {
                if (rule.GameObjectOutput[y1].row[x1].gameObject != null)
                {
                    Tilemap tilemap = rule.GameObjectOutput[y1].row[x1].Type switch
                    {
                        TileType.FLOOR => floor,
                        TileType.WALL => walls,
                        TileType.OBSTACLE => obstacles,
                        TileType.TRAP_VOID => traps,
                        TileType.TRAP_DAMAGE => traps,
                        TileType.TRAP_FLOOR => traps,
                        TileType.TRAP_CHEST => traps,
                        TileType.CHEST => traps,
                        TileType.ENEMY_SPAWN => enemies,
                        _ => traps
                    };
                    // var obj = Instantiate(rule.GameObjectOutput[y1].row[x1].gameObject, new Vector3(x + x1 + 0.5f, y + y1 + 0.5f, 0), UnityEngine.Quaternion.identity);
                    var obj = Instantiate(rule.GameObjectOutput[y1].row[x1].gameObject, new Vector3(x + x1, y + y1, 0), UnityEngine.Quaternion.identity);

                    if (rule.GameObjectOutput[y1].row[x1].Type == TileType.EMPTY)
                    {
                        continue;
                    }
                    obj.transform.parent = tilemap.transform;
                    structures[y + y1, x + x1] = rule.GameObjectOutput[y1].row[x1].Type;
                }
            }
        }
    }

    private bool checkMatrix(ref TileType[,] structures, ConvolutionRules rule, int x, int y)
    {
        if (rule.Matrix.Count() + y >= structures.GetLength(0) || rule.Matrix[0].row.Count() + x >= structures.GetLength(1))
        {
            return false;
        }
        for (int y1 = 0; y1 < rule.Matrix.Count(); y1++)
        {
            for (int x1 = 0; x1 < rule.Matrix[y1].row.Count(); x1++)
            {
                if (rule.Matrix[y1].row[x1] != structures[y + y1, x + x1])
                {
                    return false;
                }
            }
        }
        return true;
    }

    internal void UpdateGridManager()
    {
        int width = Grid.GetLength(1);
        int height = Grid.GetLength(0);

        int offsetX = 0;
        int offsetY = 0;

        GameManager.Instance.gridManager.grid = new global::Grid(new Vector2Int(width, height), new Vector2Int(offsetX, offsetY));

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameManager.Instance.gridManager.grid.nodes[x + offsetX, y + offsetY].floorType = getNodeType(Grid[y, x]);
                EntityType? entityType = getEntityType(Grid[y, x]);
                if (entityType != null) GameManager.Instance.gridManager.grid.nodes[x + offsetX, y + offsetY].entitiesOnTile.Add((EntityType)entityType);
            }
        }

        GameManager.Instance.gridManager.grid.nodes[(int)PlayerSpawn.x, (int)PlayerSpawn.y].entitiesOnTile.Add(EntityType.PLAYER);
    }

    private FloorType getNodeType(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.FLOOR:
                return FloorType.FLOOR;
            case TileType.WALL:
                return FloorType.WALL;
            case TileType.OBSTACLE:
                return FloorType.OBSTACLE;
            case TileType.TRAP_VOID:
                return FloorType.VOID;
            case TileType.TRAP_FLOOR:
                return FloorType.TRAP_FLOOR;
            default:
                return FloorType.EMPTY;
        }
    }

    private EntityType? getEntityType(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.TRAP_DAMAGE:
                return EntityType.TRAP_DAMAGE;
            case TileType.TRAP_CHEST:
                return EntityType.TRAP_CHEST;
            case TileType.CHEST:
                return EntityType.CHEST;
            case TileType.ENEMY_SPAWN:
                return EntityType.ENEMY_SPAWN;
            default:
                return null;
        }
    }
}