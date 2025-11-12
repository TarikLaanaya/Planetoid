using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float boostSpeed;
    [SerializeField] private ParticleSystem[] boostParticles;
    [SerializeField] private float accelDeccel;
    [SerializeField] private float rotationDampening;
    [SerializeField] private Transform playerModelTransform;

    private InputManager inputManager;
    
    private Rigidbody rb;
    Vector3 moveDirection;
    private bool boosting = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        float moveX = inputManager.moveX;
        float moveZ = inputManager.moveZ;

        moveDirection = transform.right * moveX + transform.forward * moveZ; // Calculate movement direction based on player orientation

        float mouseX = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.up * mouseX); // Rotate player root based on mouse movement

        // Check if boost button down
        if (inputManager.boostButtonHeld)
        {
            boosting = true;
            SetParticleSystemActive(true);
        }
        else
        {
            boosting = false;
            SetParticleSystemActive(false);
        }
    }

    void FixedUpdate()
    {
        // --- Boost --- //

        Vector3 boostMagnitude = Vector3.zero; // Set default value

        if (boosting)
        {
            boostMagnitude = playerModelTransform.forward * boostSpeed; // If we are boosting add a boost magnitude to the main movement logic
        }

        // --- Movement --- //
        
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, moveDirection.normalized * maxSpeed + boostMagnitude, accelDeccel * Time.deltaTime); // Apply movement with max speed
    }

    void LateUpdate() //We do this in late update to prevent jittering with unsynced player root and model
    {
        // Rotate the player model to match the player root rotation (Smoothly to simulate space ship turning)
        playerModelTransform.rotation = Quaternion.Slerp(playerModelTransform.rotation, transform.rotation, rotationDampening * Time.deltaTime);

        playerModelTransform.position = transform.position; // Sync player model to player root
    }

    void SetParticleSystemActive(bool active)
    {
        foreach (ParticleSystem boostParticle in boostParticles)
        {
            boostParticle.gameObject.SetActive(active);
        }
    }
}