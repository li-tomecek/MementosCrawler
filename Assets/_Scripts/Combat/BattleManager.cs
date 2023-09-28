using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //-------------fields-----------------
    public List<GameUnit> activeUnits;
    public List<PlayableUnit> ActivePlayerUnits;
    public List<EnemyUnit> ActiveEnemyUnits;

    private System.Random rand = new System.Random();

    int turn_index;
    [HideInInspector] public bool blockPlayerInputs;

    public int MOVEMENT = 4;       //represents how many tiles characters may move in one turn.
    //------constructors and start--------
    private void onAwake()
    {
        activeUnits = new List<GameUnit>();
        ActivePlayerUnits = new List<PlayableUnit>();
        ActiveEnemyUnits = new List<EnemyUnit>();
    }
    //-------------get/set----------------
    public List<GameUnit> getActiveUnits() { return activeUnits; }

    //----------other methods-------------
    public void setupBattle()
    {
        activeUnits.Sort();
        turn_index = -1;

        nextTurn();
    }

    public void nextTurn()
    {
        turn_index++;
        if (turn_index >= activeUnits.Count)
            turn_index = 0;
        //GameManager.Instance.menuManager.setLongText("It is now " + activeUnits[turn_index].gameObject.name + "'s turn.");

        if (activeUnits[turn_index] is PlayableUnit)
            GameManager.Instance.swapActiveUnit(activeUnits[turn_index].gameObject);
        else
            GameManager.Instance.getActiveUnit().GetComponent<PlayerController>().enabled = false;

        activeUnits[turn_index].TakeTurn();
    }

    public void UseMove(Move move, GameUnit target, GameUnit user)
    {
        GameManager.Instance.menuManager.setLongText(activeUnits[turn_index].gameObject.name + " used " + move.name + ".");
        if (move.getType() == MoveType.ATTACK)
        {
            if (rand.NextDouble() * 100 < move.getAccuracy())
            {
                int damage = user.getStats().strength + move.getPower() - target.getStats().defense;
                if(target is PlayableUnit)
                {
                    PlayableUnit unit = target as PlayableUnit;
                    if (unit.isBlocking)
                    {
                        damage /= 2;
                        GameManager.Instance.menuManager.setLongText("Target was blocking! Taking half damage.");
                        unit.isBlocking = false;
                    }
                
                }
                target.decreaseHP(damage);
                GameManager.Instance.menuManager.addToLongText(target.name + " took " + damage + " damage.");
            }
            else
                GameManager.Instance.menuManager.setLongText("Attack missed!");

        } else if(move.getType() == MoveType.HEAL) 
        {
            //we dont care about accuracy, healing moves always hit
            int health = user.getStats().strength + move.getPower();
            target.increaseHP(health);
            GameManager.Instance.menuManager.setLongText(user.name + " has restored " + health + "HP to " + target.name + ".");
        }
        else
        {
            Debug.Log("BUFF AND DEBUFF ACTIONS HAVE NOT BEEN IMPLEMENTED YET");
        }
    }
}
