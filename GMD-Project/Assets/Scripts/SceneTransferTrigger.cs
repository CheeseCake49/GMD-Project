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
