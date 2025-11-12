using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WinCondition : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyBases;
    private int enemies;
    public Image Crosshair;
    public Image Minigun;
    public Image ChargeGun;
    public Image Missile;
    public Image MissileGrey;
    public Canvas VictoryCanvas;
    public Button VictoryReturnToMenu;


    void Start()
    {
        VictoryCanvas.enabled = false;
        Crosshair.enabled = true;
        Minigun.enabled = true;
        ChargeGun.enabled = true;
        Missile.enabled = true;
        MissileGrey.enabled = true;
        foreach (GameObject enemyBase in enemyBases)
        {
            EnemyManager enemyManager = enemyBase.GetComponent<EnemyManager>();
            enemies += enemyManager.initialEnemyCount;
            enemyManager.winCondition = this;
        }
    }

    public void ReturnToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleMenu");
    }

    public void EnemyDestroyed()
    {
        enemies -= 1;

        if (enemies <= 0)
        {
            //YOU WIN
            VictoryCanvas.enabled = true;
            Crosshair.enabled = false;
            Minigun.enabled = false;
            ChargeGun.enabled = false;
            Missile.enabled = false;
            MissileGrey.enabled = false;
            Debug.Log("WIN");
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}