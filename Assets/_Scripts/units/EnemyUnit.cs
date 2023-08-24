using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : GameUnit
{
    //------constructors-------
    public EnemyUnit() : base() { }
    public EnemyUnit(Move[] moveset, Stats stats, Coord pos) : base(moveset, stats, pos) { }

    private UnitController controller;


    public override void TakeTurn()
    {
        executeMovement();
        chooseAction();

        GameManager.Instance.getBattleManager().nextTurn();
    }
    public  void chooseAction()
    {
        Debug.Log("choosing and action...");
    }
    public void executeMovement()
    {
        Coord coord = new Coord(1,1);
        controller.MoveToDistantTile(coord);

        Debug.Log("executing movement...");
    }


    public new void Start()
    {
        base.Start();
        GameManager.Instance.ActiveEnemyUnits.Add(this);
        this.controller = gameObject.GetComponent<UnitController>();
    }
}
