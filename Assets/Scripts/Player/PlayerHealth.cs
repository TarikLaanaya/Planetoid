using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject playerModel;

    [Header("Restart After Death Settings")]
    [SerializeField] private float restartDelay = 4f;
    [SerializeField] private InputManager inputManager;

    private float health;
    private bool dead = false;

    void Start()
    {
        health = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        float damage = 0;

        if (other.gameObject.GetComponent<Bullet>() != null)
        {
            damage = other.gameObject.GetComponent<Bullet>().damage;
        }

        switch (other.gameObject.tag)
        {
            case "EnemyBullet":
                TakeDamage(damage);
                Destroy(other.gameObject);
                break;

            // Add other cases for different bullets (when/if added)
        }

    }

    void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    void Die()
    {
        inputManager.acceptInput = false;

        dead = true;

        Instantiate(explosionPrefab, transform.position, transform.rotation);

        playerModel.SetActive(false);

        StartCoroutine(RestartSceneAfterDelay(restartDelay));
    }
    
    IEnumerator RestartSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
