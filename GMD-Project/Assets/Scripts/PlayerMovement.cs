using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Transform orientation;

    [SerializeField] private float speed;

    private Vector3 moveDirection;
    private Vector2 playerInput;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = orientation.forward * playerInput.y + orientation.right * playerInput.x;
        transform.Translate(moveDirection * (speed * Time.deltaTime));
    }

    void OnMove(InputValue value)
    {
        playerInput = value.Get<Vector2>();
        
    }
}
