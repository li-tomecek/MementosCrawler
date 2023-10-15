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
        controller.reachableCoords = controller.getReachableCoords(GameManager.MOVEMENT);
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
    public void startTurnSequence(Move move, GameUnit target)   //This method is an intermediate method (ik, messy) as calling the coroutine from within the menu causes problems once the menu is deactivated!
    {
        StartCoroutine(ExecuteTurnSequence(move, target));
    }
    IEnumerator ExecuteTurnSequence(Move move, GameUnit target)
    {
        //unit "takes action" and appropriate text is queued and displayed
        yield return StartCoroutine(GameManager.Instance.battleManager.UseMove(move, target, this));

        //change the turn
        GameManager.Instance.getBattleManager().nextTurn();
    }

    //---- START ----
    public new void Start()
    {
        base.Start();
        GameManager.Instance.getBattleManager().ActivePlayerUnits.Add(this);
        this.controller = gameObject.GetComponent<PlayerController>();
    }
}
