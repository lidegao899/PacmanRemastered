using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    public Transform target;

    [SerializeField]
    private TileManager tileManager;

    private GhostMovement ghostMovement;

    private TileManager.Tile nextTile;
    private TileManager.Tile targetTile;

    private void Awake()
    {
        ghostMovement = GetComponent<GhostMovement>();

        if (ghostMovement == null) Debug.Log("game object ghostMovement not found");
        if (tileManager == null) Debug.Log("game object tileManager not found");
    }

    public void ChaseLogic()
    {
        Vector3 currentPos = new Vector3(transform.position.x + .499f, transform.position.y + .499f);
        TileManager.Tile currentTile = tileManager.GetTileByPos(currentPos);

        targetTile = GetTargetTile();

        nextTile = tileManager.GetTileByPos(currentPos + ghostMovement.Direction);

        if (nextTile.Occupied)
        {
            if (ghostMovement.Direction == Vector3.left || ghostMovement.Direction == Vector3.right)
            {
                ghostMovement.Direction = currentTile.down == null ? Vector3.up : Vector3.down;
            }
            else
            {
                ghostMovement.Direction = currentTile.left == null ? Vector3.right : Vector3.left;
            }
        }
        else if (currentTile.IsIntersection)
        {
            float shortstDis = float.MaxValue;

            Vector3 dir = ghostMovement.Direction;

            if (currentTile.up != null && ghostMovement.Direction != Vector3.down && shortstDis > tileManager.GetTileDistance(targetTile, currentTile.up))
            {
                ghostMovement.Direction = Vector3.up;
                shortstDis = tileManager.GetTileDistance(targetTile, currentTile.up);
            }
            if (currentTile.right != null && ghostMovement.Direction != Vector3.left && shortstDis > tileManager.GetTileDistance(targetTile, currentTile.right))
            {
                ghostMovement.Direction = Vector3.right;
                shortstDis = tileManager.GetTileDistance(targetTile, currentTile.right);
            }
            if (currentTile.left != null && ghostMovement.Direction != Vector3.right && shortstDis > tileManager.GetTileDistance(targetTile, currentTile.left))
            {
                ghostMovement.Direction = Vector3.left;
                shortstDis = tileManager.GetTileDistance(targetTile, currentTile.left);
            }
            if (currentTile.down != null && ghostMovement.Direction != Vector3.up && shortstDis > tileManager.GetTileDistance(targetTile, currentTile.down))
            {
                ghostMovement.Direction = Vector3.down;
            }

            if (dir.Equals(ghostMovement.Direction))
            {
                Debug.Log("dir change");
            }
        }
        else
        {
            ghostMovement.Direction = ghostMovement.Direction;
        }
    }

    public void RunLogic()
    {
        // get current pos
        Vector3 currentPos = new Vector3(transform.position.x + .499f, transform.position.y + .499f);
        TileManager.Tile currentTile = tileManager.GetTileByPos(currentPos);

        nextTile = tileManager.GetTileByPos(currentPos + ghostMovement.Direction);

        if (nextTile.Occupied)
        {
            if (ghostMovement.Direction == Vector3.left || ghostMovement.Direction == Vector3.right)
            {
                ghostMovement.Direction = currentTile.down == null ? Vector3.up : Vector3.down;
            }
            else
            {
                ghostMovement.Direction = currentTile.left == null ? Vector3.right : Vector3.left;
            }
        }
        else if (currentTile.IsIntersection)
        {
            List<Vector3> availableDir = new List<Vector3>();

            if (currentTile.up != null && currentTile.up.Occupied == false && ghostMovement.Direction != Vector3.down)
            {
                availableDir.Add(Vector3.up); 
            }
            if (currentTile.down != null && currentTile.down.Occupied == false && ghostMovement.Direction != Vector3.up)
            {
                availableDir.Add(Vector3.down);
            }
            if (currentTile.left != null && currentTile.left.Occupied == false && ghostMovement.Direction != Vector3.right)
            {
                availableDir.Add(Vector3.left);
            }
            if (currentTile.right != null && currentTile.right.Occupied == false && ghostMovement.Direction != Vector3.left)
            {
                availableDir.Add(Vector3.right);
            }

            int randIndex = Random.Range(0, availableDir.Count);
            ghostMovement.Direction = availableDir[randIndex];
        }
        else
        {
            ghostMovement.Direction = ghostMovement.Direction;
        }
    }

    TileManager.Tile GetTargetTile()
    {
        Vector3 targetPos;
        TileManager.Tile _targetTile = new TileManager.Tile(0, 0);

        switch (name)
        {
            case "blinky": // target=pacman
                targetPos = new Vector3(target.transform.position.x, target.transform.position.y);
                _targetTile = tileManager.GetTileByPos(targetPos);
                break;
            default:
                break;
        }

        return _targetTile;
    }
}
