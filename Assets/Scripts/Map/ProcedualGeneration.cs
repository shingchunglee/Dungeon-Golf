using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
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
    public List<List<TileType>> Grid { private set; get; }

    [SerializeField]
    public int Width = 50;
    [SerializeField]
    public int Height = 50;

    public void Main()
    {
        // * HAVE A PUBLIC BOOLEAN CALLED ReadFromGrid,
        // * IF TRUE, READ FROM GRID
        // * ELSE DO THE GENERATION BELOW

        List<List<bool>> grid = Automata(Width, Height, 0.45f, 5, 4, 10);


        // (List<List<bool>> grid, List<List<int>> caverns) = Automata(100, 100, 0.45f, 5, 4, 10);
        // DrawGrid(grid, caverns);

        // DrawGrid(grid);

        FillDungeon(grid);
        Convolution(grid);

        RenderTiles(grid);
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

    private void RenderTiles(List<List<bool>> grid)
    {
        // RenderWalls(grid);
        RenderWalls(grid);
    }

    private void RenderWalls(List<List<bool>> grid)
    {
        walls.ClearAllTiles();

        int height = grid.Count;
        int width = grid[0].Count;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[y][x] == true)
                {
                    walls.SetTile(new Vector3Int(x, y, 0), voidTile);
                }
            }
        }
    }

    // == CELLULAR AUTOMATA == //

    private List<List<bool>> AutomataIteration(List<List<bool>> grid, int birthThreshold, int survivalThreshold)
    {
        int height = grid.Count;
        int width = grid[0].Count;
        List<List<bool>> newGrid = new List<List<bool>>();

        for (int y = 0; y < height; y++)
        {
            List<bool> row = new List<bool>();

            for (int x = 0; x < width; x++)
            {
                row.Add(false);
            }

            newGrid.Add(row);
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                bool newValue = NewValueAtPosition(grid, x, y, birthThreshold, survivalThreshold);
                newGrid[y][x] = newValue;
            }
        }

        return newGrid;
    }

    // private (List<List<bool>>, List<List<int>>) Automata(int width, int height, float initialProbability, int birthThreshold, int survivalThreshold, int iterations)
    private List<List<bool>> Automata(int width, int height, float initialProbability, int birthThreshold, int survivalThreshold, int iterations)
    {
        List<List<bool>> grid = GenerateGrid(width, height, initialProbability);

        for (int i = 0; i < iterations; i++)
        {
            grid = AutomataIteration(grid, birthThreshold, survivalThreshold);
        }
        FillBorders(ref grid);

        List<List<int>> caverns = IdentifyCaverns(grid);

        int fillNumber = GetLargestCavern(caverns);
        FillLoneCaverns(ref grid, caverns, fillNumber);

        // return (grid, caverns);
        return grid;
    }

    private void FillBorders(ref List<List<bool>> grid)
    {
        int height = grid.Count;
        int width = grid[0].Count;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    grid[y][x] = true;
                }
            }
        }
    }

    private void FillLoneCaverns(ref List<List<bool>> grid, List<List<int>> caverns, int fillNumber)
    {
        int height = grid.Count;
        int width = grid[0].Count;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (caverns[y][x] != fillNumber)
                {
                    grid[y][x] = true;
                }
            }
        }
    }

    private int GetLargestCavern(List<List<int>> caverns)
    {
        var dict = new Dictionary<int, int>();
        foreach (List<int> cavern in caverns)
        {
            foreach (int fillNumber in cavern)
            {
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

    private bool NewValueAtPosition(List<List<bool>> grid, int x, int y, int birthThreshold, int survivalThreshold)
    {
        bool cellIsAlive = grid[y][x] == true;
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

    private int CountAliveNeighbors(List<List<bool>> grid, int x, int y)
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
                else if (grid[j][i] == true)
                {
                    count++;
                }
            }
        }

        return count;
    }

    private bool SpotIsOffGrid(List<List<bool>> grid, int x, int y)
    {
        int height = grid.Count;
        int width = grid[0].Count;

        return x < 0 || x >= width || y < 0 || y >= height;
    }

    private List<List<bool>> GenerateGrid(int width, int height, float initialProbability)
    {
        List<List<bool>> grid = new List<List<bool>>();

        for (int y = 0; y < height; y++)
        {
            List<bool> row = new List<bool>();

            for (int x = 0; x < width; x++)
            {
                bool value = (SeededRandom.Range(SeededRandom.Instance.MapRandom, 0.0f, 1.0f) < initialProbability) ? true : false;
                // bool value = (Random.Range(0.0f, 1.0f) < initialProbability) ? true : false;
                row.Add(value);
            }

            grid.Add(row);
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

    private void FloodCaverns(ref List<List<int>> cave, List<List<bool>> grid, int x, int y, int fillNumber)
    {
        if (grid[y][x] == true || cave[y][x] != 0) { return; }

        cave[y][x] = fillNumber;

        if (x > 0)
        {
            FloodCaverns(ref cave, grid, x - 1, y, fillNumber);
        }
        if (x < grid[0].Count - 1)
        {
            FloodCaverns(ref cave, grid, x + 1, y, fillNumber);
        }
        if (y > 0)
        {
            FloodCaverns(ref cave, grid, x, y - 1, fillNumber);
        }
        if (y < grid.Count - 1)
        {
            FloodCaverns(ref cave, grid, x, y + 1, fillNumber);
        }
    }

    private List<List<int>> IdentifyCaverns(List<List<bool>> grid)
    {
        int height = grid.Count;
        int width = grid[0].Count;

        List<List<int>> cave = new List<List<int>>();

        for (int y = 0; y < height; y++)
        {
            List<int> row = new List<int>();

            for (int x = 0; x < width; x++)
            {
                row.Add(0);
            }

            cave.Add(row);
        }

        int fillNumber = 1;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (cave[y][x] == 0 && grid[y][x] == false)
                {
                    FloodCaverns(ref cave, grid, x, y, fillNumber);
                    fillNumber++;
                }
            }
        }

        return cave;
    }

    // == WAVE FUNCTION COLLAPSE == //
    private void FillDungeon(List<List<bool>> grid)
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
        for (int y = 0; y < waveFunctionCollapseMap.tiles.Count; y++)
        {
            for (int x = 0; x < waveFunctionCollapseMap.tiles[y].Count; x++)
            {
                floor.SetTile(new Vector3Int(x, y, 1), tileRules.Rules[waveFunctionCollapseMap.GetType(x, y)].Tile);
            }
        }
        // List<List<int>> map;
        // FillObstacles(grid);
        // FillHazards(grid);
        // FillChests(grid);
        // GetPlayerSpawn(grid);
        // GetGoalSpawn(grid);
    }

    private void FillHazards(List<List<bool>> grid)
    {
        throw new System.NotImplementedException();
    }

    private void FillObstacles(List<List<bool>> grid)
    {
        // CheckEntropy(grid, map)
    }

    // == CONVOLUTION == //
    private void Convolution(List<List<bool>> grid)
    {
        int height = grid.Count;
        int width = grid[0].Count;

        List<List<TileType>> structures = new List<List<TileType>>();

        for (int y = 0; y < height; y++)
        {
            List<TileType> row = new List<TileType>();

            for (int x = 0; x < width; x++)
            {
                if (grid[y][x] == false)
                {
                    row.Add(TileType.FLOOR);

                }
                else
                {
                    row.Add(TileType.WALL);
                }
            }

            structures.Add(row);
        }

        foreach (ConvolutionRules rule in convolutionRules)
        {
            FillStructures(ref structures, rule);
        }

        GetPlayerGoalPositions(ref structures);

        Grid = structures;

        string str = "";
        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                str += (int)structures[y][x] + " ";
            }
            str += "\n";
        }
        Debug.Log(str);
    }

    private void GetPlayerGoalPositions(ref List<List<TileType>> structures)
    {
        int height = structures.Count;
        int width = structures[0].Count;
        Vector2 corner = GetRandomCorner(width, height);
        Vector2 opposite = GetOppositeCorner(corner, width, height);

        PlayerSpawn = FindNearestEmpty(structures, corner);
        GoalSpawn = FindNearestEmpty(structures, opposite);
    }

    private Vector2 FindNearestEmpty(List<List<TileType>> structures, Vector2 corner)
    {
        int height = structures.Count;
        int width = structures[0].Count;
        bool foundEmptyPlayerSpace = false;

        Queue<Vector2> queue = new();
        queue.Enqueue(corner);

        List<Vector2> checkedTiles = new();

        while (!foundEmptyPlayerSpace)
        {
            Vector2 current = queue.Dequeue();
            if (structures[(int)current.y][(int)current.x] != TileType.FLOOR)
            {
                checkedTiles.Add(current);
                if ((int)current.y > 0 && !checkedTiles.Contains(new Vector2((int)current.x, (int)current.y - 1)))
                {
                    queue.Enqueue(new Vector2((int)current.x, (int)current.y - 1));
                }
                if ((int)current.x < width - 1 && !checkedTiles.Contains(new Vector2((int)current.x + 1, (int)current.y)))
                {
                    queue.Enqueue(new Vector2((int)current.x + 1, (int)current.y));
                }
                if ((int)current.y < height - 1 && !checkedTiles.Contains(new Vector2((int)current.x, (int)current.y + 1)))
                {
                    queue.Enqueue(new Vector2((int)current.x, (int)current.y + 1));
                }
                if ((int)current.x > 0 && !checkedTiles.Contains(new Vector2((int)current.x - 1, (int)current.y)))
                {
                    queue.Enqueue(new Vector2((int)current.x - 1, (int)current.y));
                }
            }
            else
            {
                foundEmptyPlayerSpace = true;
                return current;
            }
            // return Vector2.zero;
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

    private void FillStructures(ref List<List<TileType>> structures, ConvolutionRules rule)
    {
        int height = structures.Count;
        int width = structures[0].Count;

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
                        FillStructure(structures, rule, x, y);
                    }
                }
            }
        }
    }

    private void FillStructure(List<List<TileType>> structures, ConvolutionRules rule, int x, int y)
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
                structures[y + y1][x + x1] = rule.TileOutput[y1].row[x1].Type;
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
                    var obj = Instantiate(rule.GameObjectOutput[y1].row[x1].gameObject, new UnityEngine.Vector3(x + x1, y + y1, 0), UnityEngine.Quaternion.identity);

                    if (rule.GameObjectOutput[y1].row[x1].Type == TileType.EMPTY)
                    {
                        continue;
                    }
                    obj.transform.parent = tilemap.transform;
                    structures[y + y1][x + x1] = rule.GameObjectOutput[y1].row[x1].Type;
                }
            }
        }
    }

    private bool checkMatrix(ref List<List<TileType>> structures, ConvolutionRules rule, int x, int y)
    {
        if (rule.Matrix.Count() + y >= structures.Count || rule.Matrix[0].row.Count() + x >= structures[0].Count)
        {
            return false;
        }
        for (int y1 = 0; y1 < rule.Matrix.Count(); y1++)
        {
            for (int x1 = 0; x1 < rule.Matrix[y1].row.Count(); x1++)
            {
                if (rule.Matrix[y1].row[x1] != structures[y + y1][x + x1])
                {
                    return false;
                }
            }
        }
        return true;
    }
}

public enum TileType
{
    EMPTY,
    WALL,
    FLOOR,
    OBSTACLE,
    TRAP_DAMAGE,
    TRAP_VOID,
    TRAP_FLOOR,
    TRAP_CHEST,
    CHEST,
    ENEMY_SPAWN,
}