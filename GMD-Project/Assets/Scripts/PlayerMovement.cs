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

