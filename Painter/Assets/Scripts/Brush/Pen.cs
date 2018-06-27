using UnityEngine;
using System.Collections;

public class Pen : BaseBrush
{
    public override void ChangePointColor(int x, int y, Color color, int _size)
    {
        //int r2 = _size * _size;
        //int area = r2 << 2;
        //int rr = _size << 1;

        //for (int i = 0; i < area; i++)
        //{
        //    int tx = (i % rr) - _size;
        //    int ty = (i / rr) - _size;

        //    if (tx * tx + ty * ty <= r2)
        //    {
        //        int X = x + tx;
        //        int Y = y + ty;
        //        if (X < 0 || Y < 0 || X >= mXSize || Y >= mYSize) continue;
        //        int arrayPos = (X + Y * mYSize);
        //        mPrinter.SetByteColor(arrayPos, color);
        //    }
        //}
        //  _size = _size / 2;
        for (int _x = x - _size; _x < x + _size; _x++)
        {
            for (int _y = y - _size; _y < y + _size; _y++)
            {
                if (_x < 0 || _y < 0 || _x >= mXSize || _y >= mYSize) continue;
                if (((_x - x) * (_x - x) + (_y - y) * (_y - y)) < _size * _size)
                {
                    int arrayPos = (_x + _y * mYSize);

                    int dis = ((_x - x) * (_x - x) + (_y - y) * (_y - y));
                    if (dis >= (_size - 1) * (_size - 1))
                    {
                        //  int arrayPos = (_x + _y * mYSize);
                        Color val = mPrinter.GetColor(arrayPos);
                        Color result = AddColor(val, color, 0.5f);
                        mPrinter.SetByteColor(arrayPos, result);
                    }
                    else if (dis >= (_size - 2) * (_size - 2))
                    {
                        Color val = mPrinter.GetColor(arrayPos);
                        Color result = AddColor(val, color, 0.8f);
                        mPrinter.SetByteColor(arrayPos, result);
                    }
                    else
                    {
                        Color val = mPrinter.GetColor(arrayPos);
                        Color result = AddColor(val, color, 1);
                        mPrinter.SetByteColor(arrayPos, result);
                    }
                }

            }
        }
     //   Debug.Log(mConcentration);
    }


    Color AddColor(Color c1, Color c2, float Alpha)
    {
        float R; float G; float B;

        float R1 = c1.r;
        float G1 = c1.g;
        float B1 = c1.b;

        float R2 = c2.r;
        float G2 = c2.g;
        float B2 = c2.b;

        // Alpha = 0.5f;

        R = R1 + (R2 - R1) * Alpha * mConcentration;
        G = G1 + (G2 - G1) * Alpha * mConcentration;
        B = B1 + (B2 - B1) * Alpha * mConcentration;

        //Debug.Log((G2 - G1) * Alpha);
        // Debug.Log((100 - Alpha) * R1 + Alpha * R2);
        return new Color(R, G, B);
    }
}
