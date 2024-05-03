using UnityEngine;

public class Grid
{
    public Vector2Int gridSize;
    public Vector2Int gridOrigin;
    public Node[,] nodes; 

    public Grid(Vector2Int _gridSize, Vector2Int _gridOrigin)
    {
        gridOrigin = _gridOrigin;
        gridSize = _gridSize;

        nodes = new Node[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                nodes[x, y] = new Node(new Vector2Int(x, y));
            }
        }
    }
}
