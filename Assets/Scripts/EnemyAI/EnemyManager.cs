using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject basicEnemyPrefab;
    [SerializeField] private Transform planetTransform;
    [SerializeField] private Transform playerRootTransform;
    [HideInInspector] public WinCondition winCondition;

    [Header("Base Settings")]
    public int initialEnemyCount;
    [SerializeField] private float spawnRadius;
    [SerializeField] private float spawnHeightMin;
    [SerializeField] private float spawnHeightMax;

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
                if (enemy == null) continue; // Skip if enemy has been destroyed

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
            enemy.GetComponent<BasicEnemyBrain>().startHeightFromPlanetSurface = Random.Range(spawnHeightMin, spawnHeightMax);
            enemy.GetComponent<BasicEnemyBrain>().playerRootTransform = playerRootTransform;

            enemy.SetActive(true); // Reactivate enemy after setup
            enemiesList.Add(enemy); // Add enemy to list
        }
    }

    Vector3 FindRandomPointFromBase()
    {
        float randRadiusX = Random.Range(-spawnRadius, spawnRadius);
        float randRadiusZ = Random.Range(-spawnRadius, spawnRadius);

        Vector3 spawnPoint = transform.position + (transform.right * randRadiusX + transform.forward * randRadiusZ + transform.up * Random.Range(spawnHeightMin, spawnHeightMax));

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
            if (enemy == null) continue; // Skip if enemy has been destroyed

            enemy.GetComponent<BasicEnemyBrain>().StartAttack();
        }
    }
}
