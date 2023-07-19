using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableUnit : GameUnit
{
    //------constructors-------
    public PlayableUnit() : base() { }
    public PlayableUnit(Move[] moveset, Stats stats, Coords pos) : base(moveset, stats, pos) { }


    protected override void ChooseAction()
    {
        throw new System.NotImplementedException();
    }

    protected override void ChooseMovement()
    {
        throw new System.NotImplementedException();
    }
}
