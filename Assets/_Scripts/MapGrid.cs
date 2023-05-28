using System.Collections;
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
    private MapTile[,] tiles;

    //private LineRenderer lr;


    // Start is called before the first frame update
    void Start()
    {
        planeMesh = GetComponent<MeshFilter>();
        corners = new Vector3[5];

        //init corners array
        corners[2] = transform.TransformPoint(planeMesh.sharedMesh.vertices[0]);
        corners[3] = transform.TransformPoint(planeMesh.sharedMesh.vertices[10]);
        corners[1] = transform.TransformPoint(planeMesh.sharedMesh.vertices[110]);
        corners[0] = transform.TransformPoint(planeMesh.sharedMesh.vertices[120]);
        corners[4] = corners[0];


        //determine tile width and height based on params
        tileWidth = Mathf.Abs((corners[1].x - corners[0].x)) / rows;
        tileHeight = Mathf.Abs((corners[2].y - corners[0].y)) / columns;


        //init tiles array 
        MapTile temp;
        Vector3 vert = corners[0];  //BEWARE: Not sure if this just copies the values or the actual reference

        //ASSUMPTION: starting top-right then going across each row
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                temp = new MapTile(vert, tileWidth, tileHeight);
                temp.setX(i);
                temp.setY(j);

                tiles[i, j] = temp;

                vert.x += tileWidth;

            }
            
            vert.x = corners[0].x;
            vert.y -= tileHeight;
        }

        //lr = GetComponent<LineRenderer>();

        //lr.startWidth = 0.01f;
        //lr.endWidth = 0.01f;

        //lr.positionCount = 5;
        //lr.SetPositions(corners);
       
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
