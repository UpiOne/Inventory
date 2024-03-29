using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectRange = 10f; 
    [SerializeField] private float attackRange = 2f; 

    [Header("Attack Settings")]
    [SerializeField] private float attackDelay = 2f; 
    [SerializeField] private int damage = 10; 

    [Header("Target")]
    [SerializeField] private Transform target; 
    [SerializeField] private LayerMask targetLayer; 
    
    [Header("Navigation")]
    [SerializeField] private NavMeshAgent agent; 

    private float lastAttackTime; 
    private EnemyAttack enemyAttack;

    private void Start()
    {   
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        target = GameObject.FindGameObjectWithTag("Player").transform;

        enemyAttack = new EnemyAttack(target.GetComponent<IAttackable>());
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, target.position) <= detectRange)
        {
            agent.SetDestination(target.position);

            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget <= attackRange)
            {
                if (Time.time - lastAttackTime >= attackDelay)
                {
                    enemyAttack.Attack(damage);

                    lastAttackTime = Time.time;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
