using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField] private BasicEnemyBrain enemyBrain;
    [SerializeField] private LayerMask enemyLayer;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(Physics.Raycast(transform.position, (other.transform.position - transform.position).normalized, out RaycastHit hit, Mathf.Infinity, ~enemyLayer))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    enemyBrain.StartAttack();
                }
            }
        }
    }
}