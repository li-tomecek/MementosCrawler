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
    }
    public void OnGuardButton()
    {
        Debug.Log("This unit will take half damage from the next attack.");
        GameManager.Instance.getActiveUnit().GetComponent<PlayableUnit>().isBlocking = true;
    }
    public void OnWaitButton()
    {
        Debug.Log("Pressed the wait button");
    }
    public void OnExitButton()
    {
        GameManager.Instance.setMode(Mode.BATTLE_MOVE);
        gameObject.SetActive(false);
    }
}
