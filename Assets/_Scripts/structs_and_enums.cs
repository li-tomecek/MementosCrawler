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


public struct Coord
{
    public Coord(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int X { get; set; }
    public int Y { get; set; }
    public override string ToString() => $"({X}, {Y})";
    public static bool operator ==(Coord c1, Coord c2)
    {
        return ((c1.X == c2.X) && (c1.Y == c2.Y));
    }
    public static bool operator !=(Coord c1, Coord c2)
    {
        return ((c1.X != c2.X) || (c1.Y != c2.Y));
    }
}
public struct Stats
{
    public int strength, defense, maxHP, maxSP, agility;
    //later if you want: luck, magic, intelligence
    //also later: strengths and weaknesses
}
