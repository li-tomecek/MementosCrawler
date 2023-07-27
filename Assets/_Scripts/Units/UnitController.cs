using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    //--------Shared components---------
    protected bool isMoving;
    protected Vector3 origPos, targetPos;
    public float timeToMove = 5.5f;

    float movementDir;

    private void Start()
    {
        //transform.position = (MapGrid.Instance.gridToWorldCoords(1,1));
    }

    // ---------Shared Methods-----------
    protected IEnumerator MoveActor(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        origPos = transform.position;
        targetPos = origPos + direction;

        //will only move player if they stay within the grid
        Vector2Int coord = MapGrid.Instance.worldToGridCoords(targetPos);
        Vector2Int startCoord = MapGrid.Instance.worldToGridCoords(origPos);

        if (coord.x != -1 && !MapGrid.Instance.tiles[coord.x, coord.y].isOccupied())
        {
            while (elapsedTime < timeToMove)
            {
                transform.position = Vector3.Lerp(origPos, targetPos, elapsedTime / timeToMove);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPos; //just to be safe
        }
        isMoving = false;
        MapGrid.Instance.tiles[startCoord.x, startCoord.y].setOccupied(false);
        MapGrid.Instance.tiles[coord.x, coord.y].setOccupied(true);
    }

    protected void MoveToTile(int x, int y)
    {
        //A* pathfinding!

    }
}
