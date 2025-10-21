using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float sprintSpeed = 10f;
    public float jumpForce = 8f;
    public float slideSpeed = 12f;
    public float wallRunSpeed = 7f;
    public float wallJumpForce = 2f;
    public float mouseSensitivity = 200f;
    public LayerMask wallLayer;
    public Camera Camera;

    private Rigidbody rb;
    private bool isGrounded;
    //private bool isWallRunning;
    private Transform cam;
    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;  // Prevents unwanted rotation
        cam = Camera.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        PlayerMovement();
        Jump();
        Slide();
        WallRun();
    }

    private void PlayerMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        rb.linearVelocity = new Vector3(moveDirection.x * currentSpeed, rb.linearVelocity.y, moveDirection.z * currentSpeed);
    }

    private void Jump()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);  // Check if player is grounded

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }

    private void Slide()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            rb.linearVelocity = transform.forward * slideSpeed;
        }
    }

    private void WallRun()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.right, out hit, 1f, wallLayer) || 
                Physics.Raycast(transform.position, -transform.right, out hit, 1f, wallLayer))
            {
                //isWallRunning = true;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                rb.AddForce(transform.forward * wallRunSpeed, ForceMode.Acceleration);

                // Wall Jump
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Vector3 jumpDirection = hit.normal + Vector3.up;
                    rb.linearVelocity = jumpDirection * wallJumpForce;
                }
            }
            else
            {
                //isWallRunning = false;
            }
        }
    }

    private void CameraController()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}