using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    // Public variables for customizing bobbing and rotation speed
    public float bobSpeed = 10f;
    public float bobHeight = 0.05f;
    public float rotationSpeed = 100f;

    // Collection animation settings
    public float riseSpeed = 100f;
    public float fastRotationSpeed = 10000f;
    public float riseHeight = 0.3f; // Height to which the key rises upon collection
    public float spinDuration = 1f; // Time to spin before disappearing

    private Vector3 initialPosition;
    private bool isCollected = false;


    public Material Material;
    public GameObject[] Doors;
    public GameObject[] ObjectsToDestroy;
    public GameObject[] ObjectsToEnable;
    private Rigidbody rigidbody;
    public GameObject[] Objectives;

    void Start()
    {
        // Store the initial position of the key
        initialPosition = transform.position;
    }

    void Update()
    {
        if (!isCollected)
        {
            // Bobbing up and down
            float newY = initialPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);

            // Constant rotation
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    // Call this method when the player collects the key
    public void CollectKey()
    {
        if (!isCollected)
        {
            isCollected = true;
            StartCoroutine(CollectedAnimation());
        }
    }

    private IEnumerator CollectedAnimation()
    {
        // Phase 1: Rising to the target height
        Vector3 targetPosition = transform.position + Vector3.up * riseHeight;
        while (transform.position.y < targetPosition.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, riseSpeed * Time.deltaTime);
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime); // Continue slow rotation while rising
            yield return null;
        }

        // Phase 2: Spinning fast for a set duration
        float elapsedTime = 0f;
        while (elapsedTime < spinDuration)
        {
            transform.Rotate(Vector3.up, fastRotationSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Destroy the key object after the animation
        Destroy(gameObject);
    }

    // Example trigger method (you can replace this with your own collection mechanism)
    private void OnTriggerEnter(Collider other)
    {

        if (other != null && other.tag == "Player")
        {
            CollectKey();

            ArrowRenderer arrowRenderer = other.gameObject.GetComponent<ArrowRenderer>();

            var targetsToRemove = new List<GameObject>(arrowRenderer.targetObjects);

            foreach (var target in targetsToRemove)
                arrowRenderer.RemoveTargetObject(target);

            foreach (var door in Doors)
            {
                if (door.GetComponent<Rigidbody>() != null)
                    rigidbody = door.GetComponent<Rigidbody>();
                if (rigidbody != null)
                    rigidbody.isKinematic = false;

                door.GetComponent<MeshRenderer>().material = Material;
            }

            foreach (var obj in ObjectsToEnable)
                obj.SetActive(true);

            foreach (var obj in Objectives)
                arrowRenderer.AddTargetObject(obj);

            foreach (var obj in ObjectsToDestroy)
                Destroy(obj);
        }
    }
}
