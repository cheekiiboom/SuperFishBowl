using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Material Material;
    public GameObject[] Doors;
    public GameObject[] ObjectsToDestroy;
    private Rigidbody rigidbody;
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.tag == "Player")
        {
            foreach(var door in Doors)
            {
                if(door.GetComponent<Rigidbody>() != null)
                    rigidbody = door.GetComponent<Rigidbody>();
                if(rigidbody != null)
                    rigidbody.isKinematic = false;

                door.GetComponent<MeshRenderer>().material = Material;
            }

            foreach (var obj in ObjectsToDestroy)
                Destroy(obj);

            Destroy(gameObject);
        }
    }
}
