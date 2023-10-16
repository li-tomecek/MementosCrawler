using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Ground,
    Pillar,
    Door,
    OpenChest,
    ClosedChest
}

public class TileMaster
{
    //Children instances
    GroundTile groundTile = new GroundTile();
    PillarTile pillarTile = new PillarTile();
    DoorTile doorTile = new DoorTile();
    ChestOpenTile openChestTile = new ChestOpenTile();
    ChestClosedTile closedChestTile = new ChestClosedTile();

}

public class CustomTile 
{
    protected int movementPenalty;
    protected TileType type;
    protected bool isTraversable;
}

//are all subclasses creating new children?? m-- YES BUT THEY ARE NOT ACCESSIBLE  BC THEY ARE PRIVATE UGHHHHHHHH
public class GroundTile : CustomTile
{
    private void Start()
    {
        this.movementPenalty = 0;
        this.type = TileType.Ground;
        this.isTraversable = true;
    }
}

public class PillarTile : CustomTile
{
    private void Start()
    {
        this.movementPenalty = 0;
        this.type = TileType.Pillar;
        this.isTraversable = false;
    }
}

public class DoorTile : CustomTile
{
    private void Start()
    {
        this.movementPenalty = 0;
        this.type = TileType.Door;
        this.isTraversable = false;
    }
}

public class ChestOpenTile : CustomTile
{
    private void Start()
    {
        this.movementPenalty = 0;
        this.type = TileType.OpenChest;
        this.isTraversable = false;
    }
}

public class ChestClosedTile : CustomTile
{
    private void Start()
    {
        this.movementPenalty = 0;
        this.type = TileType.ClosedChest;
        this.isTraversable = false;
    }
}



