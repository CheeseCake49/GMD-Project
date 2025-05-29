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
- [X] Open close door 
- [X] Interact on E
- [X] Begin writing down what kind of game objects we want in the game (puzzles)
- [ ] See if collision detection can be made more smooth to avoid snapping back and forth when walking into a wall
- [ ] Find better solution for closing letter

## Progress Overview
Our main goals for this blog has been:

- Integrate architectural assets (walls, doors, windows).
- Get the player movement working properly with physical boundaries.
- Start experimenting with lighting and material styles.
- creating a floor plan for how the scenes should look

## Building the Scene

After importing a few furniture/exterior/interior assets, We started assembling/planning the interior of what will be the main scene: a looping hallway/house. 


## Code
### PlayerController
We began by implementing the character controller. This script handles mouse/look input using Unity’s Input System, locks the cursor, and rotates both the camera and player body:

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
```
- sensX/Y control horizontal and vertical look speed.
- camera is the player’s eyes; orientation is the body transform for movement direction.
- OnLook() reads pointer delta and accumulates rotations each frame.

### Playerinteraction

We built a interaction system that lets the player open doors, read letters and more by simply looking at them and pressing E. It’s handled through a mix of trigger zones and raycasting.

The script is attached to the player's camera and continuously checks whether the player is looking at something interactable. If so, a UI prompt appears to indicate the player can interact. When the player presses E, it either opens a nearby door or shows the content of a letter, depending on what they’re looking at.

Here’s the full script:

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance;
    public GameObject interactionText;
    public LayerMask interactionLayers;
    private DoorInteraction nearbyDoor;

    // This is called by the Input System when "Interact" (E key) is pressed
    void OnInteract()
    {
        Debug.Log("Interact Pressed");

        if (nearbyDoor != null)
        {
            nearbyDoor.ToggleDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered trigger with " + other.name);

        DoorInteraction door = other.GetComponentInParent<DoorInteraction>();
        if (door != null)
        {
            Debug.Log("Found door script on " + door.name);
            nearbyDoor = door;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DoorInteraction door = other.GetComponentInParent<DoorInteraction>();
        if (door != null && door == nearbyDoor)
        {
            nearbyDoor = null;
        }
    }

    void Update()
    {
        // RaycastHit variable which will collect information from objects 
        RaycastHit hit;

        // If the raycast hits something
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, interactionLayers))
        {
            // If the object it hit contains the Letter script
            if (hit.collider.gameObject.GetComponent<Letter>())
            {
                // Enable the interaction text
                interactionText.SetActive(true);

                // If the E key is pressed
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Call openCloseLetter on the Letter component
                    hit.collider.gameObject.GetComponent<Letter>().openCloseLetter();
                }
            }
            else
            {
                interactionText.SetActive(false);
            }
        }
        else
        {
            interactionText.SetActive(false);
        }
    }
}
```
- Trigger zones are used for doors: when the player enters a collider near a door, the script stores a reference to that door so it can be toggled when pressing E.
- Raycasting is used for anything in front of the player: when the player looks at a letter (within a certain distance), the script shows a UI prompt and listens for the E key to open the letter.
- The system is built to be expandable—later we can add puzzles that respond the same way.

### Player Movement
To handle walking, we implemented a simple Rigidbody-based movement system using Unity’s Input System. The movement logic is handled by the PlayerMovement script, which reacts to directional input and applies motion through the physics system.

Here’s the core of the script:
```csharp
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Transform orientation;
    [SerializeField] private float speed = 1f;

    private Vector2 playerInput;

    void OnMove(InputValue value)
    {
        playerInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = orientation.forward * playerInput.y + orientation.right * playerInput.x;
        Vector3 movement = moveDirection.normalized * speed * Time.fixedDeltaTime;

        rigidbody.MovePosition(rigidbody.position + movement);
    }
}
```
- OnMove() receives input from Unity’s Input System and stores it as a 2D Vector.
- In FixedUpdate(), that input is converted into world-space movement relative to the player’s orientation (which is rotated by the camera).
- The player is moved using Rigidbody.MovePosition, ensuring physics-based motion.

### Letter script
To give the player access to narrative clues and atmosphere, we created a system where you can interact with physical letters in the environment. When you look at a letter and press E, the 3D model disappears and is replaced with a full-screen UI version of the letter, making it easier to read. 
This is handled by a Letter script:
```csharp
using UnityEngine;

public class Letter : MonoBehaviour
{
    public GameObject letterUI;
    public Renderer letterMesh;
    public PlayerController player;

    private bool toggle;

    public void openCloseLetter()
    {
        // Flip the toggle state
        toggle = !toggle;

        // If closing the letter
        if (!toggle)
        {
            letterUI.SetActive(false);
            letterMesh.enabled = true;
            player.enabled = true;
        }

        // If opening the letter
        if (toggle)
        {
            letterUI.SetActive(true);
            letterMesh.enabled = false;
            player.enabled = false;
        }
    }
}
```
- The script keeps track of whether the letter is currently being read.
- When openCloseLetter() is called (from the player’s interaction raycast), the script toggles:
  - The UI panel, which displays the content of the letter in full-screen.
  - The physical mesh, which disappears while reading.
  - The PlayerController, which is disabled while reading to lock camera movement.
    
## UI
We used Unity’s built-in UI system to enhance player interaction. All UI elements are managed under a single Canvas and appear contextually.

### Interaction Prompt

A small piece of UI appears when the player looks at something interactable, like a door or letter. This is just a simple `GameObject` with a `Text` or `Image` component placed inside the canvas, set to appear when a valid object is detected by raycasting.

This is how it works:
- The UI prompt is initially disabled.
- When the `PlayerInteraction` script detects something with an `IInteractable` component, the prompt is shown.
- When the player looks away, it disappears again.

This kind of reactive UI keeps players aware of what they can do.

### Letter Reading

When reading a letter, the UI takes over the whole screen to show a clean, readable version of the letter. We implemented this with a fullscreen UI panel that is toggled on and off by the `Letter` script.

This UI panel contains:
- A background image of the letter texture.
- A close instruction (e.g. “Press E to close”).

To keep focus, the `PlayerController` is disabled while the UI is active, preventing movement and camera input.

### Puzzle UI

We’ve started preparing another piece of UI for our puzzle interaction. Like the letter system, it’s a canvas-based UI that becomes visible when the player interacts with a puzzle object (e.g., a picture frame on a table).

Right now, the UI includes:
- A square grid layout to contain the puzzle tiles.
- Logic to detect clicks, move tiles, and check if the puzzle is solved

We plan to continue using this modular UI approach throughout the game—keeping each interface contextual, toggled via interaction, and easy to manage through scripts.

## Assets

## Floor Plan
Insert image
