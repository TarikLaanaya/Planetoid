using UnityEngine;

public class TitleMenu : MonoBehaviour
{
    public GameObject SettingsPanel;
    public GameObject ThankYouGrace;

    void Start()
    {
        SettingsPanel.SetActive(false);
        ThankYouGrace.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void OpenSettings()
    {
        SettingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        SettingsPanel.SetActive(false);
    }

    public void ThankGrace()
    {
        ThankYouGrace.SetActive(true);
    }
}
