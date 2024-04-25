using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public FloorType floorType = FloorType.EMPTY;
    public List<EntityType> entitiesOnTile = new List<EntityType>();

    public Grid grid => GameManager.Instance.gridManager.grid;

    public Vector2Int position;

    public Node Connection { get; private set; }

    //The distance from the node to the start node
    public float G { get; private set; }
    //Heuristic Cost, the direct cost to the goal in spaces ignoring all obstacles
    public float H { get; private set; }
    public float F => G + H;

    private List<Node> neighbours;
    public List<Node> Neighbors
    {
        get
        {
            if (neighbours == null)
            {
                neighbours = GetNeighborNodes();
                return neighbours;
            }
            else
            {
                return neighbours;
            }
        }
        private set
        {
            neighbours = value;
        }
    }

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

    public int GetDistanceToOtherNode(Node other)
    {
        var thisNodeValue = this.position.x + this.position.y;
        var otherNodeValue = other.position.x + other.position.y;

        return Mathf.Abs(thisNodeValue - otherNodeValue);
    }

    private List<Node> GetNeighborNodes()
    {
        List<Node> neighbours = new List<Node>();

        // This could be made more efficient

        var node1 = CheckDirectionForNode(false, this, 0, 1);
        if (node1 != null) neighbours.Add(node1);

        var node2 = CheckDirectionForNode(false, this, 1, 0);
        if (node2 != null) neighbours.Add(node2);

        var node3 = CheckDirectionForNode(false, this, -1, 0);
        if (node3 != null) neighbours.Add(node3);

        var node4 = CheckDirectionForNode(false, this, 0, -1);
        if (node4 != null) neighbours.Add(node4);


        return neighbours;
    }

    public List<Node> GetWalkableNeighborNodes()
    {
        List<Node> neighbours = new List<Node>();

        // This could be made more efficient
        var node1 = CheckDirectionForNode(true, this, 0, 1);
        if (node1 != null) neighbours.Add(node1);

        var node2 = CheckDirectionForNode(true, this, 1, 0);
        if (node2 != null) neighbours.Add(node2);

        var node3 = CheckDirectionForNode(true, this, -1, 0);
        if (node3 != null) neighbours.Add(node3);

        var node4 = CheckDirectionForNode(true, this, 0, -1);
        if (node4 != null) neighbours.Add(node4);

        return neighbours;
    }

# nullable enable

    private Node? CheckDirectionForNode(bool isWalkableCheck, Node node, int xDir, int yDir)
    {
        int targetX = node.position.x + xDir;
        int targetY = node.position.y + yDir;

        //Make sure that the index is within the grid
        if (targetX >= 0 && targetX < grid.nodes.GetLength(0) &&
            targetY >= 0 && targetY < grid.nodes.GetLength(1))
        {
            var nodeNeighbour = grid.nodes[targetX, targetY];

            if (!isWalkableCheck)
            {
                return nodeNeighbour;
            }

            if (nodeNeighbour.IsWalkable())
            {
                return nodeNeighbour;
            }
            else return null;
        }
        else
        {
            Debug.Log("Direction couldn't be found. Maybe it's outside of grid limit.");
            return null;
        }
    }
}