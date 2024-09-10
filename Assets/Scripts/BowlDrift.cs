using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlDrift : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rigidBody; // The Rigidbody of the bowl

    [SerializeField]
    private Transform fishTransform; // The transform of the fish

    [SerializeField]
    private float driftFactor = 0.1f; // Factor to control how much the fish drifts

    private Vector3 previousVelocity;

    // Start is called before the first frame update
    void Start()
    {
        previousVelocity = rigidBody.velocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Calculate the acceleration in world space
        Vector3 currentVelocity = rigidBody.velocity;
        Vector3 acceleration = (currentVelocity - previousVelocity) / Time.fixedDeltaTime;
        previousVelocity = currentVelocity;

        // Convert world-space acceleration to the local space of the bowl
        Vector3 localAcceleration = transform.InverseTransformDirection(acceleration);

        // Inverse the direction of the local acceleration to simulate the fish drifting opposite
        Vector3 localDrift = -localAcceleration * driftFactor;

        // Move the fish along the local axes by adjusting its local position
        fishTransform.localPosition = Vector3.Lerp(fishTransform.localPosition, localDrift, Time.fixedDeltaTime);
    }
}
