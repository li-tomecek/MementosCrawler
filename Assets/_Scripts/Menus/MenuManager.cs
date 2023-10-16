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
    
    public SliderCanvas sliderCanvas;
    
    [SerializeField]
    TextMeshProUGUI shortText;
    [SerializeField]
    TextMeshProUGUI longText;


    public float text_delay = 0.01f;
    bool typing = false;
    Queue<IEnumerator> corountineQueue = new Queue<IEnumerator>();

    //-------UPDATE AND ON AWAKE-------
    void Update()
    {
        if (!typing && corountineQueue.Count > 0)         //we no longer do this in update! We call this after we have queued the appropriate text?
            StartCoroutine(corountineQueue.Dequeue());
    }

    //-------MENUS-------
    public GameObject getActionSelectMenu() { return actionMenu; }
    public GameObject getSpellSelectMenu() { return spellMenu; }
    
    //------- TEXT -------
    public void clearText() { shortText.text = ""; longText.text = ""; }
    public void setShortText(string text)
    {
        longText.gameObject.SetActive(false);
        shortText.gameObject.SetActive(true);
        
        //corountineQueue.Enqueue(AutoTypeText(shortText, text, true));
        shortText.text = text;
    }
    public void setLongText(string text)
    {
        shortText.gameObject.SetActive(false);  //Think there may be problems with setting these values here and not when the actual coroutine starts. If there is a queue of text with short & long mixed?
        longText.gameObject.SetActive(true);

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
        yield return new WaitForSeconds(0.8f); //wait for at least this long between lines.
        typing = false;
    }

    public IEnumerator WaitForQueuedText()
    {
        while (typing || corountineQueue.Count > 0)
        {
            yield return null;  //hopefully we dont end up in infinite loop.
        }
    }
}
