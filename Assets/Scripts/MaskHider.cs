using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MaskHider : MonoBehaviour
{
    [SerializeField]
    private float duration = 5f; // Time to hide the sprite
    private Image maskImage;
    private RectTransform maskRectTransform;

    private void Start()
    {
        maskImage = GetComponent<Image>();
        maskRectTransform = GetComponent<RectTransform>();
        maskRectTransform.sizeDelta = new Vector2(maskRectTransform.sizeDelta.x, maskRectTransform.sizeDelta.y);

        // Start the hide coroutine
        StartCoroutine(HideMask());
    }

    private IEnumerator HideMask()
    {
        float elapsed = 0f;

        // Calculate the initial height of the mask
        float initialHeight = maskRectTransform.sizeDelta.y;

        while (elapsed < duration)
        {
            // Calculate the new height based on time
            float newHeight = Mathf.Lerp(initialHeight, 0, elapsed / duration);
            maskRectTransform.sizeDelta = new Vector2(maskRectTransform.sizeDelta.x, newHeight);
            elapsed += Time.deltaTime;

            yield return null; // Wait until the next frame
        }

        // Ensure the mask is completely hidden
        maskRectTransform.sizeDelta = new Vector2(maskRectTransform.sizeDelta.x, 0);
    }
}
