using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody rigidBody;

    [HideInInspector]
    public Vector3 floorNormal;

    [SerializeField]
    public LayerMask whatIsGround;
    [SerializeField]
    private float groundCheckRadius;

    public float maxSpeed;

    [SerializeField]
    private float moveForce;

    [SerializeField]
    private float jumpForce; // Jump force

    public bool canJump = true; // To prevent mid-air jumping

    private LineRenderer lineRenderer;
    public float lineLengthMultiplier = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();  // Get rigidbody component

        // hack
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, transform.position);

        // Set the position of the line's end (in the direction of the velocity)
        Vector3 velocityDirection = rigidBody.velocity.normalized;  // Direction of the velocity
        float velocityMagnitude = rigidBody.velocity.magnitude;     // Length of the velocity

        // Set the end position of the line based on the velocity and multiplier
        lineRenderer.SetPosition(1, transform.position + velocityDirection * velocityMagnitude * lineLengthMultiplier);

        // Player should jump
        if (Input.GetButton("Jump") && OnGround())
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpForce, rigidBody.velocity.z); // Add upward force
        }
    }


    public void Move(float verticalTilt, float horizontalTilt, Vector3 right)
    {
        // Only apply movement when the player is grounded
        if (OnGround())
        {
            CalculateFloorNormal();

            // No input from player
            if (horizontalTilt == 0.0f && verticalTilt == 0.0f && rigidBody.velocity.magnitude > 0.0f)
            {
                rigidBody.velocity = Vector3.Lerp(rigidBody.velocity, Vector3.zero, moveForce * 0.1f * Time.deltaTime); // Slow down
            }
            else
            {
                // Get a direction perpendicular to the camera's right vector and the floor's normal (The forward direction)
                Vector3 forward = Vector3.Cross(right, floorNormal);

                // Apply moveForce scaled by verticalTilt in the forward direction
                Vector3 forwardForce = moveForce * verticalTilt * forward;
                // Apply moveForce scaled by horizontalTilt in the right direction
                Vector3 rightForce = moveForce * horizontalTilt * right;

                Vector3 forceVector = forwardForce + rightForce;

                rigidBody.AddForce(forceVector);
            }
        }

        /*if (OnGround())
            Debug.Log("Grounded");
        else
            Debug.Log("Airborne");*/
    }

    public bool OnGround()
    {
        return Physics.CheckSphere(transform.position + Vector3.down * 0.1f, groundCheckRadius, whatIsGround);
    }

    private void CalculateFloorNormal()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, whatIsGround))
        {
            floorNormal = hit.normal;
        }
    }
}
