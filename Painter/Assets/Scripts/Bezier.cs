using UnityEngine;
using System.Collections;

public class Bezier
{
    private Vector2 first;
    private Vector2 second;
    private Vector2 third;
    private Vector2 end;

    float smooth_value = 1;

    public Bezier()
    {
        Clear();
    }

    public void Add(Vector2 pos, bool isDis = true)
    {
        if (GetLast() != Vector2.down && Vector2.Distance(pos, GetLast()) < 10 && isDis)
        {
            return;
        }

        if (first == Vector2.down)
        {
            first = pos;
            return;
        }
        else if (second == Vector2.down)
        {

            second = pos;
            //Debug.Log("fwafa");
            return;
        }
        else if (third == Vector2.down)
        {
            third = pos;
            return;
        }
        else
        {
            end = pos;
            return;
        }
    }

    Vector2 GetLast()
    {
        if(end!=Vector2.down)
        {
            return end;
        }
        else if(third!=Vector2.down)
        {
            return third;
        }
        else if(second!=Vector2.down)
        {
            return second;
        }
        else if(first!=Vector2.down)
        {
            return first;
        }
        else
        {
            return Vector2.down;
        }
    }

    public Vector2 GetFirst()
    {
        return first;
    }
    public Vector2 GetSecond()
    {
        return second;
    }
    public Vector2 GetThird()
    {
        return third;
    }
    public Vector2 GetEnd()
    {
        return end;
    }

    public bool IsFull()
    {
        return end != Vector2.down;
    }

    public void Init()
    {
        first = second;
        second = third;
        third = end;
        end = Vector2.down;
    }

    public void Clear()
    {
        first = Vector2.down;
        second = Vector2.down;
        third = Vector2.down;
        end = Vector2.down;
    }

    public TwoPoint GetControl()
    {
        int x0 = (int)first.x;
        int x1 = (int)second.x;
        int x2 = (int)third.x;
        int x3 = (int)end.x;

        int y0 = (int)first.y;
        int y1 = (int)second.y;
        int y2 = (int)third.y;
        int y3 = (int)end.y;

        double xc1 = (x0 + x1) / 2.0;
        double yc1 = (y0 + y1) / 2.0;
        double xc2 = (x1 + x2) / 2.0;
        double yc2 = (y1 + y2) / 2.0;
        double xc3 = (x2 + x3) / 2.0;
        double yc3 = (y2 + y3) / 2.0;

        double len1 = Vector2.Distance(first, second);
        double len2 = Vector2.Distance(second, third);
        double len3 = Vector2.Distance(third, end);

        double k1 = len1 / (len1 + len2);
        double k2 = len2 / (len2 + len3);

        double xm1 = xc1 + (xc2 - xc1) * k1;
        double ym1 = yc1 + (yc2 - yc1) * k1;

        double xm2 = xc2 + (xc3 - xc2) * k2;
        double ym2 = yc2 + (yc3 - yc2) * k2;

        // Resulting control points. Here smooth_value is mentioned  
        // above coefficient K whose value should be in range [0...1].  
        int ctrl1_x = (int)(xm1 + (xc2 - xm1) * smooth_value + x1 - xm1);
        int ctrl1_y = (int)(ym1 + (yc2 - ym1) * smooth_value + y1 - ym1);

        int ctrl2_x = (int)(xm2 + (xc2 - xm2) * smooth_value + x2 - xm2);
        int ctrl2_y = (int)(ym2 + (yc2 - ym2) * smooth_value + y2 - ym2);

        return new TwoPoint(new Vector2(ctrl1_x, ctrl1_y), new Vector2(ctrl2_x, ctrl2_y));
    }

    public Vector2 GetSecondControl()
    {
        return GetMiddlePoint(second, end);
    }

    Vector2 GetMiddlePoint(Vector2 first, Vector2 end)
    {
        int x = (int)((end.x + first.x) / 2);
        int y = (int)((end.y + first.y) / 2);
        return new Vector2(x, y);
    }

    public bool IsNull()
    {
        return first == Vector2.down;
    }

    public void SetLine(Vector2 pos)
    {
        first = second;
        second =third;
        third =pos;
        end = Vector2.down;
    }

    public void SetLine1(Vector2 pos)
    {
        third = pos;
        end = pos;
    }
}

public class TwoPoint
{
    private Vector2 first;
    private Vector2 second;

    public TwoPoint(Vector2 _first,Vector2 _second)
    {
        first = _first;
        second = _second;
    }

    public Vector2 GetFirst()
    {
        return first;
    }
    public Vector2 GetSecond()
    {
        return second;
    }

}

