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

    public float text_delay = 0.01f;
    bool typing = false;
    Queue<IEnumerator> corountineQueue = new Queue<IEnumerator>();

    //chekcing every frame. dont think there is another way to do this sadly.
    void Update()
    {
        if (!typing && corountineQueue.Count > 0)
            StartCoroutine(corountineQueue.Dequeue());
    }
    public GameObject getActionSelectMenu() { return actionMenu; }
    public GameObject getSpellSelectMenu() { return spellMenu; }
    public void clearText() { shortText.text = ""; longText.text = ""; }
    public void setShortText(string text)
    {  
        longText.text = "";
        shortText.text = text;
    }
    public void setLongText(string text)
    {
        shortText.text = "";
        corountineQueue.Enqueue(AutoTypeText(longText, text, true));
    }
    public void addToLongText(string text)
    {
        corountineQueue.Enqueue(AutoTypeText(longText, text, false));
    }
    IEnumerator AutoTypeText(TextMeshProUGUI gui, string input, bool clear)
    {
        typing = true;
        if (clear)
            gui.text = "";
        foreach (char c in input)
        {
            gui.text += c;
            yield return new WaitForSeconds(text_delay);
        }
        gui.text += "\n";
        yield return new WaitForSeconds(0.7f); //wait for at least one second between lines.
        typing = false;
    }
}
