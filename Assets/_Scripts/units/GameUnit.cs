using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class GameUnit : MonoBehaviour, IComparable<GameUnit>
{
    //--------Constructors------------
    public GameUnit() { }
    public GameUnit(Move[] moveset, Stats stats, Coord pos)
    {
        if (moveset.Length != 4)
        {
            Debug.Log("This unit's moveset must contain exactly 4 moves.");
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
    protected Coord position;

    // --------Shared Methods----------
    public abstract void TakeTurn();

    public void Start()
    {
        GameManager.Instance.getBattleManager().activeUnits.Add(this);
        initializeTestUnit();   //obviously temporary
        Debug.Log("added "+ gameObject.name +  " to active unit list");
    }

    protected List<GameUnit> getAlliesInRange()
    {
        List<GameUnit> allies = new List<GameUnit>();
        foreach (GameUnit unit in GameManager.Instance.getBattleManager().activeUnits)
        {
            if ((this is EnemyUnit && unit is EnemyUnit) || (this is PlayableUnit && unit is PlayableUnit))
            {
                //TODO: check if they are within range, then check if an adjacent space is open. assuming this open space is always reachable?
                allies.Add(unit);
            }
        }
        return allies;
    }

    protected List<GameUnit> getEnemiesInRange()
    {
        List<GameUnit> enemies = new List<GameUnit>();
        foreach (GameUnit unit in GameManager.Instance.getBattleManager().activeUnits)
        {
            if ((this is EnemyUnit && unit is PlayableUnit) || (this is PlayableUnit && unit is EnemyUnit))
            {
                //TODO: check if they are within range, then check if an adjacent space is open. assuming this open space is always reachable?
                enemies.Add(unit);
            }
        }
        return enemies;
    }

    //----------GET/SET----------------
    //--~~getters~~--
    public Coord getPosition() { return position; }
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
