using UnityEngine;

public class CameraMovement : MonoBehaviour // Rotate the player to always face the same direction as the camera (Perhaps this should be renamed PlayerRotationToCamera)
{
    [SerializeField] private Transform cameraTranform;
    [SerializeField] private float dampening = 360f; // Degrees per second (higher is faster)

    void Update()
    {
        Vector3 direction = transform.position - cameraTranform.position; // Direction from camera to player
        direction.y = 0; // Keep only horizontal direction

        if (direction != Vector3.zero) // Avoid zero direction (this causes an error when player and camera have the same rotation (look rotation error when vector is zero))
        {
            Quaternion toRotation = Quaternion.LookRotation(direction); // Create a rotation looking in the direction
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, dampening * Time.deltaTime); // Rotate towards that direction with dampening
        }
    }
}