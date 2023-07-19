using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum moveType
{ 
    attack,
    buff, 
    debuff, 
    heal
}
public class Move
{
    private moveType type;
    private int power = 0;
    private int accuracy = 0;
    private int frequency = 1;
    public string name;

    public Move (string name, moveType type, int power, int accuracy, int freq)
    {
        this.name = name;
        this.type = type;
        this.power = power;
        this.accuracy = accuracy;
        this.frequency = freq;
    }


    public int getPower() {return power;}
    public int getAccuracy() {return accuracy;}
    public moveType getType() {return type;}
    public int getFrequency() { return frequency; }

    public void setPower (int pwr) {this.power = pwr;}
    public void setAccuracy(int acc) {this.accuracy = acc;}
    public void setFrequency(int freq) { this.frequency = freq; }
}
