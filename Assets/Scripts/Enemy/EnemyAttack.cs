using UnityEngine;

public class EnemyAttack
{
    private IAttackable attackTarget;

    public EnemyAttack(IAttackable target)
    {
        attackTarget = target;
    }

    public void Attack(int damage)
    {
        if (attackTarget != null)
        {
            attackTarget.TakeDamage(damage);
        }
        else
        {
            Debug.LogError("Attack target not found!");
        }
    }
}