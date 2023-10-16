using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//NOTE: AT SOME POINT, THIS LINE-DRAWING SYSTEM SHOULD BE REMOVED AND REPLACED WITH PRE-MADE TILES THAT ARE JUST TRANSPARENT SQUARES WITH A BORDER
public class MapTile
{
    // Start is called before the first frame update

    //private int x;
    //private int y;      //grid coordinates? may not need them at all.
    private Coord coord;

    //maybe want a center point so avoid doing calcualtions easch turn?

    private bool traversible = true;


    public MapTile(int x, int y)
    {
        this.coord.Y = y;
        this.coord.X = x;
    }
    public MapTile(Coord c)
    {
        this.coord = c;
    }

    //----------------------
    //  GETTERS & SETTERS  
    //----------------------
    public int getX(){ return this.coord.X;}
    public int getY(){ return this.coord.Y; }
    public Coord getCoord(){ return this.coord; }
    public bool isTraversible()
    {
        return traversible;
    }
    public void setTraversible(bool b)
    {
        traversible = b;
    }
}
