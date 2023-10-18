using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour
{

    public int rows = 6;
    public int columns = 10;
    public float tileSize = 1.0f;

    public float lineWidth = 0.008f;

    private Vector3[] corners;
    private List<Vector3> lineVerts;
    private MeshFilter planeMesh;
    private LineRenderer lr;


    public MapTile[,] tiles;
    public Coord[] player_spawn_tiles = new Coord[4];
    public Coord[] enemy_spawn_tiles = new Coord[4];

    public static MapGrid Instance { get; private set; }
    private void Awake()
    {
        Instance = this;

        //FIND CORNERS OF GRID
        planeMesh = GetComponent<MeshFilter>();
        corners = new Vector3[4];

        transform.position = new Vector3(0.5f * tileSize * (columns - 1), 0.5f * tileSize * (rows - 1));  //setting position of grid so bottom-left corner is on (0,0)
        transform.localScale = new Vector3((tileSize * columns) / 10, 0, (tileSize * rows) / 10);       //setting width and height of grid

        corners[2] = transform.TransformPoint(planeMesh.sharedMesh.vertices[0]);    //bottom right
        corners[3] = transform.TransformPoint(planeMesh.sharedMesh.vertices[10]);   //bottom left
        corners[1] = transform.TransformPoint(planeMesh.sharedMesh.vertices[110]);  //top right
        corners[0] = transform.TransformPoint(planeMesh.sharedMesh.vertices[120]);  //top left

        initTilesAndRenderer();
    }

    private void initTilesAndRenderer()
    {

        //INITIALISE TILE ARRAY AND CALCULATE LINE VERTICES
        tiles = new MapTile[columns, rows];
        lineVerts = new List<Vector3>();

        MapTile newTile;
        Vector3 vert = corners[3];  //BEWARE: Not sure if this just copies the values or the actual reference

        for (int i = 0; i < rows; i++)
        {
            lineVerts.Add(vert);

            for (int j = 0; j < columns; j++)
            {
                newTile = new MapTile(j, i);
                tiles[j, i] = newTile;
                vert.x += tileSize;
                //Debug.Log("New Tile: [" + j + "," + i + "]");
            }

            lineVerts.Add(vert);
            vert.x = corners[0].x;
            lineVerts.Add(vert);
            vert.y += tileSize;
        }

        //finish adding the vertices for the line renderer
        Vector3 vert2;
        vert = corners[0];
        lineVerts.Add(vert);

        for (int i = 0; i < columns; i++)
        {
            vert.x += tileSize;
            lineVerts.Add(vert);
            vert2 = vert;
            vert2.y = corners[3].y;
            lineVerts.Add(vert2);
            lineVerts.Add(vert);

        }

        //DRAW THE GRID
        lr = GetComponent<LineRenderer>();
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;

        lr.positionCount = lineVerts.Count;
        lr.SetPositions(lineVerts.ToArray());
    }

    //////////////////////////
    //COORDINATE CONVERSIONS//
    //////////////////////////
    
    //finds the centerpoint of the given tile in world coordinates
    public Vector3 gridToWorldCoords(int x, int y)
    {
        if (x < 0 || x >= columns || y < 0 || y >= rows)
        {
            Debug.Log("Invalid coordinates as input. There will be error in returned coordinates.");
        }
        Vector3 coord = new Vector3();
        coord.x = corners[3].x + ((x + 0.5f) * tileSize);
        coord.y = corners[3].y + ((y + 0.5f) * tileSize);
        coord.z = 0;

        return coord;
    }

    //finds the tile that the given point lies within. Returns -1 if the point lies outsidfe of the grid
    public Coord worldToGridCoords(Vector3 worldCoords)
    {
        //Debug.Log("TL: " + corners[0] + "\nBR: " + corners[2] + "\nCoord: " + worldCoords);
        Coord coord = new Coord(-1, -1);
        if(worldCoords.x < corners[0].x || worldCoords.x > corners[1].x || worldCoords.y > corners[0].y || worldCoords.y < corners[2].y)
        {
           // Debug.Log("Coordinate lies outside of the playing field.");
            //coord.X = -1;
            //coord.Y = -1;
            return coord;
        }
       
        coord.X = Mathf.FloorToInt((worldCoords.x - corners[3].x)/ tileSize);
        coord.Y = Mathf.FloorToInt((worldCoords.y - corners[3].y)/tileSize);

        return coord;
    }
    public Vector3[] getCorners()
    {
        return this.corners;
    }
}
