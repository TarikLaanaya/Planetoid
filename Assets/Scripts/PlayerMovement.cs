using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    private Rigidbody rb;
    Vector3 moveDirection;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = transform.right * moveX + transform.forward * moveZ; // Calculate movement direction based on player orientation
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveDirection.normalized * maxSpeed; // Apply movement with max speed
    }
}