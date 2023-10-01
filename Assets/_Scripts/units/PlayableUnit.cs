using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableUnit : GameUnit
{
    //-----------constructors------------
    public PlayableUnit() : base() { }
    public PlayableUnit(Move[] moveset, Stats stats) : base(moveset, stats) { }

    //-------------fields-----------------
    [HideInInspector] public bool isBlocking;


    //---- IMPLEMENTED METHODS ----
    public override void TakeTurn()
    {
        GameManager.Instance.swapActiveUnit(this.gameObject);
        GameManager.Instance.setMode(Mode.PLAYER_TURN);

        isBlocking = false;
    }
    public override List<GameUnit> getAlliesInRange()
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
    public override List<GameUnit> getEnemiesInRange()
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

    //---- UNIQUE METHODS ----
    
    //---- START ----
    public new void Start()
    {
        base.Start();
        GameManager.Instance.getBattleManager().ActivePlayerUnits.Add(this);
        this.controller = gameObject.GetComponent<PlayerController>();
    }
}
