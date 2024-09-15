using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBowlWater : MonoBehaviour
{
    [SerializeField]
    private float maxWaterAmount = 1000f; // Starting amount of water in mL

    [SerializeField]
    private float waterAmount; // Current amount of water in mL

    [SerializeField]
    private float baseLeakRate = 1f; // Base leak rate (mL per second)

    [SerializeField]
    private float collisionLeakMultiplier = 5f; // Additional leak multiplier on collisions

    private bool isLeaking = false; // Flag to track whether the bowl is leaking

    private PlayerAudio PlayerAudio;
    private const float BumpSoundThreshold = 10f; // Adjust this value as needed
    private const float GlassSoundThreshold = 120f;

    // Start is called before the first frame update
    void Start()
    {
        waterAmount = maxWaterAmount; // Initialize the bowl with max water
        PlayerAudio = GetComponent<PlayerAudio>();
    }

    // Update is called once per frame
    void Update()
    {
        // Only leak if the camera is not panning
        if (!CameraPanOrTeleportOnTrigger.isCameraPanning && isLeaking && waterAmount > 0)
        {
            // Reduce water by base leak rate per second
            DecreaseWater(baseLeakRate * Time.deltaTime);
        }

        if (waterAmount <= 0 && isLeaking)
        {
            PlayerAudio.PlaySound("glass");
            isLeaking = false;
            return;
        }
    }

    // Method to decrease water by a given amount (in mL)
    public void DecreaseWater(float amount)
    {
        waterAmount -= amount;

        // Clamp waterAmount so it doesn't go below 0
        waterAmount = Mathf.Clamp(waterAmount, 0f, maxWaterAmount);
    }

    // Method to be called on collision to reduce water faster
    private void OnCollisionEnter(Collision collision)
    {
        // Apply extra water loss when bumping into things
        if (waterAmount > 0 && collision.gameObject.layer != 9)
        {
            // Only decrease water if the camera is not panning
            if (!CameraPanOrTeleportOnTrigger.isCameraPanning)
            {
                if (!isLeaking)
                {
                    StartLeaking();
                    return;
                }
                float impactStrength = collision.impulse.magnitude
                    * (collision.relativeVelocity.magnitude + collision.relativeVelocity.y)
                    * (collision.relativeVelocity.magnitude + collision.relativeVelocity.y) / 8; // Use the strength of the impact
                DecreaseWater(collisionLeakMultiplier * impactStrength); // Decrease water based on the impact
                
                if (collisionLeakMultiplier * impactStrength > GlassSoundThreshold)
                {
                    Debug.Log(collisionLeakMultiplier * impactStrength);
                    PlayerAudio.SetVolumeAndPitch(1f, 1f);
                    PlayerAudio.PlaySound("glass");
                    return;
                }

                if (collisionLeakMultiplier * impactStrength < BumpSoundThreshold) return;
                Debug.Log(collisionLeakMultiplier * impactStrength);

                float normalizedVolume = Mathf.Clamp01(collisionLeakMultiplier * impactStrength / 15f); // Assuming max impact is 15
                float minPitch = 0.8f;
                float maxPitch = 1.2f;

                // Set the volume and pitch based on the velocity
                PlayerAudio.SetVolumeAndPitch(normalizedVolume, Mathf.Lerp(minPitch, maxPitch, normalizedVolume));
                PlayerAudio.PlaySound("bump");
            }
        }
    }

    // Method to get the current amount of water left in the bowl
    public float GetWaterAmount()
    {
        return waterAmount;
    }

    // Optional method to stop the bowl from leaking (for future use)
    public void StopLeaking()
    {
        isLeaking = false;
    }

    // Optional method to resume leaking
    public void StartLeaking()
    {
        isLeaking = true;
    }
}
