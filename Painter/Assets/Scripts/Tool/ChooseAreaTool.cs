using UnityEngine;
using System.Collections;

public class ChooseAreaTool : BaseTool
{

    DrawRectangle drawRectangle;

    bool isDraw = false;

    bool isDrag = false;

    public ChooseAreaTool()
    {
        drawRectangle = DrawRectangle.instance;
        drawRectangle.SetPrint(true);
        isDraw = true;
    }

    public override void ReciveInput(int x, int y, ClickType type)
    {
        if (isDraw)
        {
            if (type == ClickType.Down)
            {
                drawRectangle.Draw();
            }
        }
        // if(ty)
        if (type == ClickType.Up)
        {
            if(isDrag)
            {
                isDrag = false;
                drawRectangle.SetNotDrag();
            }
            if(isDraw)
            {
                EventSys.GetInstance().ChooseAreaEnd();
                
                drawRectangle.LockArea();
                isDraw = false;
                drawRectangle.GetArea();
            }
        }
        if (type == ClickType.Down)
        {
            if (!isDraw)
            {
                if (drawRectangle.IsInArea())
                {
                    isDrag = true;
                    drawRectangle.SetDrag();
                }
            }
        }
        if(isDrag)
        {
            drawRectangle.Drag();
        }
    }

    public override void SetToolVal(ToolData val)
    {
       //Vector2 pos1=new Vector2(sta)
    }

    public override void CloseTool()
    {
        base.CloseTool();
        drawRectangle.Clear();
    }
}
