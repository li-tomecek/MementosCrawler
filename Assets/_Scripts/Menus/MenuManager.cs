using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject actionMenu;
    [SerializeField]
    GameObject spellMenu;

    public GameObject getActionSelectMenu() { return actionMenu; }
    public GameObject getSpellSelectMenu() { return spellMenu; }

}
