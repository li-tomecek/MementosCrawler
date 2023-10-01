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
        if (moveset.Length != 3)
        {
            Debug.Log("This unit's moveset must contain exactly 3 moves.");
            return;
        }
        this.moveset = moveset;
        this.stats = stats;
        this.currentHP = stats.maxHP;
        this.currentSP = stats.maxSP;
    }

    //--------Shared components---------
    protected Move[] moveset;
    protected Move meleeAttack;
    [SerializeField] protected Stats stats;
    protected int currentHP, currentSP;
    protected UnitController controller;


    // --------Shared Methods----------
    public abstract void TakeTurn();
    public abstract List<GameUnit> getAlliesInRange();
    public abstract List<GameUnit> getEnemiesInRange();

    public void Start()
    {
        GameManager.Instance.getBattleManager().AddUnit(this);
        initializeTestUnit();   //obviously temporary
        Debug.Log("added "+ gameObject.name +  " to active unit list");

        currentHP = stats.maxHP;
        currentSP = stats.maxSP;

        meleeAttack = new Move("a melee attack", MoveType.MELEE, 0, 75, 1, 0);
    }


    //----------GET/SET----------------
    //--~~getters~~--
    public Stats getStats() { return stats; }
    public int getHP() { return currentHP; }
    public int getSP() { return currentSP; }
    public Move[] getMoveset() { return moveset; }
    public UnitController getController() { return controller; }
    public Move getMelee() { return meleeAttack; }

    //--~~setters~~--
    public void setHP(int hp) { currentHP = hp; }
    public void setSP(int sp) { currentSP = sp; }
    
    public void decreaseHP(int amt)
    {
        currentHP -= amt;
        currentHP = (currentHP < 0) ? 0 : currentHP;    //think this is correct syntax?
    }
    public void increaseHP(int amt)
    {
        currentHP += amt;
        currentHP = (currentHP > stats.maxHP) ? stats.maxHP : currentHP; 
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
        moveset = new Move[3];
        Move move = new Move("atk 1", MoveType.ATTACK, 10, 100, 1, 2);
        moveset[0] = move;
        move = new Move("atk 2", MoveType.ATTACK, 20, 50, 1, 3);
        moveset[1] = move;
        move = new Move("heal 10/3", MoveType.HEAL, 10, 100, 1, 3);
        moveset[2] = move;

        stats.strength = 10;
        stats.defense = 8;
        stats.agility = 10;
        stats.maxHP = 25;
        stats.maxSP = 5;
     
    }
}
