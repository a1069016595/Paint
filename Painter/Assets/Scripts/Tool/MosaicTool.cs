using UnityEngine;
using System.Collections;

public class MosaicTool : BaseTool
{
    MosaicData data;

    public override void ReciveInput(int x, int y, ClickType type)
    {
        //   int size=(int)toolVal;
        if (type == ClickType.Down)
        {
            if (data != null)
            {
                Loom.RunAsync(() =>
           {
               printer.MosaicTexture(x, y, data.area, data.mosaicval);
               //  printer.MosaicTexture(x, y,data.mosaicval);
           });
            }
        }
    }

    public override void SetToolVal(ToolData val)
    {
        data = (MosaicData)val;
    }
}

public class MosaicData : ToolData
{
    public int area;
    public int mosaicval;

    public MosaicData(int _area, int _mosaicval)
    {
        area = _area;
        mosaicval = _mosaicval;
    }
}