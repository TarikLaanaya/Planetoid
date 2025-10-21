using UnityEngine;

public class Target : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(collision.relativeVelocity * 10f, ForceMode.Impulse);
        }
    }
}
