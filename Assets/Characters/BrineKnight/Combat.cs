using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private GameObject hitboxPrefab;

    [Header("Timing")]
    [SerializeField] private float hitboxDuration = 0.2f;

    [Header("Offsets")]
    [SerializeField] private Vector2 sideAttackOffset = new Vector2(2f, 0f); // Horizontal attacks
    [SerializeField] private float upAttackOffset = 2.5f; // Up attacks
    [SerializeField] private float downAttackOffset = 2.5f; // Down attacks

    [Header("Ground Check (for down attack)")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;

    private void Update()
    {
        HandleAttack();
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        float facing = Mathf.Sign(transform.localScale.x);
        Vector3 spawnPos = transform.position;
        Quaternion rotation = Quaternion.identity;

        if (Input.GetKey(KeyCode.W)) // Up attack
        {
            spawnPos += Vector3.up * upAttackOffset;
            rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (Input.GetKey(KeyCode.S)) // Down attack
        {
            bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            if (!isGrounded) // Down attack if mid-air
            {
                spawnPos += Vector3.down * downAttackOffset;
                rotation = Quaternion.Euler(0, 0, -90);
            }
            else // Horizontal attack if grounded
            {
                spawnPos += new Vector3(sideAttackOffset.x * facing, sideAttackOffset.y, 0f);
                rotation = Quaternion.identity;
            }
        }
        else // Horizontal attack
        {
            spawnPos += new Vector3(sideAttackOffset.x * facing, sideAttackOffset.y, 0f);
            rotation = Quaternion.identity;
        }

        // Spawn hitbox
        GameObject hitbox = Instantiate(hitboxPrefab, spawnPos, rotation);

        // Flip side attacks horizontally
        if (rotation == Quaternion.identity) 
        {
            Vector3 originalScale = hitboxPrefab.transform.localScale;
            hitbox.transform.localScale = new Vector3(Mathf.Abs(originalScale.x) * facing, originalScale.y, originalScale.z);
        }

        Destroy(hitbox, hitboxDuration);
    }
}