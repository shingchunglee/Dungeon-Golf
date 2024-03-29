using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ProcedualGeneration : MonoBehaviour
{
    [SerializeField]
    GameObject wallPrefab;
    [SerializeField]
    GameObject floorPrefab;

    public void Main()
    {
        // (List<List<bool>> grid, List<List<int>> caverns) = Automata(100, 100, 0.45f, 5, 4, 10);
        // DrawGrid(grid, caverns);
        List<List<bool>> grid = Automata(100, 100, 0.45f, 5, 4, 10);

        DrawGrid(grid);
    }

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

        List<List<int>> caverns = IdentifyCaverns(grid);

        int fillNumber = GetLargestCavern(caverns);
        FillLoneCaverns(ref grid, caverns, fillNumber);

        // return (grid, caverns);
        return grid;
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
                bool value = (Random.Range(0.0f, 1.0f) < initialProbability) ? true : false;
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

        for (int y = 0; y < grid.Count; y++)
        {
            for (int x = 0; x < grid[y].Count; x++)
            {
                GameObject prefabToUse = grid[y][x] == true ? wallPrefab : floorPrefab;
                GameObject gameObject = GameObject.Instantiate(prefabToUse);
                gameObject.transform.position = new Vector3(x, y, 0);
                // if (caverns[y][x] > 0)
                // {
                //     gameObject.GetComponent<SpriteRenderer>().color = new Color((float)(0.1 * caverns[y][x]), (float)(0.1 * caverns[y][x]), (float)(0.1 * caverns[y][x]), 1f);
                // }
            }
        }
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
}

