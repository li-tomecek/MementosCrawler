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
    }
    public void OnBlockButton()
    {
        Debug.Log("Pressed the block button");
    }
    public void OnWaitButton()
    {
        Debug.Log("Pressed the wait button");
    }
    public void OnExitButton()
    {
        gameObject.SetActive(false);
        GameManager.Instance.mode = Mode.BATTLE_MOVE;
    }
}
