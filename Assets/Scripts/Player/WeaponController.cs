using UnityEngine;

public class WeaponController : MonoBehaviour
{
     [Header("Weapon Settings")]
    [SerializeField] private float attackRadius = 3f;
    [SerializeField] private int damage;
    [SerializeField] private LayerMask enemyLayer;

    public LineRenderer attackRadiusVisual;
    public Inventory inventory;

    private void Start()
    {
        attackRadiusVisual.positionCount = 100; 
        UpdateAttackRadiusVisual();
    }

    public void Shoot()
    {
        if (inventory != null)
        {

            foreach (InventorySlot slot in inventory.inventorySlots)
            {
                if (slot.item != null && slot.item.itemType == ItemType.Ammo && slot.stack > 0)
                {
                    slot.UseItem();

                    Collider2D nearestEnemy = Physics2D.OverlapCircle(transform.position, attackRadius, enemyLayer);

                    if (nearestEnemy != null)
                    {
                        EnemyHealth enemyHealth = nearestEnemy.GetComponent<EnemyHealth>();
                        if (enemyHealth != null)
                        {
                            enemyHealth.TakeDamage(damage);
                        }
                    }

                    return;
                }
            }
        }

        Debug.Log("Нет патронов!");
    }

    private void UpdateAttackRadiusVisual()
    {
        for (int i = 0; i < attackRadiusVisual.positionCount; i++)
        {
            float angle = i * Mathf.PI * 2f / (attackRadiusVisual.positionCount - 1);
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * attackRadius;
            attackRadiusVisual.SetPosition(i, (Vector2)transform.position + offset);
        }
    }
}
