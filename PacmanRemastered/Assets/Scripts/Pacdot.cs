using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacdot : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("pacman"))
        {
            Debug.Log("coll");
            Destroy(gameObject);
            GameManager.Score += 10;
        }
    }
}
