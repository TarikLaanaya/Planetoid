using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yAxis : MonoBehaviour
{
    public PlayerSettings PlayerSettings;
    public float mouseSensitivity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mouseSensitivity = PlayerSettings.WeaponSensitivity;
    }

    public Transform player;  // Reference to the body object
    public Transform pivot;   // Reference to the new pivot object

    float zRotation = 0f;   // Vertical (Z-axis) rotation
    public float topClamp = -70f;  // Limit for looking down
    public float bottomClamp = 40f; // Limit for looking up

    void Update()
    {
        // Get mouse Y input for vertical movement (Y-axis)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust the zRotation for vertical movement
        zRotation -= mouseY;
        zRotation = Mathf.Clamp(zRotation, topClamp, bottomClamp);

        // Apply the rotation to the pivot object around the Z-axis
        pivot.localRotation = Quaternion.Euler(pivot.localRotation.eulerAngles.x, pivot.localRotation.eulerAngles.y, zRotation);
    }
}
