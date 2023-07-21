using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionSelectMenu : MonoBehaviour
{

    public GameObject personaButton;
    public void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(personaButton);
    }

    public void OnPersonaButton()
    {
        Debug.Log("Pressed the persona button");
        //TODO: open a new menu (the attack options menu)
        GameManager.Instance.getMenuManager().getAbilitySelectMenu().SetActive(true);
        gameObject.SetActive(false);
    }
    public void OnGuardButton()
    {
        Debug.Log("This unit will take half damage from the next attack.");
        GameManager.Instance.getActiveUnit().GetComponent<PlayableUnit>().isBlocking = true;
        gameObject.SetActive(false);
    }
    public void OnWaitButton()
    {
        GameManager.Instance.getBattleManager().nextTurn();
        gameObject.SetActive(false);
    }
    public void OnExitButton()
    {
        GameManager.Instance.setMode(Mode.BATTLE_MOVE);
        gameObject.SetActive(false);
    }
}
