using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Vector2Int Position { get; set; }
    public Vector2Int OriginalPosition { get; private set; }

    private SlidingPuzzle puzzle;

    public void Init(Vector2Int pos, SlidingPuzzle puzzle)
    {
        Position = pos;
        OriginalPosition = pos;
        this.puzzle = puzzle;

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void UpdatePosition(Vector2Int newPos)
    {
        Position = newPos;
    }
    
    void OnClick()
    {
        puzzle.TryMoveTile(Position);
    }
}