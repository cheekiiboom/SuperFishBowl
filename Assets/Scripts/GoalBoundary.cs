using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBoundary : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other != null && other.tag == "Player")
            Destroy(other.gameObject);
    }
}
