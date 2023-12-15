using UnityEngine;

public class UISway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField, Range(1f, 20f)] private float swayAmount = 10f; // Adjust this value to control the amount of sway
    [SerializeField, Range(0.1f, 5f)] private float swaySpeed = 2f; // Adjust this value to control the speed of sway

    private RectTransform rectTransform;
    private Vector3 originalPosition;

    void Start()
    {
        // Get the RectTransform component of the UI element
        rectTransform = GetComponent<RectTransform>();

        // Store the original position of the UI element
        originalPosition = rectTransform.anchoredPosition;
    }

    void Update()
    {
        // Get mouse input (or any other input) to control sway
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calculate sway based on input and time
        float swayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount * mouseX;
        float swayY = Mathf.Sin(Time.time * swaySpeed) * swayAmount * mouseY;

        // Create a vector for the sway offset
        Vector3 swayOffset = new Vector3(swayX, swayY, 0f);

        // Apply sway to the UI element's position
        rectTransform.anchoredPosition = originalPosition + swayOffset;
    }
}