using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    //-------------------------
    //       GRID DATA
    //-------------------------
    public int rows = 1;
    public int columns = 1;

    [HideInInspector] public float tileWidth;
    [HideInInspector] public float tileHeight;


    // EVERYTHING PAST THIS POINT IS FROM SUMMER CHANGES

    //I am hoping that the following code creates a 2D array that stores "references" to one of the TileMaster instances
    //public TileMaster tileMaster = new TileMaster();
    //public TileMaster[,] TileTypeGrid = new TileMaster[Instance.columns, Instance.rows];
}



public struct Coords
{
    public Coords(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int X { get; }
    public int Y { get; }
    public override string ToString() => $"({X}, {Y})";
}
