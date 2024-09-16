using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    public Rigidbody shelfRigidbody;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartGame();
        RestartGame();
    }

    public void StartGame()
    {
        /* if (Input.GetKeyDown(KeyCode.E))
         {
             playerController.inputEnabled = true;
             shelfRigidbody.isKinematic = false;
         }*/
        playerController.inputEnabled = true;
        shelfRigidbody.isKinematic = false;
    }

    public void RestartGame()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
