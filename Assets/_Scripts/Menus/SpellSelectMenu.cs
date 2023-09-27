using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpellSelectMenu: MonoBehaviour
{
    public Button spell_1_btn, spell_2_btn, spell_3_btn, spell_4_btn;
    //public GameObject[] buttons;
    public void OnEnable()
    {
        StartCoroutine(waitForFrame());

        PlayableUnit unit = GameManager.Instance.getActiveUnit().GetComponent<PlayableUnit>();  //should put exception around here ig..

        spell_1_btn.transform.GetChild(0).GetComponent<Text>().text = unit.getMoveset()[0].name;
        spell_2_btn.transform.GetChild(0).GetComponent<Text>().text = unit.getMoveset()[1].name;
        spell_3_btn.transform.GetChild(0).GetComponent<Text>().text = unit.getMoveset()[2].name;
        spell_4_btn.transform.GetChild(0).GetComponent<Text>().text = unit.getMoveset()[3].name;
    }
   IEnumerator waitForFrame()
    {
        yield return 0; //dumbass bitch has to wait a frame before we can select a new game object.....
        EventSystem.current.SetSelectedGameObject(spell_1_btn.gameObject);
    }
    public void OnButtonPress()
    {
        print("player just pressed " + EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text);

        //check for enemies in range

        //if enemies in range: select an enemy (list maybe? draw and indicator on the map to show which enemy?)

        //if no enemies: print some message? or make a noise?
        //GameManager.Instance.getBattleManager().UseMove();
        GameManager.Instance.getBattleManager().nextTurn();
        gameObject.SetActive(false);
    }
}
