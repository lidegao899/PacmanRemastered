using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 _curDir = Vector2.zero;

    Vector2 _dest = Vector2.zero;

    // next direction
    Vector2 _nextDir = Vector2.zero;

    Animator animator;

    public float moveSpeed;
    Rigidbody2D rigidbody2D;

    [SerializeField]
    private Vector2 initPos;

    private bool _deadPlaying = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        switch (GameManager.gameState)
        {
            case GameManager.GameState.Game:
                Vector2 newPos = Vector2.MoveTowards(transform.position, _dest, moveSpeed);
                rigidbody2D.MovePosition(newPos);

                // get the next direction from keyboard
                if (Input.GetAxis("Horizontal") > 0) _nextDir = Vector2.right;
                if (Input.GetAxis("Horizontal") < 0) _nextDir = Vector2.left;
                if (Input.GetAxis("Vertical") > 0) _nextDir = Vector2.up;
                if (Input.GetAxis("Vertical") < 0) _nextDir = Vector2.down;

                if (IsValid(_nextDir))
                {
                    _dest = (Vector2)transform.position + _nextDir;
                    _curDir = _nextDir;
                }
                else if (IsValid(_curDir))
                {
                    _dest = (Vector2)transform.position + _curDir;
                }

                UpdateAnimate();
                break;
            case GameManager.GameState.Dead:
                if (!_deadPlaying)
                {
                    StartCoroutine("PlayDeadAnimation");
                }
                break;
            default:
                break;
        }
       
    }

    private bool IsValid(Vector2 direction)
    {
        Vector2 pos = transform.position;
        direction += new Vector2(direction.x * 0.5f, direction.y * 0.5f);
        RaycastHit2D hit = Physics2D.Linecast(pos + direction, pos);
        return hit.collider == GetComponent<Collider2D>();
    }

    void Start()
    {
        _dest = transform.position;
    }
    private void UpdateAnimate()
    {
        Vector2 dir = _dest - (Vector2)transform.position;
        animator.SetFloat("DirX", dir.x);
        animator.SetFloat("DirY", dir.y);
    }

    IEnumerator PlayDeadAnimation()
    {
        _deadPlaying = true;

        animator.SetBool("Die", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("Die", false);

        _deadPlaying = false;

        if (GameManager.Lives <= 0)
        {
            GameManager.Instance.GameOver();
        }
        else
        {
            GameManager.Instance.ResetScene();
        }
    }

    private void ResetPlayer()
    {
        _nextDir = initPos;
        _dest = transform.position;
        animator.SetFloat("DirX", 1);
        animator.SetFloat("DirY", 0);
    }
}
