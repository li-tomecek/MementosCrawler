using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class GameUnit : MonoBehaviour, IComparable<GameUnit>
{
    //--------Constructors------------
    public GameUnit() { }
    public GameUnit(Move[] moveset, Stats stats, Coords pos)
    {
        if (moveset.Length != 4)
        {
            Debug.Log("This unit's moveset must contain 4 moves.");
            return;
        }
        this.moveset = moveset;
        this.stats = stats;
        this.position = pos;
        this.currentHP = stats.maxHP;
        this.currentSP = stats.maxSP;
    }

    //--------Shared components---------
    protected Move[] moveset;
    [SerializeField] protected Stats stats;
    protected int currentHP, currentSP;
    protected Coords position;

    // --------Shared Methods----------
    public abstract void TakeTurn();

    public void Start()
    {
        GameManager.Instance.getBattleManager().activeUnits.Add(this);
        initializeTestUnit();   //obviously temporary
        Debug.Log("added "+ gameObject.name +  " to active unit list");
    }

    //----------GET/SET----------------
    //--~~getters~~--
    public Coords getPosition() { return position; }
    public Stats getStats() { return stats; }
    public int getHP() { return currentHP; }
    public int getSP() { return currentSP; }
    public Move[] getMoveset() { return moveset; }
    //--~~setters~~--
    public void setHP(int hp) { currentHP = hp; }
    public void setSP(int sp) { currentSP = sp; }
    public void setPosition(int x, int y) { position.X = x; position.Y = y; }
    
    
    public void decreaseHP(int amt)
    {
        currentHP -= amt;
        currentHP = (currentHP < 0) ? 0 : currentHP;    //think this is correct syntax?
    }
    public void decreaseSP(int amt)
    {
        currentSP -= amt;
        currentSP = (currentSP < 0) ? 0 : currentSP;   
    }
    public int CompareTo(GameUnit other)
    {
        return (this.stats.agility.CompareTo(other.stats.agility) * -1);  //sorting lists in descending agility order
    }

    public void initializeTestUnit()
    {
        moveset = new Move[4];
        Move move = new Move("atk 1", MoveType.ATTACK, 10, 100, 1);
        moveset[0] = move;
        move = new Move("atk 2", MoveType.ATTACK, 10, 100, 1);
        moveset[1] = move;
        move = new Move("atk 3", MoveType.ATTACK, 10, 100, 1);
        moveset[2] = move;
        move = new Move("atk 4", MoveType.ATTACK, 10, 100, 1);
        moveset[3] = move;

        stats.strength = 10;
        stats.defense = 10;
        stats.agility = 10;
        stats.maxHP = 25;
        stats.maxSP = 50;
    }
}
