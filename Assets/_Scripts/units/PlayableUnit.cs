using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableUnit : GameUnit
{
    //------constructors-------
    public PlayableUnit() : base() { }
    public PlayableUnit(Move[] moveset, Stats stats, Coords pos) : base(moveset, stats, pos) { }

    //-------implemented methods----------
    protected override void ChooseAction()
    {
        Debug.Log("A playable character is choosing an action...");
    }

    protected override void ChooseMovement()
    {
        throw new System.NotImplementedException();
    }

    //------other methods------------------
    public void initializeTestUnit()
    {
        moveset = new Move[4];
        Move move = new Move("atk 1", moveType.attack, 10, 100, 1);
        moveset[0] = move;
        move = new Move("atk 2", moveType.attack, 10, 100, 1);
        moveset[1] = move;
        move = new Move("atk 3", moveType.attack, 10, 100, 1);
        moveset[2] = move;
        move = new Move("atk 4", moveType.attack, 10, 100, 1);
        moveset[3] = move;

        stats.strength = 10;
        stats.defense = 10;
        stats.agility = 10;
        stats.maxHP = 25;
        stats.maxSP = 50;
    }

    public void Start()
    {
        initializeTestUnit();   //This is obviously temporary
    }
    public void FixedUpdate()
    {
        //NOTE: This should eventually be moved to the Player class (I think) to be called when it is the current player's turn 
        if (Input.GetKeyDown(KeyCode.P))
            ChooseAction();
    }
}
