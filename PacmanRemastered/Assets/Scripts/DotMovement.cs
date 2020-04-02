using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotMovement : MonoBehaviour
{
    [SerializeField]
    private float dotSpeed = 2f;

    void Update()
    {
        transform.Translate(Vector3.left * dotSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
