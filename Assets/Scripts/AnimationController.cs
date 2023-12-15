using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Weapon weapon;

    [Header("Movement Input")]
    [SerializeField, Range(-1f, 1f)] private float verticalInputThreshold = 0.1f;
    [SerializeField, Range(-1f, 1f)] private float horizontalInputThreshold = 0.1f;

    void Update()
    {
        // Get the input for movement
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        // Check movement direction
        bool isMovingForward = verticalInput > verticalInputThreshold;
        bool isMovingBackward = verticalInput < -verticalInputThreshold;
        bool isStrafingLeft = horizontalInput < -horizontalInputThreshold;
        bool isStrafingRight = horizontalInput > horizontalInputThreshold;

        // Check if keys are pressed
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isFiring = Input.GetMouseButton(0);
        bool isReloading = weapon.isReloading;

        // Set animation parameters based on input
        animator.SetBool("isRunning", isRunning && isMovingForward && !isMovingBackward && !isFiring && !isReloading);
        animator.SetBool("isWalking", !isRunning && isMovingForward && !isFiring && !isReloading);
        animator.SetBool("isWalkingBack", !isRunning && isMovingBackward && !isFiring && !isReloading);
        animator.SetBool("isRunningBack", isRunning && isMovingBackward && !isMovingForward && !isFiring && !isReloading);
        animator.SetBool("StrafeLeft", isStrafingLeft && !isRunning && !isFiring && !isReloading);
        animator.SetBool("StrafeRight", isStrafingRight && !isRunning && !isFiring && !isReloading);
        animator.SetBool("isIdle", !IsPlayerMoving() && !isFiring && !isReloading);
        animator.SetBool("isJumping", Input.GetKeyDown(KeyCode.Space) && isMovingForward);
        animator.SetBool("isFiring", isFiring && !IsPlayerMoving() && !isReloading);
        animator.SetBool("isWalkingAndFiring", isFiring && IsPlayerMoving() && !isReloading);
        animator.SetBool("isReloading", isReloading);
    }

    // Check if the player is moving
    bool IsPlayerMoving()
    {
        return Input.GetAxis("Vertical") != 0.0f || Input.GetAxis("Horizontal") != 0.0f;
    }
}
