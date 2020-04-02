using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject dotPrefeb;

    [SerializeField]
    private float spawnInterval;

    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime + spawnInterval < Time.time)
        {
            GameObject obj = (GameObject)Instantiate(dotPrefeb, transform.position, Quaternion.identity) ;
            obj.transform.parent = transform;

            startTime = Time.time;
        }
    }
}
