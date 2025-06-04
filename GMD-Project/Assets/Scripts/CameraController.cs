using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    [SerializeField] private Camera camera;
    [SerializeField] private Transform orientation;

    private float xRotation;
    private float yRotation;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        camera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void OnLook(InputValue value)
    {
        var camMovement = value.Get<Vector2>();

        yRotation += camMovement.x * Time.deltaTime * sensX;
        xRotation -= camMovement.y * Time.deltaTime * sensY;
    }
}
