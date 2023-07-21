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
        mode = Mode.BATTLE_MOVE;
        menuManager = gameObject.GetComponent<MenuManager>();
        battleManager = gameObject.GetComponent<BattleManager>();
    }

    public int rows = 1;
    public int columns = 1;

    [HideInInspector] public float tileWidth;
    [HideInInspector] public float tileHeight;

    [SerializeField]
    GameObject activeUnit;
    [SerializeField] Mode mode;
    bool justSwappedModes;

    MenuManager menuManager;
    BattleManager battleManager;
    
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
    public BattleManager getBattleManager() {return this.battleManager; }
    public Mode getMode() {return this.mode; }
    public void setMode(Mode m)
    {
        this.mode = m;
        justSwappedModes = true;
    }

    //---------------------
    //       UPDATE      
    //---------------------
    void Update()  //called once per frame
    {
        if (justSwappedModes)
        { 
            justSwappedModes = false;
            return;          //ensures one frame delay between mode swaps
        }

        if (mode == Mode.FREE_MOVE || mode == Mode.BATTLE_MOVE)
        {
            activeUnit.GetComponent<PlayerController>().checkInputs();
        }

    }
    
    //I am hoping that the following code creates a 2D array that stores "references" to one of the TileMaster instances
    //public TileMaster tileMaster = new TileMaster();
    //public TileMaster[,] TileTypeGrid = new TileMaster[Instance.columns, Instance.rows];
}