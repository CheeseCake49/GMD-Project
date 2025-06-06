---
title: "Blog 4 Developer Post"
author: "Johan & Simone"
date: "20-05-2024"
version: "0.1"
---

# Game Development Progress Report

## Introduction

## TO-DO
- [X] Make the second scene(including interior)
- [X] Make the third scene(including interior)
- [X] Loop through scenes + Canvas/fade out
- [X] Add sound to the different scenes 

## Progress Overview
Our main goals for this blog has been:
To create the majority of the game world. 
The fundamental looping logic to move between the different scenes has been created. 
The remaining scenes have been created and the first scene has been refined.

## Scene Building

With the first scene completed earlier in the process, we shifted focus to designing and constructing the remaining two scenes. Each scene represents a different phase in the player’s journey (utopia, glitch-reality, and dystopia) mirroring the unraveling of the protagonist’s digital consciousness.

The second scene builds on the architecture of the first but strips away the dreamlike elements, replacing them with more grounded, realistic objects and lighting. Decorative images and notes, hinting at memories returning in fragments. This shift marks the player’s gradual reawakening to reality, as fragments of consciousness begin surfacing through subtle clues in the environment.
This is where the experiment becomes clear: the player is not just wandering a house, but acting as a test subject — part of an attempt to transfer human consciousness into software. The scene reflects a mind caught between fading memories and artificial boundaries, struggling to reconstruct itself.

For the third scene, we leaned fully into themes of decay and abandonment. The vibrant walls and structured layouts of earlier scenes have given way to darkness, emptiness, and disrepair. Many objects are missing, others are broken or misplaced, and the entire space feels hollow—like something once inhabited but long since forgotten.
This scene is built on the idea that no one is left to maintain the system. There’s been an event—possibly an apocalypse in the outside world—and now the server housing the player's consciousness has been left running without oversight or purpose. Visual glitches, broken lighting, and warped textures subtly suggest data corruption over time, as if the simulation itself is rotting from within.
The player, now fully aware, is trapped in this digital echo. The simulation continues to loop, not out of function, but out of habit—an endless cycle driven by machinery with no one left to shut it off. It's not just a game environment; it's a mausoleum for a failed experiment in immortality.

Scene building has been a delicate balancing act between world design, narrative progression, and player emotion. Each room, object, and transition has been chosen to evoke curiosity, unease, or recognition. We’ve also reused and restructured assets from earlier scenes to subtly reinforce the idea of looping and recursion.

## Loop through scenes + Canvas/fade out

To create smooth transitions between scenes in our game, we implemented a reusable **Scene Transition Manager** using a full-screen UI canvas and fade animations.

The `SceneTransition` script handles scene loading with a fade-to-black effect by animating the value of a UI `Image`. The fade canvas persists across scenes using `DontDestroyOnLoad`, allowing it to manage transitions globally.

How it works
- The screen fades to black over a configurable duration.
- The new scene loads asynchronously in the background.
- Once loading completes, the screen fades back in to reveal the new scene.


Code Components

`SceneTransition.cs`  
Controls the fade effect and loads the new scene.

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;
    
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void FadeToScene(string sceneToLoad)
    {
        StartCoroutine(FadeAndLoadScene(sceneToLoad));
    }
    
    private IEnumerator FadeAndLoadScene(string sceneToLoad)
    {
        yield return StartCoroutine(Fade(0f, 1f)); // Fade out

        yield return new WaitForSeconds(fadeDuration);
        
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneToLoad);
        yield return loadOp;

        yield return StartCoroutine(Fade(1f, 0f)); // Fade in
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;
        color.a = startAlpha;
        
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
```



`SceneTransferTrigger.cs`  
Detects when the player reaches a trigger zone and starts the scene change.

```csharp
using UnityEngine;

public class SceneTransferTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneTransition.Instance.FadeToScene(sceneToLoad);
        }
    }
}
   
```



