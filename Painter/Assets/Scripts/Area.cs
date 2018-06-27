using UnityEngine;
using System.Collections;

public class Area
{
    public int minX;
    public int minY;

    public int lengthX;
    public int lengthY;

    public Area(int _minX,int _minY,int _lengthX,int _lengthY)
    {
        minX = _minX;
        minY = _minY;
        lengthX = _lengthX;
        lengthY = _lengthY;
    }
}