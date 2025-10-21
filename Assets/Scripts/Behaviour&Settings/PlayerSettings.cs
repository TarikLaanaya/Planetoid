using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public static int WeaponSensitivity;
    public MonoBehaviour cameraMovement2; // Changed to MonoBehaviour as temporary fix
    void Start()
    {
        WeaponSensitivity = PlayerPrefs.GetInt("WeaponSensitivity", 500); // Load saved sensitivity
    }

    public static void SaveScore()
    {
        PlayerPrefs.SetInt("WeaponSensitivity", WeaponSensitivity);
        PlayerPrefs.Save(); 
        // Comment out or remove the line below until CameraMovement2 class is implemented
        // CameraMovement2.UpdateSensitivity(WeaponSensitivity);
    }
}
