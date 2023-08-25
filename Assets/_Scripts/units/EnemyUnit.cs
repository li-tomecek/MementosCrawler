using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : GameUnit
{
    //------constructors-------
    public EnemyUnit() : base() { }
    public EnemyUnit(Move[] moveset, Stats stats) : base(moveset, stats) { }

    private UnitController controller;


    //-------implemented methods----------
    public override void TakeTurn()
    {
        executeMovement();
        chooseAction();

        GameManager.Instance.getBattleManager().nextTurn();
    }
    public override List<GameUnit> getAlliesInRange()       //positions taken from controller! its updates when the unit is moved by the controller
    {
        throw new System.NotImplementedException();
    }
    public override List<GameUnit> getEnemiesInRange()
    {
        throw new System.NotImplementedException();
    }

    //----------other methods--------------
    public void chooseAction()
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
