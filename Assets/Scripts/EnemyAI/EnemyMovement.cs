using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float accelDeccel;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float attackDistanceFromPlayer;
    private Rigidbody rb;
    private EnemyAINavigation enemyAINavigation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemyAINavigation = GetComponent<EnemyAINavigation>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Vector3 dir = enemyAINavigation.TargetDirection(LocationAwayFromPlayer(attackDistanceFromPlayer)); // get direction from EnemyAINavigation using our target

        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, dir.normalized * maxSpeed, accelDeccel * Time.deltaTime); // Apply movement with max speed
    }

    Vector3 LocationAwayFromPlayer(float distance) // Find a location a certain distance away from the player
    {
        Vector3 dirFromPlayer = (transform.position - playerTransform.position).normalized; //get direction
        Vector3 newPosition = playerTransform.position + dirFromPlayer * distance; // create new position from direction and distance

        return newPosition;
    }
}
