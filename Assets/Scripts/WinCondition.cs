using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyBases;
    private int enemies;

    void Start()
    {
        foreach (GameObject enemyBase in enemyBases)
        {
            EnemyManager enemyManager = enemyBase.GetComponent<EnemyManager>();
            enemies += enemyManager.initialEnemyCount;
            enemyManager.winCondition = this;
        }
    }

    public void EnemyDestroyed()
    {
        enemies -= 1;

        if (enemies <= 0)
        {
            //YOU WIN
            Debug.Log("WIN");
        }
    }
}