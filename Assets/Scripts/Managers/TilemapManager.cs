using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public static TilemapManager Instance;
    [SerializeField] Tilemap _tilemap;
    [SerializeField] public Collider2D tileMapPolygon;

    public static HashSet<Vector2> tilesPos = new();

    private void Awake()
    {
        GetAllTilesInTilemap();
        Instance = this;
    }

    void GetAllTilesInTilemap()
    {
        BoundsInt bounds = _tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                TileBase tile = _tilemap.GetTile(pos);
                if (tile != null)
                {
                    tilesPos.Add((Vector2Int)pos);
                }
            }
        }
    }
}
