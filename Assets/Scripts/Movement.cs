using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float playerHeight = 2f;

    [Header("Orientation")]
    [SerializeField] private Transform orientation;

    [Header("Movement")]
    [SerializeField, Range(1f, 20f)] private float moveSpeed = 6f; // Speed of the player
    [SerializeField, Range(0.1f, 1f)] private float airMultiplier = 0.4f; // Multiplier for movement while in the air
    [SerializeField, Range(1f, 20f)] private float movementMultiplier = 10f; // Multiplier for general movement

    [Header("Sprinting")]
    [SerializeField, Range(1f, 10f)] private float walkSpeed = 4f; // Speed while walking
    [SerializeField, Range(1f, 20f)] private float sprintSpeed = 6f; // Speed while sprinting
    [SerializeField, Range(1f, 20f)] private float acceleration = 10f; // Acceleration when changing speed

    [Header("Jumping")]
    [SerializeField, Range(1f, 20f)] public float jumpForce = 5f; // Force applied when jumping

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space; // Key to trigger jumping
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift; // Key to trigger sprinting

    [Header("Drag")]
    [SerializeField, Range(1f, 20f)] private float groundDrag = 6f; // Drag when grounded
    [SerializeField, Range(0.1f, 5f)] private float airDrag = 2f; // Drag when in the air

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck; // Reference to the ground check object
    [SerializeField] private LayerMask groundMask; // Layer mask for ground detection
    [SerializeField, Range(0.1f, 2f)] private float groundDistance = 0.2f; // Distance to check for ground

    public bool isGrounded { get; private set; } // Flag indicating if the player is grounded

    private Vector3 moveDirection; // Direction of player movement
    private Vector3 slopeMoveDirection; // Direction of movement on slopes

    private Rigidbody rb; // Reference to the Rigidbody component

    private RaycastHit slopeHit; // Information about the slope

    private bool OnSlope()
    {
        // Check if the player is on a slope
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            return slopeHit.normal != Vector3.up; // Return true if the normal is not pointing up (on a slope)
        }
        return false;
    }

    private void Start()
    {
        // Get the Rigidbody component and freeze rotation
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // Check if the player is grounded using a sphere cast
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Handle player input
        MyInput();

        // Control drag based on grounded state
        ControlDrag();

        // Control speed based on input and grounded state
        ControlSpeed();

        // Check for jump input and apply jump force if grounded
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        // Project the movement direction on the slope to handle slope movement
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    private void MyInput()
    {
        // Get raw input for movement
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        // Calculate movement direction based on orientation
        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    private void Jump()
    {
        // Apply upward force for jumping
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void ControlSpeed()
    {
        // Control movement speed based on input and grounded state
        if (Input.GetKey(sprintKey) && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    private void ControlDrag()
    {
        // Control drag based on grounded state
        rb.drag = isGrounded ? groundDrag : airDrag;
    }

    private void FixedUpdate()
    {
        // Move the player based on the calculated forces
        MovePlayer();
    }

    private void MovePlayer()
    {
        // Apply forces based on movement, grounded state, and slope conditions
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }
}
