using UnityEngine;
using System.Collections;

public abstract class BaseBrush
{
    protected int mXSize;
    protected int mYSize;
    protected AnimationCurve mCurve;
    protected int mSize;
    protected float mConcentration;
    protected Printer mPrinter;

    protected Color32 whiteColor;

    public void Init(Printer printer, int xSize, int ySize,AnimationCurve curve)
    {
        mPrinter = printer;
        mXSize = xSize;
        mYSize = ySize;
        mCurve = curve;
        whiteColor = new Color32(255, 255, 255, 0);
    }

    public abstract void ChangePointColor(int x, int y, Color color, int _size);

    public  void SetBrushData(int size, float Concentration)
    {
        mSize = size;
        mConcentration = Concentration;
    }
}
