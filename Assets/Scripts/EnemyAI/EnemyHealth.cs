using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    private float health;
    [SerializeField] GameObject enemyParent;

    void Start()
    {
        
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
                TakeDamage(100);
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
        maxHealth -= damage;
        if (maxHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        //Explosion animation
        Destroy(enemyParent);
    }
}

