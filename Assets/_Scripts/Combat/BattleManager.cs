using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //-------------fields-----------------
    public List<GameUnit> activeUnits;
    public List<PlayableUnit> ActivePlayerUnits;
    public List<EnemyUnit> ActiveEnemyUnits;

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
        Debug.Log("It is now " + activeUnits[turn_index].gameObject.name + "'s turn.");

        if (activeUnits[turn_index] is PlayableUnit)
            GameManager.Instance.swapActiveUnit(activeUnits[turn_index].gameObject);
        else
            GameManager.Instance.getActiveUnit().GetComponent<PlayerController>().enabled = false;

        activeUnits[turn_index].TakeTurn();
    }

    public void UseMove(Move move, GameUnit target, GameUnit user)
    {
        if(move.getType() == MoveType.ATTACK)
        {
            Debug.Log("used an attack move");
        } else if(move.getType() == MoveType.HEAL)
        {
            Debug.Log("used a healing move");
        }
        else
        {
            Debug.Log("BUFF AND DEBUFF ACTIONS HAVE NOT BEEN IMPLEMENTED YET");
        }
    }
}
