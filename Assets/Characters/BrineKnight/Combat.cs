using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private GameObject hitboxPrefab;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float hitboxDuration = 0.2f;

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

        // Calculate spawn position relative to attackPoint
        Vector3 spawnPos = transform.position + new Vector3(attackPoint.localPosition.x * facing, attackPoint.localPosition.y, 0);

        // Spawn hitbox
        GameObject hitbox = Instantiate(hitboxPrefab, spawnPos, Quaternion.identity);

        // Preserve prefab scale, flip X if needed
        Vector3 originalScale = hitboxPrefab.transform.localScale;
        hitbox.transform.localScale = new Vector3(Mathf.Abs(originalScale.x) * facing, originalScale.y, originalScale.z);

        Destroy(hitbox, hitboxDuration);
    }
}