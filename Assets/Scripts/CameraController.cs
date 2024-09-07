using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform mCamera;

    private PlayerController player;

    private float horizontalTilt;
    private float verticalTilt;

    private float initialXRotation;

    [SerializeField]
    private float offset;

    [SerializeField]
    private bool useFloorNormal;

    [SerializeField]
    private float rotateSpeed;

    public float distanceFromPlayer = 5f;

    private float rotationX;
    private float rotationY;

    // Start is called before the first frame update
    void Start()
    {
        mCamera = transform.GetChild(0);                // Get Camera from child
        player = FindObjectOfType<PlayerController>();  // Find player

        initialXRotation = transform.eulerAngles.x;     // Store initial x rotation
    }

    void FixedUpdate()
    {
        verticalTilt = Input.GetAxis("Vertical");
        horizontalTilt = Input.GetAxis("Horizontal");

        // mCamera.transform.Rotate(Input.GetAxis("RightStickHorizontal") * Vector3.right * Time.deltaTime * rotateSpeed);

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
        rotationX = Mathf.Clamp(rotationX, -89f, 89f);

        // Create a rotation based on the input
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0f);

        // Set the camera position relative to the player
        Vector3 offset = new Vector3(0f, 0f, -distanceFromPlayer);
        transform.position = player.transform.position + rotation * offset;

        // Look at the player
        transform.LookAt(player.transform);

        player.Move(verticalTilt, horizontalTilt, transform.right);
    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        // Get forward vector minus the y component
        Vector3 vectorA = new Vector3(transform.forward.x, 0.0f, transform.forward.z);

        // Get target's velocity vector minus the y component
        Vector3 vectorB = new Vector3(player.rigidBody.velocity.x, 0.0f, player.rigidBody.velocity.z);

        // Find the angle between vectorA and vectorB
        float rotateAngle = Vector3.SignedAngle(vectorA.normalized, vectorB.normalized, Vector3.up);

        // Get the target's speed (maginitude) without the y component
        // Only set speed factor when vector A and B are almost facing the same direction
        float speedFactor = Vector3.Dot(vectorA, vectorB) > 0.0f ? vectorB.magnitude : 1.0f;

        // Rotate towards the angle between vectorA and vectorB
        // Use speedFactor so camera doesn't rotatate at a constant speed
        // Limit speedFactor to be between 1 and 2
        // transform.Rotate(Vector3.up, rotateAngle * Mathf.Clamp(speedFactor, 1.0f, 2.0f) * Time.deltaTime);

        // Position the camera behind target at a distance of offset
        transform.position = player.transform.position - (transform.forward * offset);
        transform.LookAt(player.transform.position);
    }
}
