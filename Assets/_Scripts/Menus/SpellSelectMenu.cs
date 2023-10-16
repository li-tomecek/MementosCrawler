using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SpellSelectMenu : MonoBehaviour
{
    public Button spell_1_btn, spell_2_btn, spell_3_btn, exit_btn;
    public GameObject[] spell_buttons;
    PlayableUnit unit;
    GameUnit target;
    GameObject last_selected;
    int selection_index;
    public void OnEnable()
    {
        //GameManager.Instance.menuManager.setShortText("Which spell will you cast?");

        unit = GameManager.Instance.battleManager.getActiveUnitAsPlayer();
        target = GameManager.Instance.getActivePlayer().GetComponent<PlayerController>().target;

        spell_buttons = new[] { spell_1_btn.gameObject, spell_2_btn.gameObject, spell_3_btn.gameObject};

        for (int i = 0; i < unit.getMoveset().Length; i++)
        {
            //set name
            spell_buttons[i].transform.GetChild(0).GetComponent<Text>().text = string.Format("{0,-12}", unit.getMoveset()[i].name, unit.getMoveset()[i].getSPCost());
                
            //check if spell can be cast against faced target
            try
            {
                if ((target is PlayableUnit && unit.getMoveset()[i].getType() == MoveType.ATTACK) 
                    || (target is EnemyUnit && unit.getMoveset()[i].getType() == MoveType.HEAL) 
                    || unit.getSP() < unit.getMoveset()[i].getSPCost())
                    spell_buttons[i].GetComponent<Button>().interactable = false;
                else
                    spell_buttons[i].GetComponent<Button>().interactable = true;
            }
            catch (NullReferenceException e)
            {
                Debug.Log("Trying to access a target that does not exist!" + e);
            }
                
        }
        selection_index = -1;
        last_selected = exit_btn.gameObject;
        StartCoroutine(setupDefaultSelect());
    }
    IEnumerator setupDefaultSelect()
    {
        yield return 0; //has to wait a frame before we can select a new game object.....
        EventSystem.current.SetSelectedGameObject(exit_btn.gameObject);
        
        //setup default selection to be first interactable spell (or exit)
        foreach(GameObject obj in spell_buttons)
        {
            if (obj.GetComponent<Button>().interactable)
            {
                EventSystem.current.SetSelectedGameObject(obj);
                yield break;
            }

        }
    }
    public void OnSpellButton()
    {
        //selection_index = System.Array.IndexOf(spell_buttons, EventSystem.current.currentSelectedGameObject);

        if (selection_index < 0)      //just for backup. Should never execute, as it should be impossible for the player to make a selection outside of the valid range.
        {
            throw new CustomException("Player tried to select a spell that does not exist!");
        }

        unit.startTurnSequence(unit.getMoveset()[selection_index], target);   //have to have some intermediate function (startTurnSequence) as calling the Coroutine from inside this object created problems after I make it inactive!!
        gameObject.SetActive(false);
    }
    public void OnExitButton()
    {
        GameManager.Instance.menuManager.getActionSelectMenu().SetActive(true);
        gameObject.SetActive(false);
    }

    void Update()
    {
        string text = "";
        if (last_selected != EventSystem.current.currentSelectedGameObject)
        {
            last_selected = EventSystem.current.currentSelectedGameObject;
            selection_index = System.Array.IndexOf(spell_buttons, last_selected);

            if (selection_index >= 0)
                text = $"Power: {unit.getMoveset()[selection_index].getPower()}" +
                    $"\nAccuracy: {unit.getMoveset()[selection_index].getAccuracy()}" +
                    $"\nSP Cost: {unit.getMoveset()[selection_index].getSPCost()}";
            
            GameManager.Instance.getMenuManager().setShortText(text);
        }
    }
}
