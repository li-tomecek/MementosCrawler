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

public class TileMaster : MonoBehaviour
{
    protected int movementPenalty;
    protected TileType type;
    protected bool isTraversable;

    //Children instances
    GroundTile groundTile = new GroundTile();
    PillarTile pillarTile = new PillarTile();
    DoorTile doorTile = new DoorTile();
    ChestOpenTile openChestTile = new ChestOpenTile();
    ChestClosedTile closedChestTile = new ChestClosedTile();

}

//are all subclasses creating new children?? m-- YES BUT THEY ARE NOT ACCESSIBLE  BC THEY ARE PRIVATE UGHHHHHHHH
public class GroundTile : TileMaster
{
    private void Start()
    {
        this.movementPenalty = 0;
        this.type = TileType.Ground;
        this.isTraversable = true;
    }
}

public class PillarTile : TileMaster
{
    private void Start()
    {
        this.movementPenalty = 0;
        this.type = TileType.Pillar;
        this.isTraversable = false;
    }
}

public class DoorTile : TileMaster
{
    private void Start()
    {
        this.movementPenalty = 0;
        this.type = TileType.Door;
        this.isTraversable = false;
    }
}

public class ChestOpenTile : TileMaster
{
    private void Start()
    {
        this.movementPenalty = 0;
        this.type = TileType.OpenChest;
        this.isTraversable = false;
    }
}

public class ChestClosedTile : TileMaster
{
    private void Start()
    {
        this.movementPenalty = 0;
        this.type = TileType.ClosedChest;
        this.isTraversable = false;
    }
}



