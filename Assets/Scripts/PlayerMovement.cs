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

        moveDirection = new Vector3(moveX, 0, moveZ);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveDirection.x * maxSpeed, rb.linearVelocity.y, moveDirection.z * maxSpeed); //Set velocity based on input and max speed
    }
}