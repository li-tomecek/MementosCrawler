using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour
{

    public int rows = 1;
    public int columns = 1;

    private float tileWidth;
    private float tileHeight;

    private MeshFilter planeMesh;
    private Vector3[] corners;
    private List<Vector3> lineVerts = new List<Vector3>();

    private MapTile[,] tiles;
    private LineRenderer lr;


    // Start is called before the first frame update
    void Start()
    {
        planeMesh = GetComponent<MeshFilter>();
        corners = new Vector3[4];

        //init corners array
        corners[2] = transform.TransformPoint(planeMesh.sharedMesh.vertices[0]);
        corners[3] = transform.TransformPoint(planeMesh.sharedMesh.vertices[10]);
        corners[1] = transform.TransformPoint(planeMesh.sharedMesh.vertices[110]);
        corners[0] = transform.TransformPoint(planeMesh.sharedMesh.vertices[120]);


        //determine tile width and height based on params
        tileWidth = Mathf.Abs((corners[1].x - corners[0].x)) / columns;
        tileHeight = Mathf.Abs((corners[2].y - corners[0].y)) / rows;


        //init tiles array
        tiles = new MapTile[rows, columns];
        MapTile newTile;
        Vector3 vert = corners[0];  //BEWARE: Not sure if this just copies the values or the actual reference
        
        for (int i = 0; i < rows; i++)
        {
            lineVerts.Add(vert);

            for (int j = 0; j < columns; j++)
            {
                newTile = new MapTile(vert, tileWidth, tileHeight, i, j);
                vert.x += tileWidth;
            }

            lineVerts.Add(vert);
            vert.x = corners[0].x;
            lineVerts.Add(vert);
            vert.y -= tileHeight;
        }

        //finish adding the vertices for the line renderer
        Vector3 vert2;
        vert = corners[3];
        lineVerts.Add(vert);

        for (int i = 0; i < columns; i++)
        {
            vert.x += tileWidth;
            lineVerts.Add(vert);
            vert2 = vert;
            vert2.y = corners[0].y;
            lineVerts.Add(vert2);
            lineVerts.Add(vert);
            
        }



        //draw the grid
        lr = GetComponent<LineRenderer>();
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;

        lr.positionCount = lineVerts.Count;
        lr.SetPositions(lineVerts.ToArray());


    }

    // Update is called once per frame
    void Update(){}
    
    
    //////////////////////////
    //COORDINATE CONVERSIONS//
    //////////////////////////
    private Vector3 gridToWorldCoords(int x, int y)
    {
        return Vector3.one;
    }

    private Vector2Int worldToGridCoords(Vector3 worldCoords)
    {
        return Vector2Int.one;
    }
}
