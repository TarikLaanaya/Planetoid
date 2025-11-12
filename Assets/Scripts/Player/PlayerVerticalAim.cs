using UnityEngine;

public class PlayerVerticalAim : MonoBehaviour
{
    [Header("Movement Settings")]
    public float upperLimit = 200f;
    public float lowerLimit = -200f;
    public float moveSpeed = 10f;

    public RectTransform crosshairTransform;
    private float currentY;

    void Start()
    {
        currentY = crosshairTransform.anchoredPosition.y;
    }

    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        currentY += mouseY * moveSpeed * Time.deltaTime;
        currentY = Mathf.Clamp(currentY, lowerLimit, upperLimit);

        var pos = crosshairTransform.anchoredPosition;
        pos.y = currentY;
        crosshairTransform.anchoredPosition = pos;
    }
}