using UnityEngine;

public class EnemyAINavigation : MonoBehaviour
{
    [SerializeField] private Transform planetTransform;

    public Vector3 TargetDirection(Vector3 targetPosition)
    {
        // Raw direction to target (goes through planet)
        Vector3 dirToTarget = targetPosition - transform.position;

        // Get the direction between the planet center and the enemy AI (planets surface normal)
        Vector3 planetSurfaceNormal = (transform.position - planetTransform.position).normalized;
        
        // Project the direction to target onto the planet surface normal to get a horizontal direction (essentially deleting the vertical axis)
        Vector3 horizontalDirToTarget = Vector3.ProjectOnPlane(dirToTarget, planetSurfaceNormal).normalized;

        return horizontalDirToTarget;
    }
}
