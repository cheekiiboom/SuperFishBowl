using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBoundary : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other != null && other.tag == "Player")
        {
            ArrowRenderer arrowRenderer = other.gameObject.GetComponent<ArrowRenderer>();

            var targetsToRemove = new List<GameObject>(arrowRenderer.targetObjects);

            foreach (var target in targetsToRemove)
            {
                arrowRenderer.RemoveTargetObject(target);
            }

            Destroy(other.gameObject);
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

            Destroy(other.gameObject);
        }
    }
}
