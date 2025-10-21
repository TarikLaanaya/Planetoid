using UnityEngine;
using UnityEngine.InputSystem;

public class EscapeMenu : MonoBehaviour
{
    public Camera PlayerCamera;
    public bool isPaused = false;
    public GameObject escapeMenu;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        escapeMenu.SetActive(false);
    }

    void Update()
    {
        escapePressed();
    }

    public void escapePressed()
    {
        //if (Keyboard.current.escapeKey.wasPressedThisFrame)
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
                isPaused = false;
                escapeMenu.SetActive(false);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                isPaused = true;
                escapeMenu.SetActive(true);
            }
        }
    }
}
