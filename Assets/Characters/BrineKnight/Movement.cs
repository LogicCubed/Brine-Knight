using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float sprintMultiplier = 2f;

    [SerializeField] private float jumpForce = 18f;
    [SerializeField] private float jumpHoldMultiplier = 3f;
    [SerializeField] private float maxJumpHoldTime = 0.75f;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;

    private bool isJumping;
    private float jumpTimeCounter;
    private bool isGrounded;

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        HandleJump();
    }

    private void FixedUpdate()
    {
        HandleHorizontalMovement();
    }

    private void HandleHorizontalMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");

        bool isSprinting = Input.GetMouseButton(1) && isGrounded;

        if (moveX != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(moveX) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        float currentSpeed = moveSpeed * (isSprinting ? sprintMultiplier : 1f);
        rb.linearVelocity = new Vector2(moveX * currentSpeed, rb.linearVelocity.y);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter < maxJumpHoldTime)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce + jumpHoldMultiplier * jumpTimeCounter);
                jumpTimeCounter += Time.deltaTime;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }
}