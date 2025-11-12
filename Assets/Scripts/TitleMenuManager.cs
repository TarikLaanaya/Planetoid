using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TitleMenuManager : MonoBehaviour
{
    public Button PlayButton;
    public Button QuitButton;

    private void Start()
    {
        PlayButton.onClick.AddListener(OnPlayButtonClicked);
        QuitButton.onClick.AddListener(OnQuitButtonClicked);
    }
    private void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("ArtTests");
    }
    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
