using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : GameUnit
{
    //------constructors-------
    public EnemyUnit() : base() { }
    public EnemyUnit(Move[] moveset, Stats stats, Coords pos) : base(moveset, stats, pos) { }


    public override void TakeTurn()
    {
        ChooseAction();
        executeMovement();

        GameManager.Instance.getBattleManager().nextTurn();
    }
    public  void ChooseAction()
    {
        Debug.Log("choosing and action...");
    }
    public void executeMovement()
    {
        Debug.Log("executing movement...");

    }
}
