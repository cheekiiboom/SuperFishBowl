using TMPro;
using UnityEngine;

public class WaterDisplay : MonoBehaviour
{
    [SerializeField]
    private FishBowlWater fishBowlWater; // Reference to the FishBowlWater script

    [SerializeField]
    private TMP_Text waterText; // Reference to the TextMeshPro Text element

    // Update is called once per frame
    void Update()
    {
        // Get the current water amount and maximum water amount from the FishBowlWater script
        float currentWaterAmount = fishBowlWater.GetWaterAmount();
        float maxWaterAmount = 1000f; // Assuming max water is 1000 mL, or you can get it dynamically

        // Calculate the percentage of water remaining
        float percentage = (currentWaterAmount / maxWaterAmount) * 100f;

        // Update the TextMeshPro text to show the percentage with monospaced digits
        waterText.text = "<mspace=0.6em>" + percentage.ToString("F1") + "</mspace>%";
    }
}
