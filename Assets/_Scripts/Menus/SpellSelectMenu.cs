using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SpellSelectMenu : MonoBehaviour
{
    public Button spell_1_btn, spell_2_btn, spell_3_btn, spell_4_btn;
    public GameObject[] buttons;
    PlayableUnit unit;
    GameUnit target;
    public void OnEnable()
    {
        StartCoroutine(waitForFrame());

        unit = GameManager.Instance.battleManager.getActiveUnitAsPlayer();
        target = GameManager.Instance.getActivePlayer().GetComponent<PlayerController>().target;

        buttons = new[] { spell_1_btn.gameObject, spell_2_btn.gameObject, spell_3_btn.gameObject, spell_4_btn.gameObject };

        for (int i = 0; i < unit.getMoveset().Length; i++)
        {
            buttons[i].transform.GetChild(0).GetComponent<Text>().text = unit.getMoveset()[i].name;
            try
            {
                if ((target is PlayableUnit && unit.getMoveset()[i].getType() == MoveType.ATTACK) || (target is EnemyUnit && unit.getMoveset()[i].getType() == MoveType.HEAL))
                    buttons[i].GetComponent<Button>().interactable = false;
                else
                    buttons[i].GetComponent<Button>().interactable = true;
            }
            catch (NullReferenceException e)
            {
                Debug.Log("Trying to access a target that does not exist!" + e);
            }
        }
    }
    IEnumerator waitForFrame()
    {
        yield return 0; //has to wait a frame before we can select a new game object.....
        EventSystem.current.SetSelectedGameObject(spell_1_btn.gameObject);
    }
    public void OnButtonPress()
    {
        int selection = System.Array.IndexOf(buttons, EventSystem.current.currentSelectedGameObject);

        if (selection < 0)      //just for backup. Should never execute, as it should be impossible for the player to make a selection outside of the valid range.
        {
            Debug.Log("INVALID SELECTION, SKIPPING PLAYER ACTION.");
            return;
        }

        Debug.Log("Player selected \"" + unit.getMoveset()[selection].name + "\"");

        unit.startTurnSequence(unit.getMoveset()[selection], target);   //have to have some intermediate function (startTurnSequence) as calling the Coroutine from inside this object created problems after I make it inactive!!

        gameObject.SetActive(false);
    }
}
