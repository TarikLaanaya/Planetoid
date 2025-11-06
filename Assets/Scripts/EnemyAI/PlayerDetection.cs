using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField] private BasicEnemyBrain enemyBrain;
    [SerializeField] private LayerMask enemyLayer;

    void OnTriggerEnter(Collider other)
    {
        if (enemyBrain.currentState != BasicEnemyBrain.EnemyState.Attack && other.CompareTag("Player"))
        {
            // check we can actually see the player and ignore other enemies in this check
            if(Physics.Raycast(transform.position, (other.transform.position - transform.position).normalized, out RaycastHit hit, Mathf.Infinity, ~enemyLayer))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    enemyBrain.StartAttack();
                    enemyBrain.enemyBaseGameOBJ.GetComponent<EnemyManager>().PlayerSeenAlert();
                }
            }
        }
    }
}