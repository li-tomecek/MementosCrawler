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
        menuManager = gameObject.GetComponent<MenuManager>();
        battleManager = gameObject.GetComponent<BattleManager>();

        mode = Mode.PLAYER_TURN;
        //battleManager.setupBattle();
    }

    // ---- CONSTANTS ----
    public const int MAX_STAT_VALUE = 100;
    public const int MAX_SP_VALUE = 30;
    public const int MAX_HP_VALUE = 100;
    public const int MOVEMENT = 4;  //how far the unit can move in one turn

    // ---- OTHER FIELDS ----
    // -- managers 
    [HideInInspector] public MenuManager menuManager;
    [HideInInspector] public BattleManager battleManager;

    // -- tiles 
    public int rows = 1;
    public int columns = 1;
    [HideInInspector] public float tileWidth;
    [HideInInspector] public float tileHeight;

    // -- units and mode
    [SerializeField]
    GameObject activePlayerObject;
    [SerializeField] Mode mode;
    bool justSwappedModes;


    //-------METHODS-------
    public void swapActiveUnit(GameObject unit)
    {
        activePlayerObject.GetComponent<PlayerController>().enabled = false;    //this might not even be necessary anymore since i moved the checkMovementInputs to this class??
        unit.GetComponent<PlayerController>().enabled = true;
        activePlayerObject = unit;

    }

    //-------Getters and Setters-------
    public GameObject getActivePlayer() {return this.activePlayerObject; }
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

        if (mode == Mode.PLAYER_TURN)
        {
            menuManager.sliderCanvas.updatePlayerSliders(activePlayerObject.GetComponent<PlayableUnit>());
            menuManager.sliderCanvas.showPlayerSliders();

            activePlayerObject.GetComponent<PlayerController>().checkInputs();
        }

    }
}