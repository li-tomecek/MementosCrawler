using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : GameUnit
{
    //------constructors-------
    public EnemyUnit() : base() { }
    public EnemyUnit(Move[] moveset, Stats stats, Coords pos) : base(moveset, stats, pos) { }


    protected override void ChooseAction()
    {
        throw new System.NotImplementedException();
    }

    protected override void ChooseMovement()
    {
        throw new System.NotImplementedException();
    }
}
