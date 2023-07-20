using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject currentMenu;
    [SerializeField]
    GameObject actionMenu;

    public GameObject getActionMenu() { return actionMenu; }

}
