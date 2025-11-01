using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float speed = 10f;
    public float rotateSpeed = 200f;
    public float dampening = 5.0f; // The higher, the smoother the tracking

    private Rigidbody rb;
    private Transform target;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject enemy = GameObject.FindGameObjectWithTag("enemy");
        if (enemy != null)
        {
            target = enemy.transform;
        }
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 direction = (target.position - rb.position).normalized;
        Vector3 rotateAmount = Vector3.Cross(transform.forward, direction);

        // Apply dampening by reducing torque
        rb.angularVelocity = rotateAmount * rotateSpeed * Time.fixedDeltaTime / dampening;

        // Move forward constantly
        rb.linearVelocity = transform.forward * speed;
    }
}
