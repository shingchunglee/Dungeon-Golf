using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "MapTileRules", menuName = "MapTileRules", order = 0)]
public class MapTileRules : ScriptableObject
{
    public MapTileRule[] Rules;
}

[Serializable]
public struct MapTileRule
{
    public TileBase Tile;
    public DirectionRules DirectionRules;
    public int Weight;
}

[Serializable]
public class DirectionRules
{
    public DirectionRule[] Rules = new DirectionRule[4];

    [Serializable]
    public class DirectionRule
    {
        public Direction Direction;
        public int Tile;
        public Sprite sprite;
        public int Weight;
    }

    [Serializable]
    public enum Direction
    {
        NORTH = 0,
        EAST = 1,
        SOUTH = 2,
        WEST = 3
    }
}