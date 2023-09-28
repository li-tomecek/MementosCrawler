using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionSelectMenu : MonoBehaviour
{

    public GameObject spellsButton;
    public void OnEnable()
    {
        //tochange! dont want it hovering over spells button when you are unable to use spells
        EventSystem.current.SetSelectedGameObject(spellsButton);
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
        GameManager.Instance.getActiveUnit().GetComponent<PlayableUnit>().isBlocking = true;
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
        GameManager.Instance.menuManager.setLongText("It is " + GameManager.Instance.getActiveUnit().name + "\'s turn. Please take an action.");
        GameManager.Instance.setMode(Mode.BATTLE_MOVE);
        gameObject.SetActive(false);
    }
}
