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
    public float timeToMove = 5.0f;
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
        Coord coord = MapGrid.Instance.worldToGridCoords(targetPos);
        Coord startCoord = MapGrid.Instance.worldToGridCoords(origPos);

        if (coord.X != -1 && MapGrid.Instance.tiles[coord.X, coord.Y].isTraversible())
        {
            while (elapsedTime < timeToMove)
            {
                transform.position = Vector3.Lerp(origPos, targetPos, elapsedTime / timeToMove);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPos; //just to be safe
            MapGrid.Instance.tiles[startCoord.X, startCoord.Y].setTraversible(true);
            MapGrid.Instance.tiles[coord.X, coord.Y].setTraversible(false);
        }
        isMoving = false;
    }

    protected IEnumerator MoveOneStep(Coord target)
    {
        float elapsedTime = 0;

        origPos = transform.position;
        targetPos = MapGrid.Instance.gridToWorldCoords(target.X, target.Y);
        Coord startCoord = MapGrid.Instance.worldToGridCoords(origPos);


        if (target.X != -1 && MapGrid.Instance.tiles[target.X, target.Y].isTraversible()) //this is just an extra layer of precaution at this point
        {
            while (elapsedTime < timeToMove)
            {
                transform.position = Vector3.Lerp(origPos, targetPos, elapsedTime / timeToMove);

                elapsedTime += Time.deltaTime;
                yield return null;

            }

            transform.position = targetPos; //just to be safe
            MapGrid.Instance.tiles[startCoord.X, startCoord.Y].setTraversible(true);
            MapGrid.Instance.tiles[target.X, target.Y].setTraversible(false);
        }

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

    public void MoveToDistantTile(Coord target)
    {

        Coord startCoord = MapGrid.Instance.worldToGridCoords(transform.position);
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
            temp.traversible = tile.isTraversible();
            
            allNodes[temp.coord.X, temp.coord.Y] = temp;

            if (temp.coord == startCoord)
            {
                temp.costToStart = 0;
                nodeQueue.Add(temp);
            }

            if(temp.coord == target && !temp.traversible)
            {
                Debug.Log("The target tile is not traversible! Skipping movement.");
                return;
            }

        }
        
        //A* pathfinding using Manhattan distance as heuristic
        Node n;
        do
        {
            nodeQueue = nodeQueue.OrderBy(x => x.costToStart + x.manhattanDist).ToList();       //the maps are small enough that there should not be very many nodes in this at all, thus there is not really a time concern
            n = nodeQueue.First();
            nodeQueue.Remove(n);

            List<Node> neighbours = n.getNeighbours(allNodes);
            foreach (Node neighbour in neighbours)
            {
                if (!neighbour.visited && neighbour.traversible)
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
        List<Coord> pathToTarget = new List<Coord>();
        do
        {
            pathToTarget.Add(n.coord);
            n = n.parent;

        } while (n.coord != startCoord);

        //for each step, get vector from current position to next adjacent tile, start coroutine
        Queue<IEnumerator> corountineQueue = new Queue<IEnumerator>();
        for (int i = pathToTarget.Count() - 1; i >=0; i--)
        {
            corountineQueue.Enqueue(MoveOneStep(pathToTarget.ElementAt(i)));
        }
        
        StartCoroutine(PlayQueuedRoutines(corountineQueue));
    }
}


public class Node       
{
    public Coord coord;
    public Node parent;
    public int manhattanDist;
    public int costToStart = -1;
    public bool visited = false;
    public bool traversible;

    public int getManhattanDistanceToCoord(Coord c)
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
