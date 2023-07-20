using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType
{
    ATTACK,
    BUFF,
    DEBUFF,
    HEAL
}
public enum Mode
{
    BATTLE_MOVE, FREE_MOVE, ACTION_SELECT, ENEMY_TURN
}


public struct Coords
{
    public Coords(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int X { get; set; }
    public int Y { get; set; }
    public override string ToString() => $"({X}, {Y})";
}
public struct Stats
{
    public int strength, defense, maxHP, maxSP, agility;
    //later if you want: luck, magic, intelligence
    //also later: strengths and weaknesses
}
