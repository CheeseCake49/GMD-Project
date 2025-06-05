---
title: "Blog 5 Developer Post"
author: "Johan & Simone"
date: "20-05-2024"
version: "0.1"
---

# Game Development Progress Report

## Introduction

## TO-DO
- [X] Puzzle Slide
- [X] Puzzle Computer
- [X] Lighting
- [X] Create a menu

## Progress Overview
Our main goals for this blog has been:

## Puzzle Slide

One of our core interactive elements in this development sprint was implementing a sliding puzzle minigame. The goal was to allow players to interact with a frame in the environment and be presented with a 3x3 grid-based image puzzle that they could solve by rearranging tiles.

To achieve this, we built a modular system using Unity UI prefabs and custom scripts:

- `Puzzle.cs` handles interaction logic. When the player presses `E` near the puzzle frame, it disables player movement and the camera, makes the cursor visible, and activates the puzzle UI canvas.
- `SlidingPuzzle.cs` dynamically cuts a `Texture2D` image into smaller tile-sized sprites and assigns them to UI button tiles.
- The empty tile is tracked using a `Vector2Int` position, and all movements are handled by swapping sibling indices inside the grid container.
- Every tile is a button. When clicked, it checks whether it's adjacent to the empty space and performs a swap if so.

Script:

```csharp
public void TryMoveTile(Vector2Int clickedPos)
{
    if (!IsAdjacent(clickedPos, emptyTilePos)) return;

    GameObject clickedTile = tiles[clickedPos];
    GameObject emptyTile = tiles[emptyTilePos];

    int clickedIndex = clickedTile.transform.GetSiblingIndex();
    int emptyIndex = emptyTile.transform.GetSiblingIndex();

    clickedTile.transform.SetSiblingIndex(emptyIndex);
    emptyTile.transform.SetSiblingIndex(clickedIndex);

    tiles[emptyTilePos] = clickedTile;
    tiles[clickedPos] = emptyTile;

    if (clickedTile.TryGetComponent<Tile>(out var tileScript))
        tileScript.UpdatePosition(emptyTilePos);

    emptyTilePos = clickedPos;

    if (IsSolved())
        Debug.Log("Puzzle Solved!");
}
```
We also built a custom Tile class for each puzzle piece. This stores its original position and calls the puzzle logic when clicked:
```csharp
public void Init(Vector2Int pos, SlidingPuzzle puzzle)
{
    Position = pos;
    OriginalPosition = pos;
    this.puzzle = puzzle;
    GetComponent<Button>().onClick.AddListener(OnClick);
}
```

##Puzzle Computer

For the final puzzle interaction in the third scene, we created a computer interface that allows the player to trigger a simulated system deletion. This interaction marks the narrative climax where the player must choose to terminate or escape the simulated world. (It should be said that we didn't achive this 100%) Our first thoughts was that to delete the user you would need a password, there was to find in the world. But we unfortunately did not have time to implement this part.  

We structured this feature using two main scripts:

- `FinalScreenActivator.cs` is a simple `IInteractable` script. When the player presses `E` near the monitor, it calls `OpenMenu()` on the UI manager.
- `FinalScreenMenu.cs` manages the UI canvas and button behavior. When the menu opens, it activates the canvas. If the player clicks the button, a coroutine begins that shows a "Deleting..." message for five seconds before simulating a system crash by quitting the application.

```csharp
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FinalScreenMenu : MonoBehaviour
{

    [SerializeField] private Canvas canvasUI;
    [SerializeField] private Canvas deletingProgress;
    [SerializeField] private Button deleteButton;

    void Start()
    {
        canvasUI.gameObject.SetActive(false);
        deletingProgress.gameObject.SetActive(false);
    }
    
    public void OpenMenu()
    {
        canvasUI.gameObject.SetActive(true);
        deletingProgress.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(deleteButton.gameObject);
    }

    public void DeleteFile()
    {
        StartCoroutine(CrashGame());
    }

    private IEnumerator CrashGame()
    {
        deletingProgress.gameObject.SetActive(true);

        yield return new WaitForSeconds(5);
        
        Application.Quit();
        Debug.Log("Quitting Game");
    }

}
```


##Menu

