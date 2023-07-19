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
        mode = Mode.free_movement;
    }

    //-------GRID DATA-------

    public int rows = 1;
    public int columns = 1;

    [HideInInspector] public float tileWidth;
    [HideInInspector] public float tileHeight;

    //-------PLAYER AND MODE CONTROL-------
    GameObject[] ActiveUnits; //is this needed? not doing anything with this yet
    [SerializeField]
    GameObject activeUnit;
    public Mode mode;

    
    //-------METHODS-------
    public void swapActiveUnit(GameObject unit)
    {
        activeUnit.GetComponent<PlayerController>().enabled = false;    //this might not even be necessary anymore since i moved the checkMovementInputs to this class??
        unit.GetComponent<PlayerController>().enabled = true;
        activeUnit = unit;

    }

    //---------------------
    //       UPDATE      
    //---------------------
    void FixedUpdate()  //called once per frame
    {
        if(mode == Mode.free_movement || mode == Mode.battle_movement)
            activeUnit.GetComponent<PlayerController>().checkInputs();
        if (mode == Mode.move_select)
            return; //replace with menuManager.checkInputs();
    }

    
    
    //I am hoping that the following code creates a 2D array that stores "references" to one of the TileMaster instances
    //public TileMaster tileMaster = new TileMaster();
    //public TileMaster[,] TileTypeGrid = new TileMaster[Instance.columns, Instance.rows];
}