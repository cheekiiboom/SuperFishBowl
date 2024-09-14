using System;
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
    public bool lineRendererEnabled = true;

    public bool inputEnabled { get; set; } = false;

    private PlayerAudio PlayerAudio;
    private AudioSource PlayerAudioSource;
    private AudioSource RollAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();  // Get rigidbody component

        // hack
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        lineRenderer = GetComponent<LineRenderer>();

        PlayerAudio = GetComponent<PlayerAudio>();
        PlayerAudioSource = PlayerAudio.GetAudioSource();
        RollAudioSource = PlayerAudio.GetRollAudioSource();

        PlayerAudio.PlayRoll();
    }

    private void Update()
    {
        UpdateLineRenderer();

        // Player should jump
        Jump();
        HandleRollAudio();
    }

    // Change roll sound based on speed and ground check
    private void HandleRollAudio()
    {
        if (!OnGround())
        {
            StartCoroutine(FadeRollToZero());
            /*RollAudioSource.volume = 0f;*/
            return;
        }

        float velocityMagnitude = rigidBody.velocity.magnitude; 

        /*if (velocityMagnitude > 1f)
        {*/
            // Normalize the velocity to a range suitable for volume and pitch
            float normalizedVolume = Mathf.Clamp01(velocityMagnitude / 15f); // Assuming max speed is 10
            float minPitch = 0.8f;
            float maxPitch = 1.2f;

            // Set the volume and pitch based on the velocity
            PlayerAudio.SetRollVolumeAndPitch(normalizedVolume, Mathf.Lerp(minPitch, maxPitch, normalizedVolume));

            // Ensure the audio is playing in case it stops
            if (!RollAudioSource.isPlaying)
            {
                PlayerAudio.PlayRoll();
            }
        /*}
        else
        {
            StartCoroutine(FadeRollToZero()); // Optionally fade out the audio if the velocity is low
        }*/
    }

    private IEnumerator FadeRollToZero(float duration = 0.14f)
    {
        float startVolume = RollAudioSource.volume;

        // Gradually reduce the volume to zero over the specified duration
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            RollAudioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null; // Wait for the next frame
        }

        // Ensure the volume is set to zero after fading
        RollAudioSource.volume = 0f;
    }


    public void UpdateLineRenderer()
    {
        if (!lineRendererEnabled) 
            { return; }

        lineRenderer.SetPosition(0, transform.position);

        // Set the position of the line's end (in the direction of the velocity)
        Vector3 velocityDirection = rigidBody.velocity.normalized;  // Direction of the velocity
        float velocityMagnitude = rigidBody.velocity.magnitude;     // Length of the velocity

        // Set the end position of the line based on the velocity and multiplier
        lineRenderer.SetPosition(1, transform.position + velocityDirection * velocityMagnitude * lineLengthMultiplier);
    }

    public void Jump()
    {
        if (Input.GetButton("Jump") && OnGround() && inputEnabled && rigidBody.velocity.y < 1)
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpForce, rigidBody.velocity.z); // Add upward force
            PlayerAudio.PlaySound("jump", withVariation: true);
        }
    }


    public void Move(float verticalTilt, float horizontalTilt, Vector3 right)
    {
        // Only apply movement when the player is grounded
        
        if (OnGround() && inputEnabled)
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
