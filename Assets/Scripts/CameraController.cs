using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController player;
    private Renderer playerRenderer; // Reference to the player's renderer
    private Renderer[] childRenderers; // Reference to the children's renderers

    private float horizontalTilt;
    private float verticalTilt;

    private float initialXRotation;

    [SerializeField]
    private bool useFloorNormal;

    [SerializeField]
    private float rotateSpeed;

    [SerializeField]
    private float smoothSpeed = 5f;

    private float distanceFromPlayer;
    public float minDistance = 1f;  // Minimum distance the camera can get to the player
    public float maxDistance = 5f;  // Maximum distance the camera can be from the player
    public LayerMask obstacleLayers;  // Layers to detect as obstacles
    public float collisionBuffer = 0.2f;  // Small buffer to prevent clipping

    private float rotationX;
    private float rotationY;

    private float initialPlayerOpacity = 0.3f;  // Initial player opacity
    private float initialChildOpacity = 1f;     // Initial child opacity

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();  // Find player
        playerRenderer = player.GetComponent<Renderer>(); // Get the player's renderer
        childRenderers = player.GetComponentsInChildren<Renderer>(); // Get the children's renderers

        initialXRotation = transform.eulerAngles.x;     // Store initial x rotation
    }

    void FixedUpdate()
    {
        verticalTilt = Input.GetAxis("Vertical");
        horizontalTilt = Input.GetAxis("Horizontal");

        // Mouse rotation
        float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

        // Right Stick rotation (assumes RightStickHorizontal and RightStickVertical are set up in Input Manager)
        float rightStickX = Input.GetAxis("RightStickHorizontal") * rotateSpeed * Time.deltaTime;
        float rightStickY = Input.GetAxis("RightStickVertical") * rotateSpeed * Time.deltaTime;

        // Combine inputs
        rotationX -= mouseY + rightStickY; // Invert for natural feel
        rotationY += mouseX + rightStickX;

        // Clamp vertical rotation to avoid flipping
        rotationX = Mathf.Clamp(rotationX, -85f, 85f);

        // Create a rotation based on the input
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0f);

        // Calculate the intended camera position without obstacle adjustments
        Vector3 intendedCameraPosition = player.transform.position + rotation * new Vector3(0f, 0f, -maxDistance);

        // Adjust camera distance based on obstacles
        distanceFromPlayer = CalculateCameraDistance(intendedCameraPosition);

        // Set the camera position relative to the player, clamping it to obstacles
        Vector3 offset = new Vector3(0f, 0f, -distanceFromPlayer);
        Vector3 targetPosition = player.transform.position + rotation * offset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);

        // Look at the player
        transform.LookAt(player.transform);

        // Adjust both the player's and children's opacity based on the camera's distance
        UpdateOpacity(distanceFromPlayer);

        // Move the player based on the camera's position
        player.Move(verticalTilt, horizontalTilt, transform.right);
    }

    // Method to calculate camera distance based on obstacles
    float CalculateCameraDistance(Vector3 intendedPosition)
    {
        // Default the camera distance to max distance
        float adjustedDistance = maxDistance;

        // Create a ray from the player to the intended camera position
        Vector3 direction = (intendedPosition - player.transform.position).normalized;
        Ray ray = new Ray(player.transform.position, direction);

        RaycastHit hit;

        // Perform the raycast to check for obstacles
        if (Physics.Raycast(ray, out hit, maxDistance, obstacleLayers))
        {
            // If an obstacle is detected, adjust the camera to the hit point plus a small buffer
            adjustedDistance = Mathf.Clamp(hit.distance - collisionBuffer, minDistance, maxDistance);
        }

        return adjustedDistance;
    }

    // Update the player's and children's opacity based on the distance from the camera
    void UpdateOpacity(float distance)
    {
        // Calculate the opacity based on the camera's distance (closer -> more transparent)
        float t = Mathf.InverseLerp(minDistance, maxDistance, distance);
        float opacity = Mathf.Lerp(0f, initialPlayerOpacity, t);

        // Update the player's material opacity
        SetMaterialOpacity(playerRenderer, opacity);

        // Update all children's opacity as well
        foreach (Renderer childRenderer in childRenderers)
        {
            SetMaterialOpacity(childRenderer, opacity);
        }
    }

    // Helper function to set the material opacity
    void SetMaterialOpacity(Renderer renderer, float opacity)
    {
        if (renderer != null && renderer.material.HasProperty("_Color"))
        {
            Color color = renderer.material.color;
            color.a = opacity;
            renderer.material.color = color;
        }
    }
}
