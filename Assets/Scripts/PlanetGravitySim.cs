using UnityEngine;

public class PlanetGravitySim : MonoBehaviour
{
    [SerializeField] private Transform planetTransform;
    private float distanceFromPlanetCenter;

    void Awake()
    {
        distanceFromPlanetCenter = Vector3.Distance(transform.position, planetTransform.position); // Set initial distance from planet center
    }

    void FixedUpdate()
    {
        Vector3 directionToPlanet = (transform.position - planetTransform.position).normalized; // Direction from player to planet center (normalize to get only direction)

        // Set rotation to planet surface normal
        transform.rotation = Quaternion.FromToRotation(transform.up, directionToPlanet) * transform.rotation;

        // Adjust position to stay on planet surface
        transform.position = planetTransform.position + directionToPlanet * distanceFromPlanetCenter;
    }
}
