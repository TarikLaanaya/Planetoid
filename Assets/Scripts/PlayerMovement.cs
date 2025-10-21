using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float accelDeccel;
    [SerializeField] private float rotationDampening;
    [SerializeField] private Transform playerModelTransform;
    
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

        float mouseX = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.up * mouseX); // Rotate player root based on mouse movement
        
    }

    void FixedUpdate()
    {
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, moveDirection.normalized * maxSpeed, accelDeccel * Time.deltaTime); // Apply movement with max speed
    }

    void LateUpdate() //We do this in late update to prevent jittering with unsynced player root and model
    {
        // Rotate the player model to match the player root rotation (Smoothly to simulate space ship turning)
        playerModelTransform.rotation = Quaternion.Slerp(playerModelTransform.rotation, transform.rotation, rotationDampening * Time.deltaTime);

        playerModelTransform.position = transform.position; // Sync player model to player root
    }
}