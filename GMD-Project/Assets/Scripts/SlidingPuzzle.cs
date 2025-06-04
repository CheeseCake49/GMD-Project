using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SlidingPuzzle : MonoBehaviour
{
    public Texture2D sourceImage;
    public GameObject tileUIPrefab;
    public GameObject emptyTilePrefab;
    public RectTransform puzzleContainer; // Assign your Grid Layout Group container
    public int gridSize = 3;

    private Vector2Int emptyTilePos;
    private Dictionary<Vector2Int, GameObject> tiles = new();

    void Start()
    {
        GenerateTiles();
        ShuffleTiles();
    }

    void GenerateTiles()
    {
        int tileWidth = sourceImage.width / gridSize;
        int tileHeight = sourceImage.height / gridSize;

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Vector2Int pos = new(x, y);
                if (x == gridSize - 1 && y == gridSize - 1)
                {
                    GameObject emptyTile = Instantiate(emptyTilePrefab, puzzleContainer);
                    emptyTile.name = "EmptyTile";
                    emptyTile.transform.SetSiblingIndex(gridSize * gridSize - 1);
                    emptyTilePos = new Vector2Int(gridSize - 1, gridSize - 1);
                    tiles[emptyTilePos] = emptyTile;
                    continue;
                }

                Texture2D tileTex = new Texture2D(tileWidth, tileHeight);
                tileTex.SetPixels(sourceImage.GetPixels(x * tileWidth, y * tileHeight, tileWidth, tileHeight));
                tileTex.Apply();

                Sprite tileSprite = Sprite.Create(tileTex, new Rect(0, 0, tileWidth, tileHeight), new Vector2(0.5f, 0.5f));
                GameObject tile = Instantiate(tileUIPrefab, puzzleContainer);
                tile.transform.SetSiblingIndex(y * gridSize + x);
                tile.GetComponent<Image>().sprite = tileSprite;

                Tile tileScript = tile.AddComponent<Tile>();
                tileScript.Init(pos, this);

                tiles[pos] = tile;
            }
        }
    }

    public void TryMoveTile(Vector2Int clickedPos)
    {
        if (!IsAdjacent(clickedPos, emptyTilePos)) return;
        
        GameObject clickedTile = tiles[clickedPos];
        GameObject emptyTile = tiles[emptyTilePos];

        // Swap sibling index (UI movement)
        int clickedIndex = clickedTile.transform.GetSiblingIndex();
        int emptyIndex = emptyTile.transform.GetSiblingIndex();

        clickedTile.transform.SetSiblingIndex(emptyIndex);
        emptyTile.transform.SetSiblingIndex(clickedIndex);

        // Swap logical positions
        tiles[emptyTilePos] = clickedTile;
        tiles[clickedPos] = emptyTile;

        if (clickedTile.TryGetComponent<Tile>(out var clickedTileScript))
        {
            clickedTileScript.UpdatePosition(emptyTilePos);
        }
        emptyTilePos = clickedPos;
        
        if (IsSolved())
            Debug.Log("🎉 Puzzle Solved!");
    }
    
    bool IsAdjacent(Vector2Int a, Vector2Int b)
    {
        return (Mathf.Abs(a.x - b.x) == 1 && a.y == b.y) ||
               (Mathf.Abs(a.y - b.y) == 1 && a.x == b.x);
    }

    bool IsSolved()
    {
        foreach (var kv in tiles)
        {
            if (kv.Value.TryGetComponent(out Tile tile))
            {
                if (kv.Key != tile.OriginalPosition)
                    return false;
            }
        }
        return emptyTilePos == new Vector2Int(gridSize - 1, gridSize - 1);
    }

    void ShuffleTiles()
    {
        System.Random rng = new();
        for (int i = 0; i < 100; i++)
        {
            var neighbors = GetAdjacentPositions(emptyTilePos);
            Vector2Int chosen = neighbors[rng.Next(neighbors.Count)];
            TryMoveTile(chosen);
        }
    }

    List<Vector2Int> GetAdjacentPositions(Vector2Int pos)
    {
        List<Vector2Int> adjacent = new();
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (var d in dirs)
        {
            Vector2Int check = pos + d;
            if (tiles.ContainsKey(check))
                adjacent.Add(check);
        }
        return adjacent;
    }
}
