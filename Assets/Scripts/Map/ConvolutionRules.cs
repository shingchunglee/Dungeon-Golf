
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "ConvolutionRules", menuName = "ConvolutionRules", order = 0)]
class ConvolutionRules : ScriptableObject
{
    public Row[] Matrix;

    public OutputRow[] Output;

    public float chance;

    [Serializable]
    public class Row
    {
        public TileType[] row;
    }

    [Serializable]
    public class OutputRow
    {
        public TileBaseRow[] row;
    }

    [Serializable]
    public class TileBaseRow
    {
        public TileBase Tile;
        public TileType Type;
    }
}