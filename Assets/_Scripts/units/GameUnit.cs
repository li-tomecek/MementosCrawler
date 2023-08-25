using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class GameUnit : MonoBehaviour, IComparable<GameUnit>
{
    //--------Constructors------------
    public GameUnit() { }
    public GameUnit(Move[] moveset, Stats stats)
    {
        if (moveset.Length != 4)
        {
            Debug.Log("This unit's moveset must contain exactly 4 moves.");
            return;
        }
        this.moveset = moveset;
        this.stats = stats;
        this.currentHP = stats.maxHP;
        this.currentSP = stats.maxSP;
    }

    //--------Shared components---------
    protected Move[] moveset;
    [SerializeField] protected Stats stats;
    protected int currentHP, currentSP;
    private UnitController controller;

    // --------Shared Methods----------
    public abstract void TakeTurn();
    public abstract List<GameUnit> getAlliesInRange();
    public abstract List<GameUnit> getEnemiesInRange();

    public void Start()
    {
        GameManager.Instance.getBattleManager().activeUnits.Add(this);
        initializeTestUnit();   //obviously temporary
        this.controller = gameObject.GetComponent<UnitController>();
        Debug.Log("added "+ gameObject.name +  " to active unit list");

    }


    //----------GET/SET----------------
    //--~~getters~~--
    public Stats getStats() { return stats; }
    public int getHP() { return currentHP; }
    public int getSP() { return currentSP; }
    public Move[] getMoveset() { return moveset; }
    //--~~setters~~--
    public void setHP(int hp) { currentHP = hp; }
    public void setSP(int sp) { currentSP = sp; }
    
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
        Move move = new Move("attack", MoveType.ATTACK, 10, 100, 1, 2);
        moveset[0] = move;
        move = new Move("buff", MoveType.BUFF, 10, 100, 1, 2);
        moveset[1] = move;
        move = new Move("debuff", MoveType.DEBUFF, 10, 100, 1, 2);
        moveset[2] = move;
        move = new Move("heal", MoveType.HEAL, 10, 100, 1, 2);
        moveset[3] = move;

        stats.strength = 10;
        stats.defense = 10;
        stats.agility = 10;
        stats.maxHP = 25;
        stats.maxSP = 50;
    }
}
