using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Node
{
    public FloorType floorType = FloorType.EMPTY;
    public List<EntityType> entitiesOnTile = new List<EntityType>();

    public Vector2Int position;

    public Node Connection { get; private set; }
    public float G { get; private set; }
    public float H { get; private set; }
    public float F => G + H;

    public void SetConnection(Node node) => Connection = node;

    public void SetG(float g) => G = g;

    public void SetH(float h) => H = h;

    public Node(Vector2Int _position)
    {
        position = _position;
    }

    public bool IsWalkable()
    {
        if (floorType == FloorType.WALL ||
            floorType == FloorType.OBSTACLE ||
            floorType == FloorType.VOID)
        {
            return false;
        }

        if (entitiesOnTile.Contains(EntityType.ENEMY) ||
            entitiesOnTile.Contains(EntityType.CHEST) ||
            entitiesOnTile.Contains(EntityType.TRAP_CHEST))
        {
            return false;
        }

        return true;

    }
}