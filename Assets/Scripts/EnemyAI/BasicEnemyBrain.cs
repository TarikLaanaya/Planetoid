using UnityEngine;
using System.Collections;

public class BasicEnemyBrain : MonoBehaviour
{
    [Header("Min and Max Search Duration")]
    [Tooltip("Min and max duration the enemy will search for the player before giving up")]
    [SerializeField] private Vector2 searchDuration;

    [Header("Patrol Settings")]
    [SerializeField] private float maxPatrolRange;
    [SerializeField] LayerMask planetSurfaceLayer;

    [Header("Attack Settings")]
    [SerializeField] private float attackDistanceFromPlayer;

    // Enemy States
    [HideInInspector]
    public enum EnemyState { Idle, Patrol, Attack }

    [HideInInspector]
    public EnemyState currentState;

    // Public Variables
    [HideInInspector]
    public Transform playerRootTransform;

    [HideInInspector]
    public GameObject enemyBaseGameOBJ;

    [HideInInspector]
    public Transform planetTransform;

    [HideInInspector]
    public float heightFromPlanetSurface;

    // Script References
    private EnemyMovement enemyMovement;

    private bool waiting;
    private bool chasePlayer;

    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyMovement.playerRootTransform = playerRootTransform;
        
        StartPatrol();
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Patrol:
                Patrolling();
                break;
            case EnemyState.Attack:
                Attacking();
                break;
        }
    }


    //------------- IDLE STATE -------------//

    void StartIdle()
    {
        currentState = EnemyState.Idle;
        enemyMovement.shouldMove = false;
        StartCoroutine(WaitAndStartNewAction(Random.Range(searchDuration.x, searchDuration.y), StartPatrol));
    }

    void Idle()
    {
        
    }
    

    //------------- PATROL STATE -------------//

    void StartPatrol()
    {
        currentState = EnemyState.Patrol;
        enemyMovement.targetPos = FindPatrolPoint();
        enemyMovement.shouldMove = true;
    }

    void Patrolling()
    {
        float distanceToTarget = Vector3.Distance(transform.position, enemyMovement.targetPos + (transform.up * heightFromPlanetSurface));

        if (distanceToTarget < 1f) // If we reached our patrol point
        {
            StartIdle();
        }
    }

    Vector3 FindPatrolPoint()
    {
        float randomRangeX = Random.Range(-maxPatrolRange, maxPatrolRange);
        float randomRangeZ = Random.Range(-maxPatrolRange, maxPatrolRange);

        Transform enemyBaseTransform = enemyBaseGameOBJ.transform;

        // Set patrol point in random direction from enemy base
        Vector3 patrolPoint = enemyBaseTransform.position + (enemyBaseTransform.right * randomRangeX + enemyBaseTransform.forward * randomRangeZ + enemyBaseTransform.up * 5f);

        // ----- Raycast down and assign correct height ----- //

        Vector3 directionToPlanet = (planetTransform.position - patrolPoint).normalized;

        RaycastHit hit;

        if (Physics.Raycast(patrolPoint, directionToPlanet, out hit, Mathf.Infinity, planetSurfaceLayer)) // Find point on planet surface
        {
            patrolPoint = hit.point;
        }
        else
        {
            Debug.LogError("BasicEnemyBrain: planet surface not found");
            patrolPoint = transform.position; // Stay in place if we fail to find planet surface
            StartIdle(); // Go back to idle if we fail to find planet surface
        }

        return patrolPoint;
    }


    //------------- ATTACK STATE -------------//

    public void StartAttack()
    {
        currentState = EnemyState.Attack;
        StopCoroutine("WaitAndStartNewAction"); // Stop any waiting coroutines
        
        enemyMovement.shouldMove = true;
        enemyMovement.lookAtPlayer = true;
        chasePlayer = true;
    }

    void Attacking()
    {
        if (chasePlayer)
        {
            enemyMovement.targetPos = LocationAwayFromPlayer(playerRootTransform.position);
        }
        else
        {
            enemyMovement.targetPos = transform.position; // Stay in place

            if (Vector3.Distance(transform.position, playerRootTransform.position) > attackDistanceFromPlayer * 2) // If player is too far go back to base
            {
                enemyMovement.lookAtPlayer = false;
                StartPatrol();
            }
        }
    }

    Vector3 LocationAwayFromPlayer(Vector3 playerPos) // Find a location a certain distance away from the player
    {
        Vector3 dirFromPlayer = (transform.position - playerPos).normalized; //get direction
        playerPos = playerPos + dirFromPlayer * attackDistanceFromPlayer; // create new position from direction and distance

        return playerPos;
    }

    //---------- Destroy Call ------------//

    public bool TooFarFromBase() // Should this enemy be destroyed
    {
        chasePlayer = false;

        if (currentState == EnemyState.Attack) { return false; } return true; // Don't destroy if attacking otherwise destroy
    }

    //---------- COMMON FUNCTIONS ------------//
    
    IEnumerator WaitAndStartNewAction(float waitTime, System.Action newAction)
    {
        if (waiting) { Debug.LogError("already waiting"); yield break; } // Prevent multiple coroutines from running (in case we call this from update)
        
        waiting = true;

        yield return new WaitForSeconds(waitTime);

        waiting = false;

        if (currentState != EnemyState.Attack) { newAction(); } // Only start new action if we are not attacking
    }
}
