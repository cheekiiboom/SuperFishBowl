using System.Collections.Generic;
using UnityEngine;

public class ArrowRenderer : MonoBehaviour
{
    // List to hold all the objects to point at
    public List<GameObject> targetObjects = new List<GameObject>();

    // Line renderer prefab, one for each target object
    public GameObject lineRendererPrefab;

    // List of active line renderers
    private List<LineRenderer> lineRenderers = new List<LineRenderer>();

    // Maximum length for the lines
    public float maxLineLength = 7f;

    void Start()
    {
        // Create initial line renderers
        UpdateLineRenderers();
    }

    void Update()
    {
        // Update the positions of each line
        for (int i = 0; i < lineRenderers.Count; i++)
        {
            if (i >= targetObjects.Count || targetObjects[i] == null)
            {
                // If the target object has been destroyed or is null, destroy the line and remove it
                Destroy(lineRenderers[i].gameObject);
                lineRenderers.RemoveAt(i);
                targetObjects.RemoveAt(i);
                i--; // Adjust the index to account for the removal
                continue;
            }

            // Get the player's position
            Vector3 playerPosition = transform.position;

            // Get the target object's position
            Vector3 targetPosition = targetObjects[i].transform.position;

            // Calculate the direction from the player to the target
            Vector3 direction = targetPosition - playerPosition;

            // Calculate the distance to the target
            float distanceToTarget = direction.magnitude;

            // If the distance exceeds the maximum line length, clamp it
            if (distanceToTarget > maxLineLength)
            {
                // Clamp the direction vector to the maximum length
                direction = direction.normalized * maxLineLength;
                targetPosition = playerPosition + direction;
            }

            // Set the start position of the line to the player's position
            lineRenderers[i].SetPosition(0, playerPosition + direction.normalized * 0.5f);

            // Set the end position of the line, clamped if necessary
            lineRenderers[i].SetPosition(1, targetPosition);
        }
    }

    // Public method to add new target objects at runtime
    public void AddTargetObject(GameObject newTarget)
    {
        if (newTarget != null && !targetObjects.Contains(newTarget))
        {
            targetObjects.Add(newTarget);
            // Create a new LineRenderer for the new target
            GameObject lineRendererObject = Instantiate(lineRendererPrefab, transform.position, Quaternion.identity);
            LineRenderer lineRenderer = lineRendererObject.GetComponent<LineRenderer>();
            lineRenderers.Add(lineRenderer);
        }
    }

    // Public method to remove a target object at runtime
    public void RemoveTargetObject(GameObject target)
    {
        int index = targetObjects.IndexOf(target);
        if (index != -1)
        {
            // Destroy the corresponding line renderer
            Destroy(lineRenderers[index].gameObject);
            lineRenderers.RemoveAt(index);
            targetObjects.RemoveAt(index);
        }
    }

    // A helper method to initialize or update line renderers for all current targets
    private void UpdateLineRenderers()
    {
        // Remove existing line renderers (if any)
        foreach (LineRenderer lineRenderer in lineRenderers)
        {
            Destroy(lineRenderer.gameObject);
        }
        lineRenderers.Clear();

        // Create a LineRenderer for each target object
        foreach (GameObject target in targetObjects)
        {
            if (target != null)
            {
                GameObject lineRendererObject = Instantiate(lineRendererPrefab, transform.position, Quaternion.identity);
                LineRenderer lineRenderer = lineRendererObject.GetComponent<LineRenderer>();
                lineRenderers.Add(lineRenderer);
            }
        }
    }
}
