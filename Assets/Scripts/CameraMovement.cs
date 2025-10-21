using UnityEngine;

public class CameraMovement : MonoBehaviour // Rotate the player to always face the same direction as the camera (Perhaps this should be renamed PlayerRotationToCamera)
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float dampening = 360f; // Degrees per second (higher is faster)

    Vector3 direction;

    void FixedUpdate()
    {
        direction = transform.position - cameraTransform.position; // Direction from camera to player
        direction.y = 0; // Keep only horizontal direction

        if (direction != Vector3.zero) // Avoid zero direction (this causes an error when player and camera have the same rotation (look rotation error when vector is zero))
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction); // Turn vector3 into a quaternion
            targetRotation = Quaternion.Euler(transform.rotation.x, targetRotation.eulerAngles.y, transform.rotation.z); //only change y rotation

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, dampening * Time.deltaTime); // Rotate towards that direction with dampening
            
        }
    }
}