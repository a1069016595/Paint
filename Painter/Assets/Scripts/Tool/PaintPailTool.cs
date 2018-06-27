using UnityEngine;
using System.Collections;

public class PaintPailTool : BaseTool
{
    public override void ReciveInput(int x, int y, ClickType type)
    {
        if (type==ClickType.Down)
        {
            printer.FloodFill(x, y, printer.theColor, printer.GetColor(x, y));
        }
    }

    public override void SetToolVal(ToolData val)
    {
        throw new System.NotImplementedException();
    }
}
