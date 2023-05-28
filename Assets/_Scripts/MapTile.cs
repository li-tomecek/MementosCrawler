using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile
{
    // Start is called before the first frame update

    private int x;
    private int y;      //grid coordinates? may not need them at all.


    private LineRenderer lr;

    public Vector3[] cornerVertices;

    //private bool isOccupied;
    private int contentCode;
    private GameObject actor;
    

    public MapTile(Vector3 vert, float tileWidth, float tileHeight)
    {
        cornerVertices = new Vector3[5];
        cornerVertices[0] = vert;
        cornerVertices[1] = new Vector3(vert.x + tileWidth, vert.y, 0);
        cornerVertices[2] = new Vector3(vert.x + tileWidth, vert.y - tileHeight, 0);
        cornerVertices[3] = new Vector3(vert.x, vert.y - tileHeight, 0);
        cornerVertices[4] = cornerVertices[0];
    }
    
    
    void Start()
    {
        //cornerVertices = new Vector3[4];
        lr = new LineRenderer();
        //lr = GetComponent<LineRenderer>();
        
        drawBorder();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //--------------------
    //  Public Functions  
    //--------------------
    public void drawBorder()
    {
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;

        lr.positionCount = 5;
        lr.SetPositions(cornerVertices);


        Debug.Log("bitchasshoe");
   
    }


    ////////////////
    //  GETTERS   //
    ////////////////
    public int getX()
    {
        return this.x;
    }
    public int getY()
    {
        return this.y;
    }

    ////////////////
    //  SETTERS   //
    ////////////////
    public void setX(int i)
    {
        this.x = i;
    }
    public void setY(int  i)
    {
        this.y = i;
    }
}
