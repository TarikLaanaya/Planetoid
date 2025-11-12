using UnityEngine;

public class PlayerVerticalAim : MonoBehaviour
{
    [Header("Movement Settings")]
    public float upperLimit = 200f;
    public float lowerLimit = -200f;
    public float moveSpeed = 10f;

    private float currentHorizontalOffset = 0f;
    public float horizontalMovement = 700f; // UI units to move left/right
    public float horizontalSmoothSpeed = 5f;
    public float smoothSpeed = 0.1f;

    public RectTransform crosshairTransform;
    float currentY;

    void Start()
    {
        if (crosshairTransform != null)
        {
            currentY = crosshairTransform.anchoredPosition.y;
            currentHorizontalOffset = crosshairTransform.anchoredPosition.x;
        }
    }

    void Update()
    {
        // Vertical mouse movement
        float mouseY = Input.GetAxis("Mouse Y");
        currentY += mouseY * moveSpeed * Time.deltaTime;
        currentY = Mathf.Clamp(currentY, lowerLimit, upperLimit);

        float horizontalInput = Input.GetAxis("Horizontal");
        horizontalInput = -horizontalInput;
        float targetOffsetX = horizontalInput * horizontalMovement;
        currentHorizontalOffset = Mathf.Lerp(currentHorizontalOffset, targetOffsetX, Time.deltaTime * horizontalSmoothSpeed);
        // Apply the smoothed position to the crosshair
        Vector2 targetPosition = new Vector2(currentHorizontalOffset, currentY);
        crosshairTransform.anchoredPosition = Vector2.Lerp(crosshairTransform.anchoredPosition, targetPosition, Time.deltaTime * 10f);
    }
}