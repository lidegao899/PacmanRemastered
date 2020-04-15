using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    private Vector3 wayPoint;
    private Queue<Vector3> wayPoints;

    private Vector3 _direction;

    [SerializeField]
    private GameObject target;

    public Vector3 Direction
    {
        get { return _direction; }
        set
        {
            _direction = value;
            Vector3 pos = new Vector3((int)transform.position.x, (int)transform.position.y);
            wayPoint = pos + _direction;

            if (target == null)
            {
                return;
            }

            target.transform.position = wayPoint;
        }
    }

    public float moveSpeed = 0.3f;

    enum GhostState { Wait, Init, Scatter, Chase, Scare }
    GhostState state;

    private Vector3 startPos;

    private GameManager gameManager;

    [SerializeField]
    private float scatterTimeLength = 5f;

    [SerializeField]
    private float scareTimeLength = 7f;

    private float timeToEndScatter;

    private float timeToEndWait;

    private Rigidbody2D rigidbody2D;

    private bool isWhite = false;
    private float _timeToTurnColor;
    private float _timeToTurnColorInterval = 0.5f;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        wayPoints = new Queue<Vector3>();
    }

    private void Start()
    {
        startPos = getStartPosAccordingToName();
        InItGhost();
    }

    private void FixedUpdate()
    {
        animate();

        switch (state)
        {
            case GhostState.Wait:
                Wait();
                break;
            case GhostState.Init:
                Init();
                break;
            case GhostState.Scatter:
                Scatter();
                break;
            case GhostState.Chase:
                Chase();
                break;
            case GhostState.Scare:
                Run();
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.name.Equals("pacman"))
        {
            return;
        }

        if (state == GhostState.Scare)
        {
            Calm();
        }
        else
        {
            gameManager.LostLife();
        }
    }

    private void Run()
    {
        if (Vector3.Distance(transform.position, wayPoint) > 0.0001f)
        {
            Vector2 pos = Vector2.MoveTowards(transform.position, wayPoint, moveSpeed);
            GetComponent<Rigidbody2D>().MovePosition(pos);
        }
        else
        {
            GetComponent<GhostAI>().RunLogic();
        }
    }

    private void Scatter()
    {
        if (Time.time > timeToEndScatter)
        {
            wayPoints.Clear();
            state = GhostState.Chase;
            return;
        }

        MoveToWayPoints(true);
    }

    private void Chase()
    {
        if (Vector3.Distance(transform.position, wayPoint) > 0.0001f)
        {
            Vector2 pos = Vector2.MoveTowards(transform.position, wayPoint, moveSpeed);
            rigidbody2D.MovePosition(pos);
        }
        else
        {
            GetComponent<GhostAI>().ChaseLogic();
        }
    }

    internal void Frighten()
    {
        state = GhostState.Scare;
        _direction *= -1;
        GetComponent<Animator>().SetBool("Run", true);
    }

    public void Calm()
    {
        if (state != GhostState.Scare)
        {
            return;
        }

        wayPoints.Clear();
        state = GhostState.Chase;

        //GetComponent<Animator>().SetBool("Run_White", false);
        GetComponent<Animator>().SetBool("Run", false);
    }

    private void ToggleSkinColor()
    {
        if (Time.time < _timeToTurnColor)
        {
            return;
        }

        isWhite = !isWhite;

        GetComponent<Animator>().SetBool("Run_White", isWhite);

        _timeToTurnColor = Time.time + _timeToTurnColorInterval;

        Debug.Log("color turn" + isWhite);
    }

    private void Wait()
    {
        state = GhostState.Init;
        wayPoints.Clear();
        InitWayPoints(state);
        MoveToWayPoints(true);
    }

    private void Init()
    {
        if (wayPoints.Count == 0)
        {
            state = GhostState.Scatter;
            InitWayPoints(state);
            timeToEndScatter = Time.time + scatterTimeLength;
            return;
        }
        MoveToWayPoints();
    }

    private void InitWayPoints(GhostState state)
    {
        string data = "";
        switch (name)
        {
            case "blinky":
                data = @"22 20
22 26

27 26
27 30
22 30
22 26";
                break;
            case "pinky":
                data = @"14.5 17
14 17
14 20
7 20

7 26
7 30
2 30
2 26";
                break;
            case "inky":
                data = @"16.5 17
15 17
15 20
22 20

22 8
19 8
19 5
16 5
16 2
27 2
27 5
22 5";
                break;
            case "clyde":
                data = @"12.5 17
14 17
14 20
7 20

7 8
7 5
2 5
2 2
13 2
13 5
10 5
10 8";
                break;
        }

        string line;
        wayPoints.Clear();
        switch (state)
        {
            case GhostState.Wait:
                Vector3 pos = transform.position;

                // inky and clyde start going down and then up
                if (transform.name == "inky" || transform.name == "clyde")
                {
                    wayPoints.Enqueue(new Vector3(pos.x, pos.y - 0.5f, 0f));
                    wayPoints.Enqueue(new Vector3(pos.x, pos.y + 0.5f, 0f));
                }
                // while pinky start going up and then down
                else
                {
                    wayPoints.Enqueue(new Vector3(pos.x, pos.y + 0.5f, 0f));
                    wayPoints.Enqueue(new Vector3(pos.x, pos.y - 0.5f, 0f));
                }
                break;
            case GhostState.Init:
                UpdateWayPoints(data, false);
                break;
            case GhostState.Scatter:
                UpdateWayPoints(data, true);
                break;
            case GhostState.Chase:
                break;
            case GhostState.Scare:
                break;
            default:
                break;
        }
    }

    private void MoveToWayPoints(bool loop = false)
    {
        wayPoint = wayPoints.Peek();
        if (Vector3.Distance(transform.position, wayPoint) > 0.0001f)
        {
            _direction = Vector3.Normalize(wayPoint - transform.position);

            Vector2 moveDir = Vector2.MoveTowards(transform.position, wayPoint, moveSpeed);

            rigidbody2D.MovePosition(moveDir);
        }
        else
        {
            if (loop)
            {
                wayPoints.Enqueue(wayPoints.Dequeue());
            }
            else
            {
                wayPoints.Dequeue();
            }
        }
    }

    private void InItGhost()
    {
        wayPoint = transform.position;
        state = GhostState.Wait;
        InitWayPoints(state);
    }

    private void UpdateWayPoints(string data, bool bScatter)
    {
        StringReader reader = new StringReader(data);
        wayPoints.Clear();
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            if (line.Length == 0)
            {
                if (bScatter)
                {
                    bScatter = true;
                    wayPoints.Clear();
                    continue;
                }
                else
                {
                    break;
                }
            }

            string[] values = line.Split(' ');
            float x = float.Parse(values[0]);
            float y = float.Parse(values[1]);

            wayPoints.Enqueue(new Vector3(x, y));
        }
    }

    void animate()
    {
        switch (state)
        {
            case GhostState.Scare:
                ToggleSkinColor();
                break;
            default:
                if (GameManager.gameState == GameManager.GameState.Game)
                {
                }
                Vector3 dir = wayPoint - transform.position;
                GetComponent<Animator>().SetFloat("DirX", dir.x);
                GetComponent<Animator>().SetFloat("DirY", dir.y);
                GetComponent<Animator>().SetBool("Run", false);
                break;
        }
    }

    private Vector3 getStartPosAccordingToName()
    {
        switch (gameObject.name)
        {
            case "blinky":
                return new Vector3(15f, 20f, 0f);
            case "pinky":
                return new Vector3(14.5f, 17f, 0f);

            case "inky":
                return new Vector3(16.5f, 17f, 0f);

            case "clyde":
                return new Vector3(12.5f, 17f, 0f);
        }
        return new Vector3();
    }
}
