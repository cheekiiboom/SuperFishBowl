using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Material Material;
    public GameObject[] Doors;
    public GameObject[] ObjectsToDestroy;
    public GameObject[] ObjectsToEnable;
    private Rigidbody rigidbody;
    public GameObject[] Objectives;
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.tag == "Player")
        {
            ArrowRenderer arrowRenderer = other.gameObject.GetComponent<ArrowRenderer>();

            var targetsToRemove = new List<GameObject>(arrowRenderer.targetObjects);

            foreach (var target in targetsToRemove)
                arrowRenderer.RemoveTargetObject(target);

            foreach (var door in Doors)
            {
                if(door.GetComponent<Rigidbody>() != null)
                    rigidbody = door.GetComponent<Rigidbody>();
                if(rigidbody != null)
                    rigidbody.isKinematic = false;

                door.GetComponent<MeshRenderer>().material = Material;
            }

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
