using UnityEngine;
using System.Collections;

public class GetColorTool : BaseTool
{

    public override void ReciveInput(int x, int y, ClickType type)
    {
       Color32 c= printer.GetColor(x, y);
       EventSys.GetInstance().ChangeToningColor(c);
    }

    public override void SetToolVal(ToolData val)
    {
        throw new System.NotImplementedException();
    }
}
