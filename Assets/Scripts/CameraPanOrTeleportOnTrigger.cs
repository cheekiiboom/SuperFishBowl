using UnityEngine;
using System.Collections.Generic; // Needed for using List

public class CameraPanOrTeleportOnTrigger : MonoBehaviour
{
    public List<GameObject> targetObjects;    // The list of objects the camera will move to
    public Camera playerCamera;               // The player's camera
    public MonoBehaviour playerCameraScript;  // The script controlling the player's camera (e.g., a first-person controller)
    public float cameraDelay = 3f;            // Time the camera stays at each target position
    public float panDelay = 1f;               // Delay before the camera starts panning or teleporting
    public float freezePanDelay = 1f;         // Delay before panning starts after the game is frozen
    public float panSpeed = 1f;               // Speed at which the camera pans between targets
    public bool panCamera = false;            // If true, the camera will pan; otherwise, it will teleport
    public bool freezeGame = true;            // If true, the game will freeze when triggered

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;

    public static bool isCameraPanning = false; // This will be checked by other scripts

    private bool isTriggered = false;
    private int currentTargetIndex = 0;       // To keep track of the current target

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;

            originalCameraPosition = playerCamera.transform.position;
            originalCameraRotation = playerCamera.transform.rotation;

            // Start delay before panning or teleporting to the first target
            StartCoroutine(StartCameraMoveAfterDelay());
        }
    }

    public System.Collections.IEnumerator StartCameraMoveAfterDelay()
    {
        yield return new WaitForSecondsRealtime(panDelay); // Wait for the specified delay

        // Disable the player's camera script
        playerCameraScript.enabled = false;

        // Optionally freeze the game
        if (freezeGame)
        {
            FreezeGame();
        }

        // Introduce delay after game is frozen, before panning starts
        yield return new WaitForSecondsRealtime(freezePanDelay);

        // Move to the first target
        MoveToNextTarget();
    }

    void MoveToNextTarget()
    {
        if (currentTargetIndex >= targetObjects.Count)
        {
            // If all targets have been visited, reset the camera (pan or teleport back)
            ResetCameraPosition();
            return;
        }

        GameObject targetObject = targetObjects[currentTargetIndex];

        if (panCamera)
        {
            StartCoroutine(PanToTarget(targetObject));
        }
        else
        {
            TeleportToTarget(targetObject);
            StartCoroutine(ResetCameraAfterDelay());
        }
    }

    void FreezeGame()
    {
        Time.timeScale = 0f; // Freeze the game
    }

    void UnfreezeGame()
    {
        Time.timeScale = 1f; // Unfreeze the game
    }

    void TeleportToTarget(GameObject targetObject)
    {
        playerCamera.transform.position = targetObject.transform.position;
        playerCamera.transform.rotation = targetObject.transform.rotation;
    }

    System.Collections.IEnumerator PanToTarget(GameObject targetObject)
    {
        isCameraPanning = true; // Camera is panning

        float duration = cameraDelay / panSpeed; // Adjust panning duration based on the speed
        float elapsedTime = 0f;

        Vector3 startPos = playerCamera.transform.position;
        Quaternion startRot = playerCamera.transform.rotation;

        Vector3 endPos = targetObject.transform.position;
        Quaternion endRot = targetObject.transform.rotation;

        while (elapsedTime < duration)
        {
            playerCamera.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            playerCamera.transform.rotation = Quaternion.Slerp(startRot, endRot, elapsedTime / duration);
            elapsedTime += freezeGame ? Time.unscaledDeltaTime : Time.deltaTime;
            yield return null;
        }

        playerCamera.transform.position = endPos;
        playerCamera.transform.rotation = endRot;

        StartCoroutine(ResetCameraAfterDelay());
    }

    System.Collections.IEnumerator ResetCameraAfterDelay()
    {
        yield return new WaitForSecondsRealtime(cameraDelay);

        isCameraPanning = false; // Camera panning is done

        // Increment to the next target in the list
        currentTargetIndex++;

        // Move to the next target or reset if finished
        MoveToNextTarget();
    }

    void ResetCameraPosition()
    {
        if (panCamera)
        {
            // Pan back to the original position and rotation
            StartCoroutine(PanBackToOriginal());
        }
        else
        {
            // Teleport back to the original position and rotation
            playerCamera.transform.position = originalCameraPosition;
            playerCamera.transform.rotation = originalCameraRotation;

            // Re-enable the player's camera script
            playerCameraScript.enabled = true;

            // Optionally unfreeze the game
            if (freezeGame)
            {
                UnfreezeGame();
            }
        }
    }

    System.Collections.IEnumerator PanBackToOriginal()
    {
        isCameraPanning = true; // Camera is panning

        float duration = cameraDelay / panSpeed; // Adjust panning duration based on the speed
        float elapsedTime = 0f;

        Vector3 startPos = playerCamera.transform.position;
        Quaternion startRot = playerCamera.transform.rotation;

        Vector3 endPos = originalCameraPosition;
        Quaternion endRot = originalCameraRotation;

        while (elapsedTime < duration)
        {
            playerCamera.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            playerCamera.transform.rotation = Quaternion.Slerp(startRot, endRot, elapsedTime / duration);
            elapsedTime += freezeGame ? Time.unscaledDeltaTime : Time.deltaTime;
            yield return null;
        }

        playerCamera.transform.position = endPos;
        playerCamera.transform.rotation = endRot;

        isCameraPanning = false; // Camera panning is done

        // Re-enable the player's camera script
        playerCameraScript.enabled = true;

        // Optionally unfreeze the game
        if (freezeGame)
        {
            UnfreezeGame();
        }

        Destroy(gameObject);
    }
}
