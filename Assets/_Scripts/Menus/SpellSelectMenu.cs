using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpellSelectMenu : MonoBehaviour
{
    public Button spell_1_btn, spell_2_btn, spell_3_btn, spell_4_btn;
    public GameObject[] buttons;
    PlayableUnit unit;
    public void OnEnable()
    {
        StartCoroutine(waitForFrame());

        unit = GameManager.Instance.battleManager.getActiveUnitAsPlayer();  //should put exception around here ig..

        spell_1_btn.transform.GetChild(0).GetComponent<Text>().text = unit.getMoveset()[0].name;
        spell_2_btn.transform.GetChild(0).GetComponent<Text>().text = unit.getMoveset()[1].name;
        spell_3_btn.transform.GetChild(0).GetComponent<Text>().text = unit.getMoveset()[2].name;
        spell_4_btn.transform.GetChild(0).GetComponent<Text>().text = unit.getMoveset()[3].name;

        buttons = new[] { spell_1_btn.gameObject, spell_2_btn.gameObject, spell_3_btn.gameObject, spell_4_btn.gameObject};

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


        //check for adjacent enemies

        //if enemies in range: select an enemy (list maybe? draw and indicator on the map to show which enemy?)

        //if no enemies: print some message? or make a noise?
        //GameManager.Instance.getBattleManager().UseMove();
        GameManager.Instance.getBattleManager().nextTurn();
        gameObject.SetActive(false);
    }
}
