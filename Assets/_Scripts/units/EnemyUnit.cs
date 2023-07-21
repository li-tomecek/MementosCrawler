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
    }
    public  void ChooseAction()
    {
        print("choosing and action...");
    }
    public  void executeMovement()
    {
        print("executing movement...");

    }
}
