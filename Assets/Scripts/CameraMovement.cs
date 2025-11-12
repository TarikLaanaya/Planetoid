using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform starship;
    public Vector3 baseOffset = new Vector3();
    public float smoothSpeed = 0.1f;
    public float speedOffsetMultiplier = 2f;

    private float targetSpeedOffset = 0f;
    private float currentSpeedOffset = 0f;

    public float horizontalMovement = 0.5f;
    public float horizontalSmoothSpeed = 5f;

    private float targetHorizontalOffset = 0f;
    private float currentHorizontalOffset = 0f;

    void LateUpdate()
    {
        float inputVertical = Input.GetAxis("Vertical");
        float inputHorizontal = Input.GetAxis("Horizontal");

        targetSpeedOffset = Mathf.Lerp(targetSpeedOffset, inputVertical * speedOffsetMultiplier, Time.deltaTime * 5f);
        currentSpeedOffset = Mathf.Lerp(currentSpeedOffset, targetSpeedOffset, Time.deltaTime * 5f);
        Vector3 speedOffset = starship.transform.forward * currentSpeedOffset;
        // This function offsets the camera based on the ships speed (forwards and backwards)
        // Using Lerp it smoothly moves the camera between the current position to the 'speedOffsetMultiplier' over time (5f)
        // It then pushes this offset into a 3 dimensional vector to be applied with the horizontal movement

        targetHorizontalOffset = Mathf.Lerp(targetHorizontalOffset, inputHorizontal * horizontalMovement, Time.deltaTime * horizontalSmoothSpeed);
        currentHorizontalOffset = Mathf.Lerp(currentHorizontalOffset, targetHorizontalOffset, Time.deltaTime * horizontalSmoothSpeed);
        Vector3 horizontalOffset = starship.transform.right * currentHorizontalOffset;
        // The same logic using Lerp from the horizontal input is used to push the offset to the 3D vector.

        Vector3 desiredPosition = starship.position + starship.TransformDirection(baseOffset) + speedOffset + horizontalOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        //  Lerp is used one more time to apply the final position which is a combination of the offset from speed and horizontal movement
    }
}
