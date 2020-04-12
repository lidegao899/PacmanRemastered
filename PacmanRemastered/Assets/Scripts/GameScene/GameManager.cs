using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public enum GameState { Init, Game, Dead, Scores }
    public static GameState gameState;

    public static int Score;

    public static int Lives = 3;

    public static int Level = 0;

    [SerializeField]
    private float MoveSpeedPerLevel = 0.025f;

    private GameObject pacman;
    private GameObject blinky;

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
            if (this != _instance)
            {
                Destroy(this);
            }
        }

        AssignGhosts();
    }

    private void Start()
    {
        gameState = GameState.Init;
    }

    private void OnLevelWasLoaded(int level)
    {
        AssignGhosts();

        float speedIncreatementByLevel = level * MoveSpeedPerLevel;

        pacman.GetComponent<PlayerController>().moveSpeed += speedIncreatementByLevel / 2;

        blinky.GetComponent<GhostMovement>().moveSpeed += speedIncreatementByLevel;
    }

    private void AssignGhosts()
    {
        pacman = GameObject.Find("pacman");
        blinky = GameObject.Find("blinky");
    }

    public void LostLife()
    {
        throw new NotImplementedException();
    }

    public void ResetScene()
    {
        pacman.transform.position = new Vector3(15f, 11f, 0f);
        blinky.transform.position = new Vector3(15f, 20f, 0f);

        gameState = GameState.Init;
    }
}
