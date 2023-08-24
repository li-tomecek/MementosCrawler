using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableUnit : GameUnit
{
    //-----------constructors------------
    public PlayableUnit() : base() { }
    public PlayableUnit(Move[] moveset, Stats stats, Coord pos) : base(moveset, stats, pos) { }

    //-------------fields-----------------
    [HideInInspector] public bool isBlocking;

    //-------implemented methods----------
    public override void TakeTurn()
    {
        GameManager.Instance.setMode(Mode.BATTLE_MOVE);
    }

    //----------other methods--------------

    public new void Start()
    {
        base.Start();
        GameManager.Instance.ActivePlayerUnits.Add(this);
    }
}
