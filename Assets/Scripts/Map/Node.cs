using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Node
{
    public FloorType floorType = FloorType.EMPTY;
    public List<EntityType> entitiesOnTile = new List<EntityType>();

    public Node Connection { get; private set; }
    public float G { get; private set; }
    public float H { get; private set; }
    public float F => G + H;

    public void SetConnection(Node node) => Connection = node;

    public void SetG(float g) => G = g;

    public void SetH(float h) => H = h;
}