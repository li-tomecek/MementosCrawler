using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//NOTE: AT SOME POINT, THIS LINE-DRAWING SYSTEM SHOULD BE REMOVED AND REPLACED WITH PRE-MADE TILES THAT ARE JUST TRANSPARENT SQUARED WITH A BORDER
public class MapTile
{
    // Start is called before the first frame update

    private int x;
    private int y;      //grid coordinates? may not need them at all.

    private bool occupied = false;


    public MapTile(int x, int y)
    {
        this.y = y;
        this.x = x;
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
    public bool isOccupied()
    {
        return occupied;
    }
    public void setX(int i)
    {
        this.x = i;
    }
    public void setY(int  i)
    {
        this.y = i;
    }
    public void setOccupied(bool occ)
    {
        occupied = occ;
    }
}
