using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

public class UnitController : MonoBehaviour
{
    //--------Shared components---------
    protected bool isMoving;
    protected Vector3 origPos, targetPos;
    public float timeToMove = 5.5f;
    public int startX;
    public int startY;

    float movementDir;

    private void Start()
    {
        transform.position = (MapGrid.Instance.gridToWorldCoords(startX,startY));
    }

    // ---------Shared Methods-----------
    protected IEnumerator MoveActor(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        origPos = transform.position;
        targetPos = origPos + direction;

        //will only move player if they stay within the grid
        Coords coord = MapGrid.Instance.worldToGridCoords(targetPos);
        Coords startCoord = MapGrid.Instance.worldToGridCoords(origPos);

        if (coord.X != -1 && !MapGrid.Instance.tiles[coord.X, coord.Y].isOccupied())
        {
            while (elapsedTime < timeToMove)
            {
                transform.position = Vector3.Lerp(origPos, targetPos, elapsedTime / timeToMove);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPos; //just to be safe
            MapGrid.Instance.tiles[startCoord.X, startCoord.Y].setOccupied(false);
            MapGrid.Instance.tiles[coord.X, coord.Y].setOccupied(true);
        }
        isMoving = false;
    }

    protected IEnumerator MoveOneStep(Coords target)
    {
        float elapsedTime = 0;
        //isMoving = true;

        origPos = transform.position;
        targetPos = MapGrid.Instance.gridToWorldCoords(target.X, target.Y);
        Coords startCoord = MapGrid.Instance.worldToGridCoords(origPos);


        if (target.X != -1 && !MapGrid.Instance.tiles[target.X, target.Y].isOccupied()) //this is just an extra layer of precaution at this point
        {
            while (elapsedTime < timeToMove)
            {
                transform.position = Vector3.Lerp(origPos, targetPos, elapsedTime / timeToMove);

                elapsedTime += Time.deltaTime;
                yield return null;

            }

            transform.position = targetPos; //just to be safe
            MapGrid.Instance.tiles[startCoord.X, startCoord.Y].setOccupied(false);
            MapGrid.Instance.tiles[target.X, target.Y].setOccupied(true);
        }

        //isMoving = false;
    }

    private IEnumerator PlayQueuedRoutines(Queue<IEnumerator> coroutines)
    {
        GameManager.Instance.getBattleManager().blockPlayerInputs = true;
        IEnumerator currentCoroutine;
        while(coroutines.Count > 0)
        {
            currentCoroutine = coroutines.Dequeue();
            yield return StartCoroutine(currentCoroutine);
        }
        GameManager.Instance.getBattleManager().blockPlayerInputs = false;


    }

    public void MoveToDistantTile(Coords target)
    {

        Coords startCoord = MapGrid.Instance.worldToGridCoords(transform.position);
        if (startCoord == target)
            return;
        
        List<Node> nodeQueue = new List<Node>();
        Node[,] allNodes = new Node[GameManager.Instance.columns, GameManager.Instance.rows];

        foreach (MapTile tile in MapGrid.Instance.tiles)
        {
            Node temp = new Node();
            temp.coord.X = tile.getX();
            temp.coord.Y = tile.getY();
            temp.manhattanDist = temp.getManhattanDistanceToCoord(target);
            
            allNodes[temp.coord.X, temp.coord.Y] = temp;

            if (temp.coord == startCoord)
            {
                temp.costToStart = 0;
                nodeQueue.Add(temp);
            }
        }
        //A* pathfinding using Manhattan distance as heuristic

        Node n;
        do
        {
            nodeQueue = nodeQueue.OrderBy(x => x.costToStart + x.manhattanDist).ToList();
            n = nodeQueue.First();
            nodeQueue.Remove(n);

            List<Node> neighbours = n.getNeighbours(allNodes);
            foreach (Node neighbour in neighbours)
            {
                if (!neighbour.visited)
                {
                    if( neighbour.costToStart == -1 || neighbour.costToStart > n.costToStart + 1)
                    {
                        neighbour.costToStart = n.costToStart + 1;
                        neighbour.parent = n;

                        if (!nodeQueue.Contains(neighbour))
                            nodeQueue.Add(neighbour);
                    }
                }
            }
            n.visited = true;
            if (n.coord == target)
                break;
        } while(nodeQueue.Count > 0);

        //use list from A* to trace a path (reverse order)
        List<Coords> pathToTarget = new List<Coords>();
        do
        {
            pathToTarget.Add(n.coord);
            n = n.parent;

        } while (n.coord != startCoord);

        //for each step, get vector from current position to next adjacent tile, start coroutine (MoveActor(this vector))?
        Queue<IEnumerator> corountineQueue = new Queue<IEnumerator>();
        for (int i = pathToTarget.Count() - 1; i >=0; i--)
        {
            corountineQueue.Enqueue(MoveOneStep(pathToTarget.ElementAt(i)));
        }
        
        StartCoroutine(PlayQueuedRoutines(corountineQueue));
    }
}


public class Node       //!!!!WILL PROBABLY WANT TO EVENTUALLY MERGE THIS INTO MAPTILE SO THAT WE CAN HAVE SOME COLLISION PREVENTION (WALKING AROUND OTHER UNITS RATHER THAN THROUGH THEM!!!!!)
{
    public Coords coord;
    public Node parent;
    public int manhattanDist;
    public int costToStart = -1;
    public bool visited = false;

    public int getManhattanDistanceToCoord(Coords c)
    {
        return Mathf.Abs(c.X - coord.X) + Mathf.Abs(c.Y - coord.Y);
    }

    public List<Node> getNeighbours(Node[,] nodes)
    {
        List<Node> temp = new List<Node>();

        for(int i = coord.X - 1; i <= coord.X + 1; i += 2)
        {
            if (!(i < 0 || i >= GameManager.Instance.columns))
                temp.Add(nodes[i, coord.Y]);
        }
        for (int i = coord.Y - 1; i <= coord.Y + 1; i += 2)
        {
            if (!(i < 0 || i >= GameManager.Instance.rows))
                temp.Add(nodes[coord.X, i]);
        }

        return temp;
    }

}
