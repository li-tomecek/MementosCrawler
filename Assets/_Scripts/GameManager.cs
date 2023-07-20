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
        mode = Mode.FREE_MOVE;
        menuManager = gameObject.GetComponent<MenuManager>();
    }

    public int rows = 1;
    public int columns = 1;

    [HideInInspector] public float tileWidth;
    [HideInInspector] public float tileHeight;

    [SerializeField]
    GameObject activeUnit;
    public Mode mode;

    MenuManager menuManager;
    
    //-------METHODS-------
    public void swapActiveUnit(GameObject unit)
    {
        activeUnit.GetComponent<PlayerController>().enabled = false;    //this might not even be necessary anymore since i moved the checkMovementInputs to this class??
        unit.GetComponent<PlayerController>().enabled = true;
        activeUnit = unit;

    }

    //-------Getters and Setters-------
    public GameObject getActiveUnit() {return this.activeUnit;}
    public MenuManager getMenuManager() {return this.menuManager; }

    //---------------------
    //       UPDATE      
    //---------------------
    void Update()  //called once per frame
    {
        if(mode == Mode.FREE_MOVE || mode == Mode.BATTLE_MOVE)
            activeUnit.GetComponent<PlayerController>().checkInputs();
        //if (mode == Mode.ACTION_SELECT)
            //menuManager.checkInputs();
    }
    
    //I am hoping that the following code creates a 2D array that stores "references" to one of the TileMaster instances
    //public TileMaster tileMaster = new TileMaster();
    //public TileMaster[,] TileTypeGrid = new TileMaster[Instance.columns, Instance.rows];
}