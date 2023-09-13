using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyUnit : GameUnit
{
    //------constructors-------
    public EnemyUnit() : base() { }
    public EnemyUnit(Move[] moveset, Stats stats) : base(moveset, stats) { }


    //--move selection variables--
    private System.Random rand = new System.Random();
    
    private GameUnit target;
    private Coord target_coord = new Coord(1,1);//to remove default value;
    private Move selected_move;

    public const float ATK_K = 1;
    public const float HEAL_K = 1;
    public const float ENEMY_HEALTH_C = 0.1f;        //for now, basically putting most on a scale up to 10 (SP a little smaller scale)
    public const float ALLY_HEALTH_C = 0.1f;
    public const float ATK_SP_C = 0.25f;
    public const float HEAL_SP_C = 0.25f;
    public const float ACC_C = 0.1f;


    //-------implemented methods----------
    public override void TakeTurn()
    {
        //chooseAction();
        executeMovement();
        //GameManager.Instance.getBattleManager().UseMove(selected_move, target, this);

        GameManager.Instance.getBattleManager().nextTurn();
    }
    public override List<GameUnit> getAlliesInRange()       //positions taken from controller! its updates when the unit is moved by the controller
    {
        throw new System.NotImplementedException();
    }                                                                   //TODO !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    public override List<GameUnit> getEnemiesInRange()
    {
        throw new System.NotImplementedException();
    }

    //----------other methods--------------
    public void chooseAction()
    {
        Debug.Log("choosing and action...");

        target = this;  //default value that should definitely be replaced within the following functions
        float desire = -1.0f;

        foreach (Move m in moveset)
        {
            if (m.getType() == MoveType.ATTACK)         // ~1~ For each attack move, calculate attack desirablility for all reachable enemy units
                desire = find_best_attack_target(desire, m);

            else if (m.getType() == MoveType.HEAL)      // ~2~ For each healing move, calculate heal desirablility for all reachable allied units
                desire = find_best_heal_target(desire, m);

            //Note: desire value only replaced if a move with a higher "desireability" is found
        }

        // ~3~ Select move with the highest "desireablilty"
        //target_coord = something like getAdjacentCoords(target.getCoord())[0]; once i solve the getting the coord issue (shouldnt be too hard?)
    }

    public float find_best_attack_target(float max_desire, Move move)  //**NOTE** 1/x does not provide a negative linear relationship btwn desire and the value of x. it would intead have to be some "max value" constant - x ** TO FIX FOR SOME RELATIONSHIPS (and adjust weights accordingly)
    {
        float desire, enemy_health;
        foreach (PlayableUnit unit in getEnemiesInRange())
        {
            enemy_health = Math.Max(0.0f,(unit.getHP() - (this.stats.strength + move.getPower() - unit.getStats().defense)));
            
            desire = Math.Max(0.0f, ATK_K * (ENEMY_HEALTH_C*(GameManager.MAX_HP_VALUE - enemy_health)) * (ATK_SP_C*(GameManager.MAX_SP_VALUE - move.getSPCost())) * (ACC_C*(move.getAccuracy()))); 
            if (desire > max_desire)
            {
                max_desire = desire;
                target = unit;
                selected_move = move;
            }
            else if (desire == max_desire)
            {
                if (rand.NextDouble() < 0.5)
                {
                    max_desire = desire;
                    target = unit;
                    selected_move = move;
                }
            }
        }
        return max_desire;
    }
    public float find_best_heal_target(float max_desire, Move move)
    {
        float desire;
        foreach(EnemyUnit unit in getAlliesInRange())
        {
            desire = Math.Max(0.0f, HEAL_K * (ALLY_HEALTH_C * (GameManager.MAX_HP_VALUE - unit.getHP())) * (HEAL_SP_C * (GameManager.MAX_SP_VALUE - move.getSPCost())));

            if (desire > max_desire)
            {
                max_desire = desire;
                target = unit;
                selected_move = move;
            }
            else if (desire == max_desire)
            {
                if (rand.NextDouble() < 0.5)
                {
                    max_desire = desire;
                    target = unit;
                    selected_move = move;
                }
            }
        }

        return max_desire;
    }


    public void executeMovement()
    {
        controller.MoveToDistantTile(target_coord);
        Debug.Log("executing movement...");

    }


    public new void Start()
    {
        base.Start();
        GameManager.Instance.ActiveEnemyUnits.Add(this);
        this.controller = gameObject.GetComponent<UnitController>();
    }
}
