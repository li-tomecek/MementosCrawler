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

    [SerializeField] GameObject targetSelectionSquare;
    [SerializeField] GameObject turnArrow;
    [HideInInspector] public List<GameObject> reachableTiles = new List<GameObject>();
    public GameObject tileVisualizer;

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
    public void StartBattle()
    {
        activeUnits.Sort();

        turn_index = -1;
        activeUnit = activeUnits[0];
        nextTurn();
    }
    /**
     *  NOTE: This function is temporary!
     *  In the future, there will be a trigger to determine when a battle has started. When this happends, this trigger will call "Start Battle" adnd we will NOT have to sort here, 
     *  just add active units before the battle starts!! This function can also move all units to pre-determined start locations!
     * **/
    public void AddUnit(GameUnit unit) 
    {
        activeUnits.Add(unit);
        activeUnits.Sort(); //probably a better way to do this rather that re-sort everytime a unit is inserted (could do once all units are inserted) but list is small enough that it should be negligible?
        activeUnit = activeUnits[0];
    }

    public void nextTurn()
    {
        disableTurnArrow();
        disableSelectionSquare();
        destroyMovementVisualizer();
        GameManager.Instance.menuManager.sliderCanvas.hideSliders();

        
        //IF BATTLE IS OVER
        if(ActivePlayerUnits.Count <= 0)
        {
            //TEMP
            GameManager.Instance.menuManager.setLongText("GAME OVER!");
            return;
        }
        else if(ActiveEnemyUnits.Count <= 0)
        {
            //TEMP
            GameManager.Instance.menuManager.setLongText("BATTLE WON!");
            GameManager.Instance.setMode(Mode.FREE_MOVE);
            return;
        }


        turn_index++;
        if (turn_index >= activeUnits.Count)
            turn_index = 0;

        activeUnit = activeUnits[turn_index];
        
        GameManager.Instance.menuManager.setLongText("It is now " + activeUnit.name + "'s turn.");
        activeUnit.TakeTurn();
    }

    public IEnumerator UseMove(Move move, GameUnit target, GameUnit user)
    {

        GameManager.Instance.menuManager.sliderCanvas.updateTargetSlider(target);
        setSelectionSquarePosition(target.gameObject.transform.position);
        GameManager.Instance.menuManager.setLongText(activeUnit.gameObject.name + " used " + move.name + ".");

        //DEAL DAMAGE 
        if (move.getType() == MoveType.ATTACK || move.getType() == MoveType.MELEE)
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
                        GameManager.Instance.menuManager.addToLongText("Target was blocking! Taking half damage.");
                        unit.isBlocking = false;
                    }
                
                }
                target.decreaseHP(damage);
                user.decreaseSP(move.getSPCost());

                yield return StartCoroutine(GameManager.Instance.menuManager.WaitForQueuedText());
                yield return StartCoroutine(updateSliders(user, target));

                GameManager.Instance.menuManager.addToLongText(target.name + " took " + damage + " damage.");
            }
            else
                GameManager.Instance.menuManager.addToLongText("Attack missed!");

        } 
        //RESTORE HEALTH
        else if(move.getType() == MoveType.HEAL) 
        {
            int health = user.getStats().strength + move.getPower();        //we dont care about accuracy, healing moves always hit
            target.increaseHP(health);
            user.decreaseSP(move.getSPCost());

            yield return StartCoroutine(GameManager.Instance.menuManager.WaitForQueuedText());
            yield return StartCoroutine(updateSliders(user, target));

            GameManager.Instance.menuManager.setLongText(user.name + " has restored " + health + "HP to " + target.name + ".");
        }
        else
        {
            Debug.Log("BUFF AND DEBUFF ACTIONS HAVE NOT BEEN IMPLEMENTED YET");
        }
        yield return StartCoroutine(GameManager.Instance.menuManager.WaitForQueuedText());

        //IF TARGET HAS BEEN KILLED
        if(target.getHP() <= 0)
        {
            activeUnits.Remove(target);
            if (target is PlayableUnit)
                ActivePlayerUnits.Remove(target as PlayableUnit);
            else
                ActiveEnemyUnits.Remove(target as EnemyUnit);

            target.gameObject.SetActive(false);
            MapGrid.Instance.tiles[target.getController().position.X, target.getController().position.Y].setTraversible(true);
            
            //would maybe play some noise here?
        }

    }
    IEnumerator updateSliders(GameUnit user, GameUnit target)
    {
        //UPDATE SLIDERS IF A PLAYABLE CHARACTER IS INVOLVED
        if (user is PlayableUnit)   //user casts some spell
        {
            GameManager.Instance.menuManager.sliderCanvas.updatePlayerSliders(user);
        }
        GameManager.Instance.menuManager.sliderCanvas.updateTargetSlider(target);   //always update target slider
        yield return new WaitForSeconds(0.7f);  //wait to see updated sliders
    }

    // -- visual game objects and sprites --
    public void setSelectionSquarePosition(Vector3 position)
    {
        targetSelectionSquare.SetActive(true);
        targetSelectionSquare.transform.position = position;
    }
    public void disableSelectionSquare()
    {
        targetSelectionSquare.SetActive(false); ;
    }
    public void setTurnArrowPosition(Vector3 position)
    {
        turnArrow.SetActive(true);
        turnArrow.transform.position = position + (Vector3.up * 0.1f);
    }
    public void disableTurnArrow()
    {
        turnArrow.SetActive(false);
    }
    
    public void destroyMovementVisualizer()
    {
        foreach (GameObject obj in reachableTiles)
            Destroy(obj);
        reachableTiles.Clear();
    }
}
