using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Sensitivity : MonoBehaviour
{
    public int sensitivity = 500;
    public Transform playerBody;
    public Transform sealMinigun;
    float xRotation = 0f;
    float yRotation = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        sensitivity = PlayerPrefs.GetInt("Sensitivity", 500);
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        
        xRotation -= mouseY; // Update xRotation based on vertical mouse movement
        xRotation = Mathf.Clamp(xRotation, -70f, 40f);
        
        yRotation += mouseX;
        
        // Apply rotations properly
        sealMinigun.localRotation = Quaternion.Euler(0f, 0f, xRotation);   
        playerBody.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    public void UpdateSensitivity(float newSensitivity)
    {
        sensitivity = PlayerPrefs.GetInt("Sensitivity", 500);
    }
}