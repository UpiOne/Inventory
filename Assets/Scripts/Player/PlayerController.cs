using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float playerSpeed = 2.0f;

    private Rigidbody2D rb;
    private bool groundedPlayer;
    private PlayerInput playerInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        groundedPlayer = IsGrounded();

        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector2 move = new Vector2(input.x, input.y); 

        rb.velocity = move * playerSpeed;
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        return hit.collider != null;
    }
}
