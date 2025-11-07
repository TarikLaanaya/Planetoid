using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float speed = 10f;
    public float rotateSpeed = 200f;
    public float dampening = 5.0f;

    private Rigidbody rb;

    public Transform target;        

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 direction = (target.position - rb.position).normalized;
        Vector3 rotateAmount = Vector3.Cross(transform.forward, direction);

        rb.angularVelocity = rotateAmount * rotateSpeed * Time.fixedDeltaTime / dampening;

        // Move forward constantly
        rb.linearVelocity = transform.forward * speed;
    }
}
