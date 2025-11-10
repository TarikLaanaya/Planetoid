using UnityEngine;

public class PlayerVerticalAim : MonoBehaviour
{
    [Header("Movement Settings")]
    public float upperLimit = 200f;
    public float lowerLimit = -200f;
    public float moveSpeed = 10f;

    public RectTransform crosshairTransform;
    float currentY;

    void Start()
    {
        
    }

    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y");

        // Add to currentY depending on MouseY positive/negative * the sensitivity (moveSpeed) independent of framerate (Time.deltatime)
        currentY += mouseY * moveSpeed * Time.deltaTime;

        // Limit currentY to the upper and lower limit
        currentY = Mathf.Clamp(currentY, lowerLimit, upperLimit);

        // Set crosshair position to current Y (ignore x because we only move vertically). Use lerp to make it move smoothly
        crosshairTransform.anchoredPosition = Vector2.Lerp(crosshairTransform.anchoredPosition,  new Vector2(0f, currentY), Time.deltaTime * 10f);
    }

    public Vector3 GetCrosshairPos()
    {
        return crosshairTransform.position; //position in 3d space
    }
}