using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import the SceneManager

public class GoalBoundary : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.tag == "Player")
        {
            ArrowRenderer arrowRenderer = other.gameObject.GetComponent<ArrowRenderer>();

            var targetsToRemove = new List<GameObject>(arrowRenderer.targetObjects);

            foreach (var target in targetsToRemove)
            {
                arrowRenderer.RemoveTargetObject(target);
            }

            // Load scene 3 instead of destroying the player
            SceneManager.LoadScene(3);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other != null && other.gameObject.tag == "Player")
        {
            ArrowRenderer arrowRenderer = other.gameObject.GetComponent<ArrowRenderer>();

            var targetsToRemove = new List<GameObject>(arrowRenderer.targetObjects);

            foreach (var target in targetsToRemove)
            {
                arrowRenderer.RemoveTargetObject(target);
            }

            // Load scene 3 instead of destroying the player
            SceneManager.LoadScene(3);
        }
    }
}
