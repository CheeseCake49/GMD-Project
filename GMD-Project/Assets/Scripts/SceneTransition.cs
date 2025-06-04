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
        Debug.Log("SceneTransitionManager Awake in scene: " + SceneManager.GetActiveScene().name);
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("Set instance");
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Destroying object");
            Destroy(gameObject);
        }
    }
    
    public void FadeToScene(string sceneToLoad)
    {
        StartCoroutine(FadeAndLoadScene(sceneToLoad));
    }
    
    private IEnumerator FadeAndLoadScene(string sceneToLoad)
    {
        yield return StartCoroutine(Fade(0f, 1f));

        yield return new WaitForSeconds(fadeDuration);
        
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneToLoad);
        yield return loadOp;
        Debug.Log("Scene loaded: " + SceneManager.GetActiveScene().name);
        
        yield return null;

        yield return StartCoroutine(Fade(1f, 0f));
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
