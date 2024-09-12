using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Material Material;
    public GameObject Door;
    public GameObject[] ObjectsToDestroy;
    private Rigidbody rigidbody;
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.tag == "Player")
        {
            rigidbody = Door.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            Door.GetComponent<MeshRenderer>().material = Material;

            foreach (var obj in ObjectsToDestroy)
                Destroy(obj);

            Destroy(gameObject);
        }
    }
}
