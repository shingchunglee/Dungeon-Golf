using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionCollapseMap
{
    public List<List<bool>> grid;
    public List<List<WaveFunctionCollapseTile>> tiles;
    public MapTileRules tileRules;
    public WaveFunctionCollapseMap(List<List<bool>> grid, MapTileRules tileRules)
    {
        this.tileRules = tileRules;
        this.grid = grid;
        tiles = new();

        int height = grid.Count;
        int width = grid[0].Count;

        for (int y = 0; y < height; y++)
        {
            List<WaveFunctionCollapseTile> row = new();
            for (int x = 0; x < width; x++)
            {
                if (grid[y][x] == false)
                {
                    row.Add(new WaveFunctionCollapseTile(tileRules));
                }
                else
                {
                    WaveFunctionCollapseTile wallTile = new(tileRules)
                    {
                        Possibilities = new List<int>(){
                            0
                        },
                        Entropy = 0
                    };
                    row.Add(wallTile);
                }
            }

            tiles.Add(row);
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var tile = tiles[y][x];

                if (y > 0)
                {
                    tile.AddNeighbour(DirectionRules.Direction.SOUTH, tiles[y - 1][x]);
                }
                if (x < width - 1)
                {
                    tile.AddNeighbour(DirectionRules.Direction.EAST, tiles[y][x + 1]);
                }
                if (y < height - 1)
                {
                    tile.AddNeighbour(DirectionRules.Direction.NORTH, tiles[y + 1][x]);
                }
                if (x > 0)
                {
                    tile.AddNeighbour(DirectionRules.Direction.WEST, tiles[y][x - 1]);
                }
            }
        }
    }

    public int GetEntropy(int x, int y)
    {
        return tiles[y][x].Entropy;
    }

    public int GetType(int x, int y)
    {
        return tiles[y][x].Possibilities[0];
    }

    public int GetLowestEntropy()
    {
        int lowestEntropy = tileRules.Rules.Length;

        for (int y = 0; y < grid.Count; y++)
        {
            for (int x = 0; x < grid[0].Count; x++)
            {
                int tileEntropy = tiles[y][x].Entropy;
                if (tileEntropy > 0 && tileEntropy < lowestEntropy)
                {
                    lowestEntropy = tileEntropy;
                }
            }
        }
        return lowestEntropy;
    }

    public List<WaveFunctionCollapseTile> GetTilesLowestEntropy()
    {
        int lowestEntropy = tileRules.Rules.Length;
        List<WaveFunctionCollapseTile> tileList = new();

        for (int y = 0; y < grid.Count; y++)
        {
            for (int x = 0; x < grid[0].Count; x++)
            {
                int tileEntropy = tiles[y][x].Entropy;
                if (tileEntropy > 0 && tileEntropy < lowestEntropy)
                {
                    tileList.Clear();
                    lowestEntropy = tileEntropy;
                }
                if (tileEntropy == lowestEntropy)
                {
                    tileList.Add(tiles[y][x]);
                }
            }
        }
        return tileList;
    }

    public bool WaveFunctionCollapse()
    {
        List<WaveFunctionCollapseTile> tilesLowestEntropy = GetTilesLowestEntropy();

        if (tilesLowestEntropy.Count == 0)
        {
            return false;
        }

        WaveFunctionCollapseTile tileToCollapse = tilesLowestEntropy[Random.Range(0, tilesLowestEntropy.Count)];
        tileToCollapse.Collapse();

        Stack<WaveFunctionCollapseTile> stack = new();
        stack.Push(tileToCollapse);

        while (stack.Count > 0)
        {
            WaveFunctionCollapseTile tile = stack.Pop();
            List<int> tilePossibilities = tile.GetPossibilities();
            List<DirectionRules.Direction> directions = tile.GetDirections();

            foreach (DirectionRules.Direction direction in directions)
            {
                WaveFunctionCollapseTile neighbour = tile.GetNeighbour(direction);
                if (neighbour.Entropy != 0)
                {
                    bool reduced = neighbour.Constrain(tilePossibilities, direction);

                    if (reduced)
                    {
                        stack.Push(neighbour);
                    }
                }
            }
        }

        return true;
    }
}