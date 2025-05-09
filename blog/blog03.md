---
title: "Blog 3 Developer Post"
author: "Johan & Simone"
date: "02-04-2024"
version: "0.1"
---

# Game Development Progress Report

## Introduction
These past weeks, We focused on the environment and visual elements for the first scene in our game. Most of the work revolved around learning-by-doing figuring out how to build spaces that feel immersive and are good-looking, even at this early stage. Between creating the first scene and writing the core scripts, we focused on building the fundamental interactivity of the game. This included player movement, camera control, and door interaction—laying the groundwork for how the player experiences the world. Now, doors can be approached and opened with a button press, and the camera smoothly follows player input, making navigation feel responsive. It’s a big step in turning a blank scene into something that evntually will turn out as a game.

## TO-DO
- [ ] Make the first scene(including interior)
- [X] Movement and camera
- [ ] Open close door + Loop
- [ ] Interact on E
- [ ] Begin writing down what kind of game objects we want in the game (puzzles)

## Progress Overview
Our main goals for this blog has been:

- Integrate architectural assets (walls, doors, windows).
- Get the player movement working properly with physical boundaries.
- Start experimenting with lighting and material styles.
- creating a floor plan for how the scenes should look

## Building the Scene

After importing a few furniture/exterior/interior assets, We started assembling/planning the interior of what will be the main scene: a looping hallway/house. 


## Code
###PlayerController
We began by implementing the characte controller. This script handles mouse/look input using Unity’s Input System, locks the cursor, and rotates both the camera and player body:

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Sensitivity")]
    [SerializeField] private float sensX = 100f;
    [SerializeField] private float sensY = 100f;

    [Header("References")]
    [SerializeField] private Camera camera;
    [SerializeField] private Transform orientation;

    private float xRotation;
    private float yRotation;
    
    void Start()
    {
        // Lock and hide the cursor for FPS control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        // Clamp vertical look to avoid flipping
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotations: camera pitches, body yaws
        camera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation    = Quaternion.Euler(0,           yRotation, 0);
    }

    // Called by the Input System’s “Look” action
    void OnLook(InputValue value)
    {
        Vector2 camMovement = value.Get<Vector2>();
        yRotation += camMovement.x * Time.deltaTime * sensX;
        xRotation -= camMovement.y * Time.deltaTime * sensY;
    }
}





