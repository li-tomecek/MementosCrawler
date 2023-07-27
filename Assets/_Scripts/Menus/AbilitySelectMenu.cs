using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilitySelectMenu : MonoBehaviour
{
    public GameObject action_1_btn, action_2_btn, action_3_btn, action_4_btn;
    public GameObject[] buttons;
    public void OnEnable()
    {
  
        //EventSystem.current.SetSelectedGameObject(action_1_btn);//idk why this isnt working :)
        PlayableUnit unit = GameManager.Instance.getActiveUnit().GetComponent<PlayableUnit>();  //should put exception around here ig..

        action_1_btn.transform.GetChild(0).GetComponent<Text>().text = unit.getMoveset()[0].name;
        action_2_btn.transform.GetChild(0).GetComponent<Text>().text = unit.getMoveset()[1].name;
        action_3_btn.transform.GetChild(0).GetComponent<Text>().text = unit.getMoveset()[2].name;
        action_4_btn.transform.GetChild(0).GetComponent<Text>().text = unit.getMoveset()[3].name;
    }

    public void OnButtonPress()
    {
        print("player just pressed " + EventSystem.current.currentSelectedGameObject.name);
        
        //check for enemies in range

        //if enemies in range: select an enemy (list maybe? draw and indicator on the map to show which enemy?)

        //if no enemies: print some message? or make a noise?
    }
}
