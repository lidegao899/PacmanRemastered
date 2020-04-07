using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static int Score;

    public static int Lives = 3;

    public static int Level = 0;

    private GameManager gameManager;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
                DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this!=_instance)
            {
                Destroy(this);
            }
        }

        gameManager = GameObject.FindObjectOfType<GameManager>();
    }
}
