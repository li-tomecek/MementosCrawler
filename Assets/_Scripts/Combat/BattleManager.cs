using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //-------------fields-----------------
    public List<GameUnit> activeUnits;
    int turn_index;
    //------constructors and start--------
    private void onAwake()
    {
        activeUnits = new List<GameUnit>();
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
}
