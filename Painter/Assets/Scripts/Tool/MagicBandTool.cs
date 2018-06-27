using UnityEngine;
using System.Collections;

public class MagicBandTool : BaseTool
{

    public override void ReciveInput(int x, int y, ClickType type)
    {
        if (type == ClickType.Down)
        {
            printer.SetMaskArea();
            printer.FloodFillMaskThread(x, y, Color.blue, printer.GetColor(x, y));
            EventSys.GetInstance().MagicBandEnd();
        }
    }

    public override void SetToolVal(ToolData val)
    {
        throw new System.NotImplementedException();
    }
}
