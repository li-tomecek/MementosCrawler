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


    public Coord grid_pos;
    protected SpriteRenderer spriteRenderer;
    [HideInInspector] public List<Coord> reachableCoords = new List<Coord>();
  

    protected void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Coord[] spawn_tiles = MapGrid.Instance.enemy_spawn_tiles;
        if (this is PlayerController)
            spawn_tiles = MapGrid.Instance.player_spawn_tiles;

        foreach (Coord c in spawn_tiles)
        {
            if(MapGrid.Instance.tiles[c.X, c.Y].isTraversible())
            {
                transform.position = (MapGrid.Instance.gridToWorldCoords(c.X, c.Y));
                grid_pos = c;
                MapGrid.Instance.tiles[c.X, c.Y].setOccupant(this.gameObject);
                return;
            }
        }
    }

    // ---------Shared Methods-----------
    protected IEnumerator MoveInDirection(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        origPos = transform.position;
        targetPos = origPos + direction;

        //will only move player if they stay within the grid
        Coord coord = MapGrid.Instance.worldToGridCoords(targetPos);
        Coord startCoord = MapGrid.Instance.worldToGridCoords(origPos);

        if (coord.X != -1
            && MapGrid.Instance.tiles[coord.X, coord.Y].isTraversible()
            && (reachableCoords.Contains(coord) || GameManager.Instance.getMode() != Mode.PLAYER_TURN))
        {
            while (elapsedTime < timeToMove)
            {
                transform.position = Vector3.Lerp(origPos, targetPos, elapsedTime / timeToMove);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPos; //just to be safe
            MapGrid.Instance.tiles[startCoord.X, startCoord.Y].clearOccupant();
            MapGrid.Instance.tiles[coord.X, coord.Y].setOccupant(gameObject);
            grid_pos = coord;
            //Debug.Log("Position is now " + position.ToString());

        }
        isMoving = false;
    }
    protected IEnumerator MoveOneStep(Coord target)
    {
        float elapsedTime = 0;

        origPos = transform.position;
        targetPos = MapGrid.Instance.gridToWorldCoords(target.X, target.Y);
        Coord startCoord = MapGrid.Instance.worldToGridCoords(origPos);
        if (target.X < startCoord.X)
            spriteRenderer.flipX = false;
        else if (target.X > startCoord.X)
            spriteRenderer.flipX = true;

        if (target.X != -1 && MapGrid.Instance.tiles[target.X, target.Y].isTraversible()) //this is just an extra layer of precaution at this point
        {
            while (elapsedTime < timeToMove)
            {
                transform.position = Vector3.Lerp(origPos, targetPos, elapsedTime / timeToMove);
                GameManager.Instance.battleManager.setTurnArrowPosition(this.transform.position);
                elapsedTime += Time.deltaTime;
                yield return null;

            }

            transform.position = targetPos; //just to be safe
            MapGrid.Instance.tiles[startCoord.X, startCoord.Y].clearOccupant();
            MapGrid.Instance.tiles[target.X, target.Y].setOccupant(gameObject);
            grid_pos = target;

            //Debug.Log("Position is now " + position.ToString());
        }

    }
    private IEnumerator PlayQueuedRoutines(Queue<IEnumerator> coroutines)
    {
        IEnumerator currentCoroutine;
        while(coroutines.Count > 0)
        {
            currentCoroutine = coroutines.Dequeue();
            yield return StartCoroutine(currentCoroutine);
        }
    }
    public void MoveToDistantTile(Coord target)
    {
        MoveToDistantTile(target, false);
    }
    public IEnumerator MoveToDistantTile(Coord target, bool moveToAdjacent)
    {
        //moveToAdjacent is a boolean that determines if we are moveing the unit to the target tile, or simply an open tile adjacent to the target tile.
        Coord startCoord = MapGrid.Instance.worldToGridCoords(transform.position);

       //time savers/edge-cases:
        if (!moveToAdjacent && startCoord == target)
            yield break;

        if (moveToAdjacent && !target.findOpenAdjacentCoords().Any())
        {
            Debug.Log("There are no open adjacent tiles. Skipping movement.");
            yield break;

        }
        if (!moveToAdjacent && !MapGrid.Instance.tiles[target.X, target.Y].isTraversible())
        {
            Debug.Log("The target tile is not traversible! Skipping movement.");
            yield break;
            
        }

        if (moveToAdjacent && target == startCoord) //just move to first open adjacent tile.
        {
            //MoveOneStep(target.findOpenAdjacentCoords()[0]);
            yield return StartCoroutine(MoveOneStep(target.findOpenAdjacentCoords()[0]));
            yield break;
        }


        List<Node> nodeQueue = new List<Node>();
        Node[,] allNodes = new Node[MapGrid.Instance.columns, MapGrid.Instance.rows];

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
        }
        
        //A* pathfinding using Manhattan distance as heuristic
        Node n;
        do
        {
            nodeQueue = nodeQueue.OrderBy(x => x.costToStart + x.manhattanDist).ToList();       //the maps are small enough that there should not be very many nodes in this at all, thus there is not really a time concern
            n = nodeQueue.First();
            nodeQueue.Remove(n);

            if (n.coord == target)
            {
                break;
            }

            List<Node> neighbours = n.getNeighbours(allNodes);
            foreach (Node neighbour in neighbours)
            {
                if (!neighbour.visited && (neighbour.traversible || neighbour.coord == target)) //in the case that we are looking for adjacent tiles, we dont care if the target is traversible. If we dont want adjacent, this is already checked above.
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
        } while(nodeQueue.Count > 0);

        //use list from A* to trace a path (reverse order)
        if (moveToAdjacent)
            n = n.parent;
        List<Coord> pathToTarget = new List<Coord>();
        while (n.coord != startCoord)
        {
            pathToTarget.Add(n.coord);
            n = n.parent;
        }

        //for each step, get vector from current position to next adjacent tile, start coroutine
        Queue<IEnumerator> corountineQueue = new Queue<IEnumerator>();
        for (int i = pathToTarget.Count() - 1; i >=0; i--)
        {
            corountineQueue.Enqueue(MoveOneStep(pathToTarget.ElementAt(i)));
        }
        
        yield return StartCoroutine(PlayQueuedRoutines(corountineQueue));
    }
    public IEnumerator MoveTowardsTarget(Coord target)
    {
        //moves towards the target. No pathfinding, so in theory they could be making bad movement decisions but thats a problem for later, if anything
        int y_movement = target.Y - grid_pos.Y;
        int x_movement = target.X - grid_pos.X;
        int total_movement = 0;
        Coord next_tile = grid_pos;

        Queue<IEnumerator> corountineQueue = new Queue<IEnumerator>();
        while (total_movement < GameManager.MOVEMENT)
        {
            if (y_movement < 0 && MapGrid.Instance.tiles[next_tile.X, next_tile.Y - 1].isTraversible())
            {
                next_tile.Y--;
                corountineQueue.Enqueue(MoveOneStep(next_tile));
                y_movement++;
            }
            else if (y_movement > 0 && MapGrid.Instance.tiles[next_tile.X, next_tile.Y + 1].isTraversible())
            {
                next_tile.Y++;
                corountineQueue.Enqueue(MoveOneStep(next_tile));
                y_movement--;
            }
            else if (x_movement < 0 && MapGrid.Instance.tiles[next_tile.X - 1, next_tile.Y].isTraversible())
            {
                next_tile.X--;
                corountineQueue.Enqueue(MoveOneStep(next_tile));
                x_movement++;
            }
            else if (x_movement > 0 && MapGrid.Instance.tiles[next_tile.X + 1, next_tile.Y].isTraversible())
            {
                next_tile.X++;
                corountineQueue.Enqueue(MoveOneStep(next_tile));
                x_movement--;
            }
            else
                break;
            total_movement++;
        }
        yield return StartCoroutine(PlayQueuedRoutines(corountineQueue));
    }
    public int lengthOfShortestPathToAdjacent(Coord target) // returns -1 if the target is not reachable BUT NOT if the target tile itself is non-traversible.
    {
        //this COULD be cleaned up so that there is not so much overlap between this and the "move to distant tile" function
        //for the sake of my sanity at this moment, I will be copy-pasting a lot of code from the above method.
          
        //if (Mathf.Abs(position.X - target.X) + Mathf.Abs(position.Y - target.Y) > GameManager.MOVEMENT+1) 
        //    return -1;

        List<Node> nodeQueue = new List<Node>();
        Node[,] allNodes = new Node[MapGrid.Instance.columns, MapGrid.Instance.rows];

        foreach (MapTile tile in MapGrid.Instance.tiles)
        {
            Node temp = new Node();
            temp.coord.X = tile.getX();
            temp.coord.Y = tile.getY();
            temp.manhattanDist = temp.getManhattanDistanceToCoord(target);
            temp.traversible = tile.isTraversible();

            allNodes[temp.coord.X, temp.coord.Y] = temp;

            if (temp.coord == grid_pos)
            {
                temp.costToStart = 0;
                nodeQueue.Add(temp);
            }
        }

        //A* pathfinding using Manhattan distance as heuristic
        Node n;
        do
        {
            nodeQueue = nodeQueue.OrderBy(x => x.costToStart + x.manhattanDist).ToList();       //the maps are small enough that there should not be very many nodes in this at all, thus there is not really a time concern
            n = nodeQueue.First();
            nodeQueue.Remove(n);
            //Debug.Log("Checking Node: " + n.coord);
            if (n.coord == target && n.costToStart != 0) //in the case that the unit is already standing on the tile, we dont want to return -1 unless there are no open adjacent tiles. (idk, better be consisitent just in case)
            {
                
                return n.costToStart - 1;   //this is more of a safety net
            }

            List<Node> neighbours = n.getNeighbours(allNodes);
            foreach (Node neighbour in neighbours)
            {
                if (neighbour.coord == target)
                    return n.costToStart;
                
                if(!neighbour.visited && neighbour.traversible) //we add target even if that tile is not traversible because we just want to know if this tile has an adjacent reachable one
                {
                    if (neighbour.costToStart == -1 || neighbour.costToStart > n.costToStart + 1)
                    {
                        neighbour.costToStart = n.costToStart + 1;
                        //neighbour.parent = n;

                        if (!nodeQueue.Contains(neighbour))
                            nodeQueue.Add(neighbour);
                    }
                }
            }
            n.visited = true;
        } while (nodeQueue.Count > 0);

        return -1;
    }
    public List<Coord> setReachableTiles(int max_movement)
    {
        Coord temp_coord;
        GameObject temp_tile;
        List<Coord> coords = new List<Coord>() { grid_pos };

        int min_x = Math.Max(0, grid_pos.X -( max_movement+1));
        int min_y = Math.Max(0, grid_pos.Y - (max_movement+1));
        int max_x = Math.Min(MapGrid.Instance.columns - 1, grid_pos.X + (max_movement+1));
        int max_y = Math.Min(MapGrid.Instance.rows - 1, grid_pos.Y + (max_movement+1));
   
   
        for (int i = min_x; i <= max_x; i++)
        {
            for (int j = min_y; j <= max_y; j++)
            {
                if (grid_pos.manhattanDistTo(i, j) > max_movement+1 || (!MapGrid.Instance.tiles[i, j].isTraversible() && !MapGrid.Instance.tiles[i, j].hasOccupant()))
                    continue;   //if tile is out of reach, or there is some sort of obstacle? might have to change this later when obstacles are added

                temp_coord = new Coord(i, j);
                if (lengthOfShortestPathToAdjacent(temp_coord) == max_movement || (temp_coord != grid_pos && MapGrid.Instance.tiles[i, j].hasOccupant())) //tiles adjacent to walkable tiles
                {
                    temp_tile = Instantiate(GameManager.Instance.battleManager.tileVisualizer, MapGrid.Instance.gridToWorldCoords(i, j), Quaternion.identity);
                    temp_tile.SetActive(true);
                    Color tmp =  Color.red;
                    tmp.a = 0.15f; //set it to red tile
                    temp_tile.GetComponentInChildren<SpriteRenderer>().color = tmp; //set it to red tile
                    GameManager.Instance.battleManager.visibleTiles.Add(temp_tile);

                    //not added to reachable tiles list
                }
                else if (lengthOfShortestPathToAdjacent(temp_coord) < max_movement) //walkable tiles
                {
                    temp_tile = Instantiate(GameManager.Instance.battleManager.tileVisualizer, MapGrid.Instance.gridToWorldCoords(i, j), Quaternion.identity);
                    temp_tile.SetActive(true);
                    GameManager.Instance.battleManager.visibleTiles.Add(temp_tile);

                    coords.Add(temp_coord);
                }
            }
        }
        return coords;
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
            if (!(i < 0 || i >= MapGrid.Instance.columns))
                temp.Add(nodes[i, coord.Y]);
        }
        for (int i = coord.Y - 1; i <= coord.Y + 1; i += 2)
        {
            if (!(i < 0 || i >= MapGrid.Instance.rows))
                temp.Add(nodes[coord.X, i]);
        }

        return temp;
    }

}
