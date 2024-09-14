using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    public GameObject[] Objectives;
    public GameObject[] ObjectsToEnable;
    public GameObject[] ObjectsToDestroy;
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.tag == "Player")
        {
            ArrowRenderer arrowRenderer = other.gameObject.GetComponent<ArrowRenderer>();

            var targetsToRemove = new List<GameObject>(arrowRenderer.targetObjects);

            foreach (var target in targetsToRemove)
                arrowRenderer.RemoveTargetObject(target);

            foreach (var obj in ObjectsToEnable)
                obj.SetActive(true);

            foreach (var obj in Objectives)
                arrowRenderer.AddTargetObject(obj);

            foreach (var obj in ObjectsToDestroy)
                Destroy(obj);

            Destroy(gameObject);
        }
    }
}
