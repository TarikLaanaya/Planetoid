using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] GameObject enemyParent;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Missile":
                TakeDamage(100);
                Destroy(other.gameObject);
                break;

            case "GatlingBullet":
                TakeDamage(4);
                Destroy(other.gameObject);
                break;
            case "ChargeBullet":
                int damage = other.gameObject.transform.localScale.x switch
                {
                    >= 3f => 50,
                    >= 2f => 30,
                    _ => 1,
                };
                TakeDamage(damage);
                break;
        }

    }

    void TakeDamage(int damage)
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

