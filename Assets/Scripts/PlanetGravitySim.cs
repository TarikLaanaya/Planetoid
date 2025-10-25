using UnityEngine;

public class PlanetGravitySim : MonoBehaviour
{
    public Transform planetTransform;
    private float distanceFromPlanetCenter;

    void Start()
    {
        distanceFromPlanetCenter = Vector3.Distance(transform.position, planetTransform.position); // Set initial distance from planet center
    }

    void FixedUpdate()
    {
        Vector3 directionToPlanet = (transform.position - planetTransform.position).normalized; // Direction from player to planet center (normalize to get only direction)

        // Set rotation to planet surface normal
        transform.rotation = Quaternion.FromToRotation(transform.up, directionToPlanet) * transform.rotation; 
        //This code looks simple but does a lot:
        //1. Quaternion.FromToRotation creates a rotation that rotates from a to b (for example from 0 degrees to 90 degrees = 90 degrees)
        //2. We times that rotation by the current rotation of the player to get the new rotation aligned to the planet surface
        // this works because with quaternions order matters (Rot A * Rot B = apply B then A)
        // So were essentialy saying "Take my current rotation and then rotate it by the rotation Qutaternion.FromToRotation gives us"

        // Adjust position to stay on planet surface
        transform.position = planetTransform.position + directionToPlanet * distanceFromPlanetCenter; //This forces the player to be "distanceFromPlanetCenter" away from planet center
    }
}
