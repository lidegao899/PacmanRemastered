using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private GUIManager uiManager;

    public enum GameState { Init, Game, Dead, Scores }
    public static GameState gameState;

    public static int Score;

    public static int Lives = 3;

    public static int Level = 0;

    [SerializeField]
    private float MoveSpeedPerLevel = 0.025f;

    private GameObject pacman;
    private GameObject blinky;

    [SerializeField]
    private float _scareTimeLength = 7f;

    private float _timeToCalm = 0f;
    private bool scared;

    public float ScareTimeLength
    {
        get { return _scareTimeLength; }
    }

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

    public void ScareGhosts()
    {
        scared = true;

        blinky.GetComponent<GhostMovement>().Frighten();
        _timeToCalm = Time.time + ScareTimeLength;
    }

    internal static void DestroySelf()
    {
        Score = 0;
        Level = 0;
        Lives = 3;
        Destroy(GameObject.Find("Game Manager"));
    }

    public void CalmGhost()
    {
        scared = false;

        blinky.GetComponent<GhostMovement>().Calm();
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
        uiManager = GameObject.Find("UIManager").GetComponent<GUIManager>();
    }

    private void Start()
    {
        gameState = GameState.Init;
    }

    public int[] SortedSquares(int[] A)
    {

        int[] arr = new int[A.Length];

        int leftIndex = 0, rightIndex = A.Length;

        int leftV = A[leftIndex], rightV = A[rightIndex];

        int curIndex = A.Length;

        for (int i = 0; i < A.Length; i++)
        {
            if (leftV > rightV)
            {
                arr[curIndex] = leftV;
                leftIndex++;
                leftV = arr[curIndex] * arr[curIndex];
            }
            else
            {
                arr[curIndex] = rightV;
                rightV--;
                rightV = arr[curIndex] * arr[curIndex];
            }
            curIndex--;
        }

        return arr;
    }

    private void Update()
    {
        if (scared && Time.time > _timeToCalm)
        {
            CalmGhost();
        }
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

    public void PlayerDead()
    {
        Lives--;
        gameState = GameState.Dead;

        uiManager.UpdateLife(Lives);
    }

    public void ResetScene()
    {
        CalmGhost();

        pacman.transform.position = new Vector3(15f, 11f, 0f);
        blinky.transform.position = new Vector3(15f, 20f, 0f);
        blinky.GetComponent<GhostMovement>().Initialize();

        gameState = GameState.Init;

        uiManager.ShowReadyScreen();
    }

    public void GameOver()
    {
        uiManager.ShowGameOverScreen();
    }
}
