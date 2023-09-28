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

    public Slider playerHP_slider;
    public Slider enemyHP_slider;
    public Slider playerSP_slider;



    public float text_delay = 0.01f;
    bool typing = false;
    Queue<IEnumerator> corountineQueue = new Queue<IEnumerator>();

    //-------UPDATE AND ON AWAKE-------
    void Update()
    {
        if (!typing && corountineQueue.Count > 0)
            StartCoroutine(corountineQueue.Dequeue());
    }

    private void Awake()
    {
        //playerHP_slider = 
    }

    //-------MENUS-------
    public GameObject getActionSelectMenu() { return actionMenu; }
    public GameObject getSpellSelectMenu() { return spellMenu; }
    
    //------- TEXT -------
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

    //-------SLIDERS-------
    public void updateSliders(GameUnit target, GameUnit caster)
    {
        if (target is PlayableUnit)
        {
            playerHP_slider.value = ((float)target.getHP() / target.getStats().maxHP);
            //playerHP_slider.GetComponent<Text>().text = target.getHP() + "/" + target.getStats().maxHP;
        }
        else 
        { 
            enemyHP_slider.value = ((float)target.getHP() / target.getStats().maxHP);
            //enemyHP_slider.GetComponent<Text>().text = target.getHP() + "/" + target.getStats().maxHP;
        }
        if (caster is PlayableUnit)
        {
            playerSP_slider.value = ((float)caster.getSP() / caster.getStats().maxSP);
            //playerSP_slider.GetComponent<Text>().text = caster.getSP() + "/" + caster.getStats().maxSP;
        }
    }
}
