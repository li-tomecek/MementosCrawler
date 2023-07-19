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

    public Move (moveType type, int power, int accuracy)
    {
        this.type = type;
        this.power = power;
        this.accuracy = accuracy;
    }


    public int getPower() {return power;}
    public int getAccuracy() {return accuracy;}
    public moveType getType() {return type;}

    public void setPower (int pwr) {this.power = pwr;}
    public void setAccuracy(int acc) {this.accuracy = acc;}
}
