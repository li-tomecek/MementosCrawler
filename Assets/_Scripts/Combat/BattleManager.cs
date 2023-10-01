using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //-------------fields-----------------
    public List<GameUnit> activeUnits;
    public List<PlayableUnit> ActivePlayerUnits;
    public List<EnemyUnit> ActiveEnemyUnits;
    [HideInInspector]
    public bool blockPlayerInputs;

    private GameUnit activeUnit;        //the unit whose turn it is currently
    private System.Random rand = new System.Random();
    int turn_index;

    public int MOVEMENT = 4;       //represents how many tiles characters may move in one turn.

    //------constructors and start--------
    private void onAwake()
    {
        activeUnits = new List<GameUnit>();
        ActivePlayerUnits = new List<PlayableUnit>();
        ActiveEnemyUnits = new List<EnemyUnit>();
    }
    //-------------get/set----------------
    public List<GameUnit> getActiveUnits() { return activeUnits; }
    public GameUnit getActiveUnit() { return activeUnit; }
    public PlayableUnit getActiveUnitAsPlayer()
    {
        if(activeUnit is PlayableUnit)
        {
            return (activeUnit as PlayableUnit);
        }
        else
        {
            throw new CustomException("The active unit is not an instance of a PlayableUnit! Trying to access a playableUnit that does not exist here.");
        }
    }

    //----------other methods-------------
    public void StartBattle(GameUnit unit)
    {
        activeUnits.Sort();

        turn_index = -1;
        activeUnit = activeUnits[0];
        nextTurn();
    }
    /**
     *  NOTE: This function is temporary!
     *  In the future, there will be a trigger to determine when a battle has started. When this happends, this trigger will call "Start Battle" adnd we will NOT have to sort here, 
     *  just add active units before the battle starts!!
     * **/
    public void AddUnit(GameUnit unit) 
    {
        activeUnits.Add(unit);
        activeUnits.Sort(); //probably a better way to do this rather that re-sort everytime a unit is inserted (could do once all units are inserted) but list is small enough that it should be negligible?
        activeUnit = activeUnits[0];
    }

    public void nextTurn()
    {
        turn_index++;
        if (turn_index >= activeUnits.Count)
            turn_index = 0;

        activeUnit = activeUnits[turn_index];
        /**if (activeUnit is PlayableUnit)
        {
            GameManager.Instance.swapActiveUnit(activeUnits[turn_index].gameObject);
            GameManager.Instance.setMode(Mode.PLAYER_TURN);
        }
        else
        {
            GameManager.Instance.getActivePlayer().GetComponent<PlayerController>().enabled = false;
            GameManager.Instance.setMode(Mode.ENEMY_TURN);

        }**/
        GameManager.Instance.menuManager.setLongText("It is now " + activeUnit.name + "'s turn.");
        activeUnit.TakeTurn();
    }

    public IEnumerator UseMove(Move move, GameUnit target, GameUnit user)
    {
        //DEAL DAMAGE 
        GameManager.Instance.menuManager.setLongText(activeUnit.gameObject.name + " used " + move.name + ".");
        if (move.getType() == MoveType.ATTACK)
        {
            if (rand.NextDouble() * 100 < move.getAccuracy())
            {
                int damage = user.getStats().strength + move.getPower() - target.getStats().defense;
                if(target is PlayableUnit)
                {
                    PlayableUnit unit = target as PlayableUnit;
                    if (unit.isBlocking)
                    {
                        damage /= 2;
                        GameManager.Instance.menuManager.setLongText("Target was blocking! Taking half damage.");
                        unit.isBlocking = false;
                    }
                
                }
                target.decreaseHP(damage);
                GameManager.Instance.menuManager.addToLongText(target.name + " took " + damage + " damage.");
            }
            else
                GameManager.Instance.menuManager.setLongText("Attack missed!");

        } 
        //RESTORE HEALTH
        else if(move.getType() == MoveType.HEAL) 
        {
            int health = user.getStats().strength + move.getPower();        //we dont care about accuracy, healing moves always hit
            target.increaseHP(health);
            GameManager.Instance.menuManager.setLongText(user.name + " has restored " + health + "HP to " + target.name + ".");
        }
        else
        {
            Debug.Log("BUFF AND DEBUFF ACTIONS HAVE NOT BEEN IMPLEMENTED YET");
        }

        //UPDATE SLIDERS IF A PLAYABLE CHARACTER IS INVOLVED
        if (user is PlayableUnit)   //user casts some spell
        {
            GameManager.Instance.menuManager.sliderCanvas.updateTargetSlider(target);
            GameManager.Instance.menuManager.sliderCanvas.updatePlayerSliders(user);
        }
        else if(target is PlayableUnit) //user is target of enemy spell
        {
            GameManager.Instance.menuManager.sliderCanvas.updatePlayerSliders(target);
        }

        yield return StartCoroutine(GameManager.Instance.menuManager.WaitForQueuedText());
    }
}
