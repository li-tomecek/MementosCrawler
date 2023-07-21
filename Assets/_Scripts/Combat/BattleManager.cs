using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //-------------fields-----------------
    List<GameUnit> activeUnits;
    List<GameObject> test;

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
        GameManager.Instance.swapActiveUnit(activeUnits[activeUnits.Count - 1].gameObject); //i THINK this should work.....
        activeUnits.RemoveAt(activeUnits.Count - 1);///TODO HERE LATER
    }
}
