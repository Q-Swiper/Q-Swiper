using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction
{
    public int X { get; set; }
    public int Y { get; set; }
    public Direction(int x, int y)
    {
        X = x;
        Y = y;
    }
    public Direction(DirectionLetter directionLetter)
    {
        switch (directionLetter)
        {
            case DirectionLetter.Right:
                X = 1;
                Y = 0;
                break;
            case DirectionLetter.Left:
                X = -1;
                Y = 0;
                break;
            case DirectionLetter.Up:
                X = 0;
                Y = -1;
                break;
            case DirectionLetter.Down:
                X = 0;
                Y = 1;
                break;
        }
    }
    public Direction GetOppositeDirection()
    {
        return new Direction(X * -1, Y * -1);
    }
    public DirectionLetter GetDirectionLetter()
    {
        if (X == 1 && Y == 0)
        {
            return DirectionLetter.Right;
        }
        else if (X == -1 && Y == 0)
        {
            return DirectionLetter.Left;
        }
        else if (X == 0 && Y == -1)
        {
            return DirectionLetter.Up;
        }
        else if (X == 0 && Y == 1)
        {
            return DirectionLetter.Down;
        }
        return DirectionLetter.InvalidDirection;
    }
    public enum DirectionLetter{
        Right,
        Left,
        Up,
        Down,
        InvalidDirection
    }
}
