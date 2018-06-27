using UnityEngine;
using System.Collections;

public abstract class BaseTool 
{
    protected Printer printer;
    protected float toolVal;

    public BaseTool()
    {
        printer = Printer.instance;
    }

    public abstract void ReciveInput(int x, int y, ClickType isDown);

    public abstract void SetToolVal(ToolData val);

    public virtual void CloseTool()
    {

    }
}

public abstract class ToolData
{

}