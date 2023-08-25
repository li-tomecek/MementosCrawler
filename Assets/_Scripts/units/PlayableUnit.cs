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
  

    //-------implemented methods----------
    public override void TakeTurn()
    {
        GameManager.Instance.setMode(Mode.BATTLE_MOVE);

    }
    public override List<GameUnit> getAlliesInRange()
    {
        throw new System.NotImplementedException();
    }
    public override List<GameUnit> getEnemiesInRange()
    {
        throw new System.NotImplementedException();
    }

    //----------other methods--------------

    public new void Start()
    {
        base.Start();
        GameManager.Instance.ActivePlayerUnits.Add(this);
    }
}
