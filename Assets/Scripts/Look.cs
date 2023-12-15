using UnityEngine;

public class Look : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Transform spine; // Reference to the spine object

    private float rotationX = 0f;

    void Start()
    {
        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Get the mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotate the player horizontally based on mouse input
        Vector3 rotation = transform.localEulerAngles;
        rotation.y += mouseX * rotationSpeed;
        transform.localEulerAngles = rotation;

        // Rotate the spine vertically based on mouse input
        rotationX -= mouseY * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, -40f, 40f); // Limit the vertical rotation

        // Apply rotation to the spine using Quaternion
        Quaternion spineRotation = Quaternion.Euler(rotationX, 0f, rotationX);
        spine.localRotation = spineRotation;
    }
}