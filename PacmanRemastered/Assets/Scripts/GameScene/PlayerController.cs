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

    [SerializeField]
    private float moveSpeed;
    Rigidbody2D rigidbody2D;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
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
}
