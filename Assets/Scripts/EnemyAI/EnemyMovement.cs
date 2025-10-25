using UnityEngine;

public class EnemyMovement : MonoBehaviour //Should rename to EnemyNavigation
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float accelDeccel;
    [SerializeField] private float rotationDampening;
    private Rigidbody rb;


    // public variables
    [HideInInspector]
    public bool shouldMove;

    [HideInInspector]
    public Vector3 targetPos;

    [HideInInspector]
    public bool lookAtPlayer;
    [HideInInspector]
    public Transform playerRootTransform;

    [HideInInspector]
    public Transform planetTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        lookAtPlayer = false;
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
        Vector3 direction = TargetDirection(targetPos);

        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, direction.normalized * maxSpeed, accelDeccel * Time.deltaTime); // Apply movement with max speed

        if (lookAtPlayer)
        {
            direction = TargetDirection(playerRootTransform.position); // look directly at target position
            Debug.Log("Looking directly at target");
        }
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationDampening * Time.deltaTime);
    }
    
    // Find direction to target position along planet surface
    public Vector3 TargetDirection(Vector3 targetPosition)
    {
        // Raw direction to target (goes through planet)
        Vector3 dirToTarget = targetPosition - transform.position;

        // Get the direction between the planet center and the enemy AI (planets surface normal)
        Vector3 planetSurfaceNormal = (transform.position - planetTransform.position).normalized;
        
        // Project the direction to target onto the planet surface normal to get a horizontal direction (essentially deleting the vertical axis)
        Vector3 horizontalDirToTarget = Vector3.ProjectOnPlane(dirToTarget, planetSurfaceNormal).normalized;

        return horizontalDirToTarget;
    }
}
