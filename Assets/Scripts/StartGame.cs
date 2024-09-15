using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    private bool gameStarted = false;

    [SerializeField]
    private Image StoryImage;

    [SerializeField]
    private GameObject SpaceElement; // The element to hide (like a button or text)

    private Vector3 currentStoryPosition;
    private Vector3 targetStoryPosition;
    private float scrollSpeed = 0.05f; // Adjust this for scrolling speed

    private void Start()
    {
        // Store the initial position of the story image and define the target position
        currentStoryPosition = StoryImage.transform.position; // 1920 pos x
        targetStoryPosition = new Vector3(-800f, currentStoryPosition.y, currentStoryPosition.z); // Target position (-1920 pos x)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !gameStarted)
        {
            // Start the game and begin scrolling the image
            gameStarted = true;
            SpaceElement.SetActive(false); // Hide the "space" element
            StartCoroutine(PlayScene());
        }
    }

    // Coroutine that scrolls the story image to the target position (-1920 pos x)
    IEnumerator PlayScene()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * scrollSpeed; // Use scrollSpeed to control the speed of scrolling
            StoryImage.transform.position = Vector3.Lerp(currentStoryPosition, targetStoryPosition, t);
            yield return null; // Wait for the next frame
        }

        // wait 5 seconds and then continue to the next scene in the game
        yield return new WaitForSeconds(5);

        SceneManager.LoadScene(1);

    }
}
