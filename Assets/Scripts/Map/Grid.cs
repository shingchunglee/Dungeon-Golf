using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Vector2Int gridSize;
    public Node[,] nodes;

    public Grid(Vector2Int _gridSize)
    {
        gridSize = _gridSize;

        nodes = new Node[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                nodes[x, y] = new Node();
            }
        }
    }
}
