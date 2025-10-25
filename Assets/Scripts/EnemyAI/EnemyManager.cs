using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject basicEnemyPrefab;
    [SerializeField] private Transform planetTransform;

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

    // Destroy enemies when player exits trigger
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject enemy in enemiesList)
            {
                Destroy(enemy);
            }
            enemiesList.Clear();
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < initialEnemyCount; i++)
        {
            Vector3 spawnPoint = FindRandomPointFromBase();

            GameObject enemy = Instantiate(basicEnemyPrefab, spawnPoint, Quaternion.identity); // Spawn enemy at random point around base

            enemy.SetActive(false); // Deactivate enemy until all setup is done

            // Set neccessary variables
            enemy.GetComponent<EnemyAINavigation>().planetTransform = planetTransform;
            enemy.GetComponent<PlanetGravitySim>().planetTransform = planetTransform;
            enemy.GetComponent<BasicEnemyBrain>().enemyBaseTransform = transform;
            enemy.GetComponent<BasicEnemyBrain>().planetTransform = planetTransform;
            enemy.GetComponent<BasicEnemyBrain>().heightFromPlanetSurface = spawnHeight;

            enemy.SetActive(true); // Reactivate enemy after setup
            enemiesList.Add(enemy); // Add enemy to list
        }
    }

    Vector3 FindRandomPointFromBase()
    {
        float randRadiusX = Random.Range(-spawnRadius, spawnRadius);
        float randRadiusZ = Random.Range(-spawnRadius, spawnRadius);

        Vector3 spawnPoint = transform.position + (transform.right * randRadiusX + transform.forward * randRadiusZ + transform.up * spawnHeight);

        return spawnPoint;
    }

    void OnDrawGizmos()
    {
        // Size of spawn area
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
