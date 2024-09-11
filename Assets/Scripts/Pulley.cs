using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulley : MonoBehaviour
{
    public GameObject object1; // The first object
    public GameObject object2; // The second object
    private Vector3 previousPositionObject1;
    private Vector3 previousPositionObject2;

    private LineRenderer _lineRenderer;

    [SerializeField]
    private Transform[] _pivotTransforms;

    void Start()
    {
        // Initialize the previous positions
        previousPositionObject1 = object1.transform.position;
        previousPositionObject2 = object2.transform.position;

        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        // Calculate the difference in position of object1
        Vector3 object1Movement = object1.transform.position - previousPositionObject1;

        // Calculate the difference in position of object2
        Vector3 object2Movement = object2.transform.position - previousPositionObject2;

        // If object1 moved, move object2 by the opposite amount along the y-axis
        if (object1Movement != Vector3.zero)
        {
            object2.transform.position += new Vector3(0, -object1Movement.y, 0);
        }

        // If object2 moved, move object1 by the opposite amount along the y-axis
        if (object2Movement != Vector3.zero)
        {
            object1.transform.position += new Vector3(0, -object2Movement.y, 0);
        }

        // Update the previous positions
        previousPositionObject1 = object1.transform.position;
        previousPositionObject2 = object2.transform.position;

        _lineRenderer.positionCount = _pivotTransforms.Length;

        for (int i = 0; i < _pivotTransforms.Length; i++)
            _lineRenderer.SetPosition(i, _pivotTransforms[i].position);
    }
}
