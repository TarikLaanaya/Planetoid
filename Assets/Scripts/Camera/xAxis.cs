using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xAxis : MonoBehaviour
{
    public PlayerSettings PlayerSettings;
    public float mouseSensitivity;

    float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mouseSensitivity = PlayerSettings.WeaponSensitivity;
    }

    void Update()
    {
        // Get mouse X movement (horizontal)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        // Accumulate the yRotation based on the mouse X movement
        yRotation += mouseX;

        // Apply only the yRotation to rotate the object around the Y-axis
        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
