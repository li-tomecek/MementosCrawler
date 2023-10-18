using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//NOTE: AT SOME POINT, THIS LINE-DRAWING SYSTEM SHOULD BE REMOVED AND REPLACED WITH PRE-MADE TILES THAT ARE JUST TRANSPARENT SQUARES WITH A BORDER
public class MapTile
{
    private Coord coord;

    private bool traversible = true;
    public int movementCost = 1;
    private GameObject occupant;


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
    public GameObject getOccupant() { return this.occupant; }
    public void setOccupant(GameObject obj){
        this.occupant = obj;
        traversible = false;
    }
    public bool hasOccupant()
    {
        return !(occupant == null);
    }
    public void clearOccupant()
    {
        if(occupant != null)
        {
            occupant = null;
            traversible = true;
        }
    }
    public bool isTraversible()
    {
        return traversible;
    }
    public void setTraversible(bool b)
    {
        traversible = b;
    }
}
