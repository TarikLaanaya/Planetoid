using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public bool acceptInput = true;
    public float moveX { get; private set; }
    public float moveZ { get; private set; }

    public bool boostButtonHeld { get; private set; }

    //[SerializeField] private KeyCode pauseKey = KeyCode.Escape; // Example of a serialized field for a pause key

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen (Temporary)
    }

    void Update()
    {
        if (!acceptInput) return;
        // Check for different inputs

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        boostButtonHeld = Input.GetKey(KeyCode.LeftShift);
    }
}
