using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType
{
    ATTACK, BUFF, DEBUFF, HEAL
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

    public List<Coord> findAdjacentCoords()
    {
        List<Coord> adj = new List<Coord>();
        Coord temp = new Coord(X, Y);

        for (int i = X - 1; i <= X + 1; i += 2)
        {
            if (!(i < 0 || i >= GameManager.Instance.columns))
            {
                temp.X = i;
                adj.Add(temp);
            }
        }
        temp.X = X;
        for (int i = Y - 1; i <= Y + 1; i += 2)
        {
            if (!(i < 0 || i >= GameManager.Instance.rows))
            {
                temp.Y = i;
                adj.Add(temp);
            }
        }
        return adj;
    }

    public override string ToString() => $"({X}, {Y})";
    public static bool operator ==(Coord c1, Coord c2)
    {
        return ((c1.X == c2.X) && (c1.Y == c2.Y));
    }
    public static bool operator !=(Coord c1, Coord c2)
    {
        return ((c1.X != c2.X) || (c1.Y != c2.Y));
    }
    public override bool Equals(object obj) //just to get rid fo warnings
    {
        return base.Equals(obj);
    }
    public override int GetHashCode()       //just to get rid of warnings
    {
        return base.GetHashCode();
    }
}
public struct Stats
{
    public int strength, defense, maxHP, maxSP, agility;

    //later if you want: luck, magic, intelligence
    //also later: strengths and weaknesses
}
