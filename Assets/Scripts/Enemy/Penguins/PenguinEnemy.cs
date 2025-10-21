using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PenguinEnemy : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    [Header("Health")]
    [SerializeField] private int maxHealth = 10; // Takes 10 hits (gun does 1 damage)
    private int currentHealth;

    [Header("Attack")]
    public float timeBetweenAttacks = 1.5f;
    private bool alreadyAttacked;
    public GameObject projectile; // (unused in this stripped logic)

    [Header("Perception")]
    public float sightRange = 15f;
    public float attackRange = 5f;
    public bool playerInSightRange;
    public bool playerInAttackRange;

    // Patrol
    [SerializeField] private float patrolRadius = 20f;
    [SerializeField] private float patrolInterval = 5f;
    private float nextPatrolTime;

    private void Awake()
    {
        currentHealth = maxHealth;
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        // Detection
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
            Patroling();
        else if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();
        else if (playerInSightRange && playerInAttackRange)
            AttackPlayer();
    }

    #region Health
    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
        // Optional: hit reaction (animation, flash, sound)
    }

    private void Die()
    {
        // TODO: death animation / loot / effects
        Destroy(gameObject);
    }
    #endregion

    #region Core AI
    private void FindPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity;
        GameObject closest = null;

        foreach (var p in players)
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = p;
            }
        }

        if (closest != null)
            player = closest.transform;
    }

    private void Patroling()
    {
        if (Time.time < nextPatrolTime) return;
        nextPatrolTime = Time.time + patrolInterval;

        Vector3 randomPos = RandomNavSphere(transform.position, patrolRadius, NavMesh.AllAreas);
        agent.SetDestination(randomPos);
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Placeholder for projectile / melee
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack() => alreadyAttacked = false;

    public static Vector3 RandomNavSphere(Vector3 origin, float radius, int areaMask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius + origin;
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, areaMask))
            return hit.position;
        return origin;
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}