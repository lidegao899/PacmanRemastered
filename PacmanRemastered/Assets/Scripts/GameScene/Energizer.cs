using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energizer : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("energizer did not get game manager");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name != "pacman")
        {
            return;
        }

        gameManager.ScareGhosts();
        Destroy(gameObject);
    }
}
