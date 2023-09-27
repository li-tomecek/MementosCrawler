using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject actionMenu;
    [SerializeField]
    GameObject spellMenu;
    [SerializeField]
    TextMeshProUGUI shortText;
    [SerializeField]
    TextMeshProUGUI longText;

    public GameObject getActionSelectMenu() { return actionMenu; }
    public GameObject getSpellSelectMenu() { return spellMenu; }
    public void setShortText(string text) {     //no idea if this works
        longText.text = "";
        shortText.text = text;

    }    
    public void setLongText(string text) {
        shortText.text = "";
        longText.text = text;
    }

    public void addLineToText(TextMeshProUGUI gui, string text)
    {
        gui.text += "\n" + text;
    }

}
