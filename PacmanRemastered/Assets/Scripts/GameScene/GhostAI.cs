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

            if (currentTile.up != null == false && ghostMovement.Direction != Vector3.down && shortstDis > tileManager.GetTileDistance(currentTile,currentTile.up))
            {
                ghostMovement.Direction = Vector3.up;
                shortstDis = tileManager.GetTileDistance(currentTile, currentTile.up);
            }
            if (currentTile.right != null == false && ghostMovement.Direction != Vector3.left && shortstDis > tileManager.GetTileDistance(currentTile, currentTile.right))
            {
                ghostMovement.Direction = Vector3.right;
                shortstDis = tileManager.GetTileDistance(currentTile, currentTile.right);
            }
            if (currentTile.left != null == false && ghostMovement.Direction != Vector3.right && shortstDis > tileManager.GetTileDistance(currentTile, currentTile.left))
            {
                ghostMovement.Direction = Vector3.left;
                shortstDis = tileManager.GetTileDistance(currentTile, currentTile.left);
            }
            if (currentTile.down != null == false && ghostMovement.Direction != Vector3.up && shortstDis > tileManager.GetTileDistance(currentTile, currentTile.down))
            {
                ghostMovement.Direction = Vector3.down;
            }
        }
    }

    public void RunLogic()
    {

    }

    TileManager.Tile GetTargetTile()
    {
        Vector3 targetPos;
        TileManager.Tile _targetTile = new TileManager.Tile(0, 0);

        switch (name)
        {
            case "blinky": // target=pacman
                targetPos = new Vector3(transform.position.x + .499f, transform.position.y + .499f);
                _targetTile = tileManager.GetTileByPos(targetPos);
                break;
            default:
                break;
        }

        return _targetTile;
    }
}
