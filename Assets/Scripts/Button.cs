using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject[] Water; // The object to enable when all buttons are pressed
    public GameObject[] OtherButtons; // The other buttons that need to be pressed
    private Vector3 pressedPositionOffset = new Vector3(0, 0, -0.09f); // How much the button will move down
    public Material pressedMaterial; // Optional: Material to change to when pressed
    private Vector3 initialPosition; // Store the initial position of the button
    private Material originalMaterial; // Store the original material of the button
    private bool isPressed = false; // Track if this button has been pressed

    private void Start()
    {
        // Store the initial position of the button
        initialPosition = transform.localPosition;

        // Store the original material if using materials
        if (GetComponent<Renderer>() != null)
        {
            originalMaterial = GetComponent<Renderer>().material;
        }
    }

    // Method called when the player triggers the button
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.tag == "Player" && !isPressed)
        {
            isPressed = true; // Mark this button as pressed
            PressButton(); // Visually press the button
            CheckAllButtonsPressed(); // Check if all buttons are pressed
        }
    }

    // Method to visually press the button (e.g., move it down and change material)
    private void PressButton()
    {
        // Move the button down
        transform.localPosition = initialPosition + pressedPositionOffset;

        // Change the material if one is provided
        if (pressedMaterial != null && GetComponent<Renderer>() != null)
        {
            GetComponent<Renderer>().material = pressedMaterial;
        }
    }

    // Method to check if all buttons have been pressed
    private void CheckAllButtonsPressed()
    {
        // Check if all other buttons are pressed
        foreach (var button in OtherButtons)
        {
            Button buttonScript = button.GetComponent<Button>();
            if (!buttonScript.isPressed)
            {
                return; // If any button isn't pressed, exit
            }
        }

        // If all buttons are pressed, re-enable the Water object
        foreach (var water in Water)
            water.SetActive(true);
    }
}