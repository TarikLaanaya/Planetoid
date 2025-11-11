using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject basicEnemyPrefab;
    [SerializeField] private Transform planetTransform;
    [SerializeField] private Transform playerRootTransform;
    [SerializeField] LayerMask planetSurfaceLayer;

    [Header("Base Settings")]
    [SerializeField] private int initialEnemyCount;
    [SerializeField] private float spawnRadius;
    [SerializeField] private float spawnHeight;

    private List<GameObject> enemiesList = new();


    // Spawn enemies when player enters trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnEnemies();
        }
    }

    // Tell enemies when player exits trigger
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            List<GameObject> tempList = new(); // Create copy of list to avoid modification during iteration (get an error for this otherwise)

            foreach (GameObject enemy in enemiesList)
            {
                bool shouldDestroy = enemy.GetComponent<BasicEnemyBrain>().TooFarFromBase();

                if (shouldDestroy)
                {
                    Destroy(enemy);
                    tempList.Add(enemy);

                }
            }
            
            // Remove destroyed enemies from main list
            foreach (GameObject enemy in tempList)
            {
                enemiesList.Remove(enemy);
            }
        }
    }

    void SpawnEnemies()
    {
        int initialCount = enemiesList.Count;
        for (int i = 0; i < initialEnemyCount - initialCount; i++) // Check if there are any active enemies already spawned and don't exceed initial count
        {
            Vector3 spawnPoint = FindRandomPointFromBase();

            GameObject enemy = Instantiate(basicEnemyPrefab, spawnPoint, Quaternion.identity); // Spawn enemy at random point around base

            enemy.SetActive(false); // Deactivate enemy until all setup is done

            // Set neccessary variables
            enemy.GetComponent<EnemyMovement>().planetTransform = planetTransform;
            enemy.GetComponent<PlanetGravitySim>().planetTransform = planetTransform;
            enemy.GetComponent<BasicEnemyBrain>().enemyBaseGameOBJ = this.gameObject;
            enemy.GetComponent<BasicEnemyBrain>().planetTransform = planetTransform;
            enemy.GetComponent<BasicEnemyBrain>().heightFromPlanetSurface = spawnHeight;
            enemy.GetComponent<BasicEnemyBrain>().playerRootTransform = playerRootTransform;

            enemy.SetActive(true); // Reactivate enemy after setup
            enemiesList.Add(enemy); // Add enemy to list
        }
    }

    Vector3 FindRandomPointFromBase()
    {
        float randRadiusX = Random.Range(-spawnRadius, spawnRadius);
        float randRadiusZ = Random.Range(-spawnRadius, spawnRadius);

        Vector3 posOnGround;

        #region FindGround

        Vector3 directionToPlanet = (planetTransform.position - transform.position).normalized;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, directionToPlanet, out hit, Mathf.Infinity, planetSurfaceLayer)) // Find point on planet surface
        {
            posOnGround = hit.point;
        }
        else
        {
            Debug.LogError("EnemyManager: planet surface not found");
            posOnGround = transform.position;
        }

        #endregion

        // Find a point in a random direction at a set height
        Vector3 spawnPoint = posOnGround + (transform.right * randRadiusX + transform.forward * randRadiusZ + transform.up * spawnHeight);

        return spawnPoint;
    }

    void OnDrawGizmos()
    {
        // Size of spawn area
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    public void PlayerSeenAlert()
    {
        foreach(GameObject enemy in enemiesList)
        {
            enemy.GetComponent<BasicEnemyBrain>().StartAttack();
        }
    }
}
