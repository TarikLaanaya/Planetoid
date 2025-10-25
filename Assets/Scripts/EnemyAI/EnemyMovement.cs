using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float accelDeccel;
    [SerializeField] private float rotationDampening;
    private Rigidbody rb;
    private EnemyAINavigation enemyAINavigation;


    // public variables
    [HideInInspector]
    public bool shouldMove;

    [HideInInspector]
    public Vector3 targetPos;

    [HideInInspector]
    public bool lookAtMoveDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemyAINavigation = GetComponent<EnemyAINavigation>();

        lookAtMoveDir = true;
    }

    void FixedUpdate()
    {
        if (shouldMove)
        {
            MoveToTarget();
        }
        else
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, accelDeccel * Time.deltaTime); // smoothly stop
        }
    }

    private void MoveToTarget()
    {
        Vector3 direction = enemyAINavigation.TargetDirection(targetPos); // get direction from EnemyAINavigation using our target

        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, direction.normalized * maxSpeed, accelDeccel * Time.deltaTime); // Apply movement with max speed

        if(!lookAtMoveDir) return;

        // Rotate enemy to face movement direction
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationDampening * Time.deltaTime);
    }
}
