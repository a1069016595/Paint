using UnityEngine;
using System.Collections;

public class Eraser : BaseBrush
{
    public override void ChangePointColor(int x, int y, Color color, int _size)
    {
        int r2 = _size * _size;
        int area = r2 << 2;
        int rr = _size << 1;

        for (int i = 0; i < area; i++)
        {
            int tx = (i % rr) - _size;
            int ty = (i / rr) - _size;

            if (tx * tx + ty * ty <= r2)
            {
                int X = x + tx;
                int Y = y + ty;
                if (X < 0 || Y < 0 || X >= mXSize || Y >= mYSize) continue;
                int arrayPos = (X + Y * mYSize);
                int dis = (X - x) * (X - x) + (Y - y) * (Y - y);


                mPrinter.SetByteColor(arrayPos, whiteColor);
            }
        }
    }
}
