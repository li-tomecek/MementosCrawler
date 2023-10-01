using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionSelectMenu : MonoBehaviour
{

    public Button spellsButton;
    public Button guardButton;

    public void OnEnable()
    {
        if (GameManager.Instance.getActivePlayer().GetComponent<PlayerController>().facedEnemy() != null)
        {
            spellsButton.interactable = true;
            EventSystem.current.SetSelectedGameObject(spellsButton.gameObject);
        }
        else
        {
            spellsButton.interactable = false;
            EventSystem.current.SetSelectedGameObject(guardButton.gameObject);
        } 
    }

    public void OnAbilitiesButton()
    {
        GameManager.Instance.menuManager.setShortText("Which spell will you cast?");
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
    public void OnWaitButton()
    {
        GameManager.Instance.getBattleManager().nextTurn();
        gameObject.SetActive(false);
    }
    public void OnExitButton()
    {
        GameManager.Instance.menuManager.setLongText("It is " + GameManager.Instance.battleManager.getActiveUnit().name + "\'s turn. Please take an action.");
        GameManager.Instance.setMode(Mode.PLAYER_TURN);
        gameObject.SetActive(false);
    }
}
