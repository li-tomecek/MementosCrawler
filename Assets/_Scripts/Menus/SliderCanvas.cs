using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderCanvas : MonoBehaviour
{
    //----FIELDS------
    public CanvasGroup playerGroup;
    public CanvasGroup targetGroup;

    Slider playerHP_slider;  //text value associated with these as well
    Slider targetHP_slider;
    Slider playerSP_slider;

    Text caster_name;
    Text target_name;


    //----START AND UPDATE------
    void Start()
    {
        playerHP_slider = playerGroup.transform.GetChild(0).GetComponent<Slider>();
        playerSP_slider = playerGroup.transform.GetChild(1).GetComponent<Slider>();
        caster_name = playerGroup.transform.GetChild(2).GetComponent<Text>();

        targetHP_slider = targetGroup.transform.GetChild(0).GetComponent<Slider>();
        target_name = targetGroup.transform.GetChild(1).GetComponent<Text>();

        hideSliders();
    }

    //void Update() {}

    //---- UPDATE SLIDERS ----
    public void updatePlayerSliders(GameUnit unit)
    {
        caster_name.text = unit.name;
        playerHP_slider.value = ((float)unit.getHP() / unit.getStats().maxHP);
        playerSP_slider.value = ((float)unit.getSP() / unit.getStats().maxSP);

        playerHP_slider.transform.GetComponentInChildren<Text>().text = string.Format("{0,3}/{0,-3}", unit.getHP(), unit.getStats().maxHP);
        playerSP_slider.transform.GetComponentInChildren<Text>().text = string.Format("{0,3}/{0,-3}", unit.getSP(), unit.getStats().maxSP);
    }
    public void updateTargetSlider(GameUnit target)
    {
        target_name.text = target.name;
        targetHP_slider.value = ((float)target.getHP() / target.getStats().maxHP);
        targetHP_slider.transform.GetComponentInChildren<Text>().text = string.Format("{0,3}/{0,-3}", target.getHP(), target.getStats().maxHP);
    }

    //--- CHANGE VISIBILITY ----
    public void hideSliders()
    {
        playerGroup.alpha = 0;
        targetGroup.alpha = 0;
    }
    public void showSliders()
    {
        playerGroup.alpha = 1;
        targetGroup.alpha = 1;
    }
    public void hideTargetSlider()
    {
        targetGroup.alpha = 0;
    }
    public void showTargetSlider()
    {
        targetGroup.alpha = 1;
    }
    public void showPlayerSliders()
    {
        playerGroup.alpha = 1;
    }
}
