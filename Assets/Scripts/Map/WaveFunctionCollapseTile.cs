using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveFunctionCollapseTile
{
    public List<int> Possibilities;
    public int Entropy;
    public Dictionary<DirectionRules.Direction, WaveFunctionCollapseTile> Neighbours;
    public MapTileRules tileRules;

    public WaveFunctionCollapseTile(MapTileRules tileRules)
    {
        Possibilities = new List<int>();
        for (int i = 0; i < tileRules.Rules.Length; i++)
        {
            Possibilities.Add(i);
        }
        Entropy = Possibilities.Count;
        Neighbours = new Dictionary<DirectionRules.Direction, WaveFunctionCollapseTile>();
        this.tileRules = tileRules;
    }

    public void AddNeighbour(DirectionRules.Direction direction, WaveFunctionCollapseTile tile)
    {
        Neighbours[direction] = tile;
    }

    public WaveFunctionCollapseTile GetNeighbour(DirectionRules.Direction direction)
    {
        return Neighbours[direction];
    }

    public List<DirectionRules.Direction> GetDirections()
    {
        List<DirectionRules.Direction> directions = new();
        Neighbours.Keys.ToList().ForEach(x => directions.Add(x));
        return directions;
    }

    public List<int> GetPossibilities()
    {
        return Possibilities;
    }

    public void Collapse()
    {
        List<int> weights = new();
        foreach (var i in Possibilities)
        {
            weights.Add(tileRules.Rules[i].Weight);
        }
        int totalWeight = weights.Sum();
        int randomInt = SeededRandom.Range(SeededRandom.Instance.MapRandom, 0, totalWeight);
        // int randomInt = Random.Range(0, totalWeight);
        foreach (var i in Possibilities)
        {
            if (randomInt < tileRules.Rules[i].Weight)
            {
                Possibilities = new()
                {
                    i
                };
                break;
            }
            randomInt -= tileRules.Rules[i].Weight;
        }
        Entropy = 0;
    }

    public bool Constrain(List<int> neighbourPossibilities, DirectionRules.Direction direction)
    {
        bool reduced = false;

        if (Entropy > 0)
        {
            List<int> connectors = new();
            for (int i = 0; i < neighbourPossibilities.Count; i++)
            {
                connectors.Add(tileRules.Rules[neighbourPossibilities[i]].DirectionRules.Rules[(int)direction].Tile);
            }

            DirectionRules.Direction Opposite = DirectionRules.Direction.NORTH;
            if (direction == DirectionRules.Direction.NORTH) Opposite = DirectionRules.Direction.SOUTH;
            if (direction == DirectionRules.Direction.EAST) Opposite = DirectionRules.Direction.WEST;
            if (direction == DirectionRules.Direction.SOUTH) Opposite = DirectionRules.Direction.NORTH;
            if (direction == DirectionRules.Direction.WEST) Opposite = DirectionRules.Direction.EAST;

            for (int i = Possibilities.Count - 1; i >= 0; i--)
            {
                int oppositeTile = tileRules.Rules[Possibilities[i]].DirectionRules.Rules.First(x => x.Direction == Opposite).Tile;
                if (!connectors.Contains(oppositeTile))
                {
                    Possibilities.RemoveAt(i);
                    reduced = true;
                }
            }

            Entropy = Possibilities.Count;
        }

        return reduced;
    }
}