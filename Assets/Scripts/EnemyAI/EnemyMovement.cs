using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float accelDeccel;
    [SerializeField] private Transform playerTransform;
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
        Vector3 dir = enemyAINavigation.TargetDirection(playerTransform.position); // get target direction from EnemyAINavigation

        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, dir.normalized * maxSpeed, accelDeccel * Time.deltaTime); // Apply movement with max speed
    }
}
