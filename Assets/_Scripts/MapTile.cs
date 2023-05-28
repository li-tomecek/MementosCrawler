using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile
{
    // Start is called before the first frame update

    private int x;
    private int y;      //grid coordinates? may not need them at all.

    public Vector3[] cornerVertices;

    private bool isOccupied;
    private int contentCode;
    private GameObject actor;


    public MapTile(Vector3 vert, float tileWidth, float tileHeight, int i, int j)
    {
        cornerVertices = new Vector3[4];
        cornerVertices[0] = vert;
        cornerVertices[1] = new Vector3(vert.x + tileWidth, vert.y, 0);
        cornerVertices[2] = new Vector3(vert.x + tileWidth, vert.y - tileHeight, 0);
        cornerVertices[3] = new Vector3(vert.x, vert.y - tileHeight, 0);
        y = i;
        x = j;
    }

    //----------------------
    //  GETTERS & SETTERS  
    //----------------------
    public int getX()
    {
        return this.x;
    }
    public int getY()
    {
        return this.y;
    }
    public void setX(int i)
    {
        this.x = i;
    }
    public void setY(int  i)
    {
        this.y = i;
    }
}
