using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Character Prefabs")]
    public GameObject FiftyCal;
    public GameObject Minigun;

    [Header("Character Spawn")]
    public Transform spawnPoint;
    public void choose50Cal()
    {
        Instantiate (FiftyCal, spawnPoint.position, spawnPoint.rotation);
    }
    
    public void chooseMinigun()
    {
        Instantiate (Minigun, spawnPoint.position, spawnPoint.rotation);
    }

    public void returnToMenu()
    {
        SceneManager.LoadScene("TitleMenu");
    }
}
