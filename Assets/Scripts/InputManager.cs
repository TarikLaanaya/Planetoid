using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public bool acceptInput = true;

    //[SerializeField] private KeyCode pauseKey = KeyCode.Escape; // Example of a serialized field for a pause key

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen (Temporary)
    }

    void Update()
    {
        // Check for different inputs
    }
}
