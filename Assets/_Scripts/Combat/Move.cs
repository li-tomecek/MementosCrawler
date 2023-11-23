using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Move
{
    [SerializeField] private MoveType type;
    [SerializeField] private int power = 0;
    [SerializeField] private int accuracy = 0;
    [SerializeField] private int frequency = 1;      //??
    [SerializeField] private int cost = 1;           //SP cost
    public string name;

    public Move (string name, MoveType type, int power, int accuracy, int freq, int cost)
    {
        this.name = name;
        this.type = type;
        this.power = power;
        this.accuracy = accuracy;
        this.frequency = freq;      //what the fuck is this i don't remember why i put this here maybe it has something to do with range? or crit hits? I THINK ITS LIKE THE SILLY HANDS: DO THEY HIT MULTIPLE TIMES!
        this.cost = cost;
    }


    public int getPower() {return power;}
    public int getAccuracy() {return accuracy;}
    public MoveType getType() {return type;}
    public int getFrequency() { return frequency; }
    public int getSPCost() { return cost; }

    public void setPower (int pwr) {this.power = pwr;}
    public void setAccuracy(int acc) {this.accuracy = acc;}
    public void setFrequency(int freq) { this.frequency = freq; }
    public void setSPCost(int c) { this.cost = c; }

}
