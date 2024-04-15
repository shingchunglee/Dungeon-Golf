using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ScanPremadeMap : MonoBehaviour
{
    public bool ReadFromGrid = true;

    public List<Tilemap> tilemaps;
    public TileType[,] tileTypesGrid;

    private void Start()
    {
        if (ReadFromGrid)
        {
            int sizeX = 0;
            int sizeY = 0;

            // Note that a tilemap is always at least measured from [0,0], even if the first tile is at [1000,1000].
            // The origin can go below this if the tile map has tiles below [0,0].
            int originX = 0;
            int originY = 0;

            foreach (var tilemap in tilemaps)
            {
                BoundsInt gridBounds = tilemap.cellBounds;

                if (gridBounds.size.x > sizeX)
                {
                    sizeX = gridBounds.size.x;
                }
                if (gridBounds.size.y > sizeY)
                {
                    sizeY = gridBounds.size.y;
                }

                if (gridBounds.x < originX)
                {
                    originX = gridBounds.x;
                }
                if (gridBounds.y < originY)
                {
                    originY = gridBounds.y;
                }
            }

            tileTypesGrid = new TileType[sizeX, sizeY];

            Debug.Log($"Largest size tilemap is [{sizeX},{sizeY}].");

            foreach (var tilemap in tilemaps)
            {
                BoundsInt bounds = tilemap.cellBounds;
                TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

                Debug.Log($"Map: {tilemap.gameObject.name} =====================================================================");
                Debug.Log($"Map: {tilemap.gameObject.name} | Origin: [{bounds.x},{bounds.y}], Size: [{bounds.size.x}, {bounds.size.y}]");
                Debug.Log($"==============================================================================");

                for (int x = 0; x < bounds.size.x; x++)
                {
                    for (int y = 0; y < bounds.size.y; y++)
                    {
                        TileBase tile = allTiles[x + y * bounds.size.x];

                        int offsetX = 0;
                        int offsetY = 0;

                        if (bounds.x != originX)
                        {

                        }

                        if (tile != null)
                        {
                            Debug.Log($"Map: {tilemap.gameObject.name}, Relative: [{x},{y}], Grid: [{x + offsetX},{y + offsetY}] tile: {tile.name}");
                        }
                        else
                        {
                            Debug.Log($"Map: {tilemap.gameObject.name}, Relative: [{x},{y}], Grid: [{x + offsetX},{y + offsetY}] tile: null");

                        }
                    }
                }
            }



        }
    }
}
