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


    public const float ATK_BUFFER = 0.1f;            
    public const float ENEMY_HEALTH_C = 0.45f;      //these 4 must add up to 1
    public const float ATK_SP_C = 0.25f;
    public const float ACC_C = 0.20f;

    public const float ALLY_HEALTH_C = 0.65f;        //these 2 must add up to 1
    public const float HEAL_SP_C = 0.35f;


    //-------implemented methods----------
    public override void TakeTurn()
    {
        chooseAction();
        executeMovement();
        GameManager.Instance.getBattleManager().UseMove(selected_move, target, this);

        GameManager.Instance.getBattleManager().nextTurn();
    }
    public override List<GameUnit> getAlliesInRange()   
    {
        List<GameUnit> list = new List<GameUnit>();
        foreach (EnemyUnit unit in GameManager.Instance.getBattleManager().ActiveEnemyUnits)
        {
            int temp = controller.lengthOfShortestPathToAdjacent(unit.getController().position);
            if (temp > -1 && temp <= (GameManager.MOVEMENT))
                list.Add(unit);
        }
        return list;
    }                                                                 
    public override List<GameUnit> getEnemiesInRange()
    {
        List<GameUnit> list = new List<GameUnit>();
        foreach (PlayableUnit unit in GameManager.Instance.getBattleManager().ActivePlayerUnits)
        {
            int temp = controller.lengthOfShortestPathToAdjacent(unit.getController().position);
            if (temp > -1 && temp <= (GameManager.MOVEMENT))
                list.Add(unit);
        }

        return list;
    }

    //----------other methods--------------
    public void chooseAction()
    {
        Debug.Log("CHOOSING AN ACTION...");
        target = this;  //default value that should definitely be replaced within the following functions
        target_coord = controller.position;
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
        if(target != this)
            target_coord = target.getController().position.findOpenAdjacentCoords()[0]; //idk if this will work, but it should return the  first adjacent coordinate that isnt full.
    }
    public float find_best_attack_target(float max_desire, Move move)  //**NOTE** 1/x does not provide a negative linear relationship btwn desire and the value of x. it would intead have to be some "max value" constant - x ** TO FIX FOR SOME RELATIONSHIPS (and adjust weights accordingly)
    {
        float desire, remaining_health, remaining_sp;
        float enemy_norm;
        float sp_norm;
        float accuracy_norm;
  
        foreach (PlayableUnit unit in getEnemiesInRange())
        {
            remaining_health = Math.Max(0.0f,(unit.getHP() - (this.stats.strength + move.getPower() - unit.getStats().defense)));
            remaining_sp = getSP() - move.getSPCost();

            if (remaining_sp < 0)
                desire = 0;
            else if (remaining_health <= 0)
                desire = 1;
            else
            {
                //Normalize each "contributor" (scale 0-1) and then do a linear combination that results in a final number on a scale of 0-1
                //normalize formula -- norm(x) = (x - min_x)/(max_x - min_x)
                enemy_norm = (remaining_health - unit.getStats().maxHP) / (-unit.getStats().maxHP);    //swapping min/max values in this case because of inverse relationship with enemy health. i dunno if this will crate problems
                sp_norm = remaining_sp / getSP();   //min = 0SP max = currentSP
                accuracy_norm = move.getAccuracy() / 100.0f; //min = 0%TOCHANGE?, max = 100% accuracy

                desire = (ENEMY_HEALTH_C * enemy_norm + ATK_SP_C * sp_norm + ACC_C * accuracy_norm + ATK_BUFFER);  //where the constants add up to 1. Buffer ensures, that even if the enemy is at full health, there will be a small 'desire' to attack them
            }

            //Debug.Log("Checking move: " + move.name + " target: " + target.name + " desire: " + desire);

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
        float desire, health_norm, sp_norm, remaining_sp;
        foreach(EnemyUnit unit in getAlliesInRange())
        {
            if (unit.getHP() == unit.stats.maxHP)
                return 0;

            remaining_sp = getSP() - move.getSPCost();

            if (remaining_sp < 0)
                desire = 0;
            else
            {
                health_norm = (unit.getHP() - unit.getStats().maxHP) / (-unit.getStats().maxHP);    //swapping min and max here as well. I think this works...
                sp_norm = remaining_sp / getSP();

                desire = (ALLY_HEALTH_C * health_norm) + (HEAL_SP_C * sp_norm);
            }
            Debug.Log("Checking move: " + move.name + " Target: " + target.name + " Desire: " + desire);


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
        GameManager.Instance.getBattleManager().ActiveEnemyUnits.Add(this);
        this.controller = gameObject.GetComponent<UnitController>();
    }
}
