using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLevelFollower : MonoBehaviour
{
    [SerializeField]
    private FishBowlWater fishBowlWater; // Reference to the FishBowlWater script

    [SerializeField]
    private GameObject targetObject; // The object to follow

    [SerializeField]
    private GameObject floatingObject; // The object whose Y position will change

    [SerializeField]
    private float heightOffset = 1.5f; // Maximum offset above the target object's height
    [SerializeField]
    private float minHeightOffset = 0.5f; // Minimum offset above the target object's height

    private float maxWaterLevel; // Maximum water level from the FishBowlWater script

    // Start is called before the first frame update
    void Start()
    {
        if (fishBowlWater != null)
        {
            maxWaterLevel = fishBowlWater.GetWaterAmount(); // Get the maximum water level
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fishBowlWater != null && floatingObject != null && targetObject != null)
        {
            // Get the current water level
            float currentWaterLevel = fishBowlWater.GetWaterAmount();

            // Calculate the new Y offset based on the water level relative to the target object's height
            float normalizedWaterLevel = Mathf.Clamp01(currentWaterLevel / maxWaterLevel); // Normalize water level to range [0,1]
            float newYPosition = Mathf.Lerp(minHeightOffset, heightOffset, normalizedWaterLevel) + targetObject.transform.position.y;

            // Set the floating object's position while following the target object and keeping the floating object upright
            floatingObject.transform.position = new Vector3(
                targetObject.transform.position.x, // Follow target's X position
                newYPosition,                      // Set the Y position based on water level and relative to target object
                targetObject.transform.position.z  // Follow target's Z position
            );

            // Ensure the floating object remains upright (optional, depending on if you want rotation locked)
            floatingObject.transform.rotation = Quaternion.Euler(0, 0, 0); // Keep rotation upright
        }
    }
}
