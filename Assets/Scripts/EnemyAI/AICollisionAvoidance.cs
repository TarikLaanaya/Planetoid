using UnityEngine;

public class AICollisionAvoidance : MonoBehaviour
{
    [SerializeField] private EnemyMovement enemyMovement;
    private bool colliding;
    private Collider target;
    private int numInCollider;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("avoidCollider"))
        {
            colliding = true;
            target = other;
            numInCollider += 1;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("avoidCollider"))
        {
            numInCollider -= 1;
            
            if(numInCollider == 0)
            {
                colliding = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (colliding && target != null)
        {
            enemyMovement.MoveAwayFrom(target.transform.position);
        }
    }
}
