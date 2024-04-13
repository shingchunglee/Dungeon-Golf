
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "ConvolutionRules", menuName = "ConvolutionRules", order = 0)]
class ConvolutionRules : ScriptableObject
{
    public Row[] Matrix;

    public TileOutputRow[] TileOutput;

    public GameObjectOutputRow[] GameObjectOutput;

    public float chance;

    [Serializable]
    public class Row
    {
        public TileType[] row;
    }

    [Serializable]
    public class TileOutputRow
    {
        public TileBaseRow[] row;
    }

    [Serializable]
    public class TileBaseRow
    {
        public TileBase Tile;
        public TileType Type;
    }

    [Serializable]
    public class GameObjectOutputRow
    {
        public GameObjectRow[] row;
    }

    [Serializable]
    public class GameObjectRow
    {
        public GameObject gameObject;
        public TileType Type;
    }
}