using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private GameObject explosionPrefab;

    private float health;
    [SerializeField] GameObject enemyParent;

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        
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
            case "Missile":
                TakeDamage(damage);
                Destroy(other.gameObject);
                break;

            case "GatlingBullet":
                TakeDamage(damage);
                Destroy(other.gameObject);
                break;
            case "ChargeBullet":
                TakeDamage(damage);
                break;
        }

    }

    void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        
        Destroy(enemyParent);
    }
}

