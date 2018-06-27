using UnityEngine;
using System.Collections;

public class Segment : MonoBehaviour
{
    private Vector2 firstPoint;
    private Vector2 secondPoint;

    public Segment()
    {
        Clear();
    }

    public void Add(Vector2 pos)
    {
        if(firstPoint==Vector2.down)
        {
            firstPoint = pos;
        }
        else
        {
            secondPoint = pos;
        }
    }

    public bool IsFull()
    {
        return secondPoint != Vector2.down;
    }

    public Vector2 GetFirst()
    {
        return firstPoint;
    }

    public Vector2 GetSecond()
    {
        return secondPoint;
    }


    public void Init()
    {
        firstPoint = secondPoint;
        secondPoint = Vector2.down;
    }

    public void Clear()
    {
        firstPoint = Vector2.down;
        secondPoint = Vector2.down;
    }

    public void SetLine(Vector2 pos)
    {
        firstPoint = pos;
        secondPoint = Vector2.down;
    }
}
