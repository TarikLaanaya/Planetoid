using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField] private BasicEnemyBrain enemyBrain;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(Physics.Raycast(transform.position, (other.transform.position - transform.position).normalized, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    enemyBrain.StartAttack();
                }
            }
        }
    }
}
