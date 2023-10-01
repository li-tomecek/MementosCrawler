using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionSelectMenu : MonoBehaviour
{

    public Button spellsButton;
    public Button guardButton;
    public Button meleeButton;
    GameUnit target;

    public void OnEnable()
    {
        GameManager.Instance.menuManager.setShortText("Make an action?");
        target = GameManager.Instance.getActivePlayer().GetComponent<PlayerController>().target;
        StartCoroutine(setupDefaultSelect());
    }
    IEnumerator setupDefaultSelect()
    {
        yield return 0; //has to wait a frame before we can select a new game object.....
        
        if (target != null)
        {
            spellsButton.interactable = true;
            if (!(target is PlayableUnit))
                meleeButton.interactable = true;
            EventSystem.current.SetSelectedGameObject(spellsButton.gameObject);
        }
        else
        {
            spellsButton.interactable = false;
            meleeButton.interactable = false;
            EventSystem.current.SetSelectedGameObject(guardButton.gameObject);
        }
    }
    public void OnAbilitiesButton()
    {
        GameManager.Instance.menuManager.getSpellSelectMenu().SetActive(true);
        gameObject.SetActive(false);
    }
    public void OnGuardButton()
    {
        GameManager.Instance.menuManager.setLongText("The unit will take half damage from the next attack this turn.");
        GameManager.Instance.battleManager.getActiveUnitAsPlayer().isBlocking = true;
        
        GameManager.Instance.battleManager.nextTurn();
        gameObject.SetActive(false);
    }
    public void OnMeleeButton()
    {
        PlayableUnit unit = GameManager.Instance.battleManager.getActiveUnitAsPlayer();
        unit.startTurnSequence(unit.getMelee(), target);

        gameObject.SetActive(false);
    }
    public void OnExitButton()
    {
        GameManager.Instance.menuManager.setLongText("It is " + GameManager.Instance.battleManager.getActiveUnit().name + "\'s turn. Please take an action.");
        GameManager.Instance.setMode(Mode.PLAYER_TURN);
        gameObject.SetActive(false);
    }
}
