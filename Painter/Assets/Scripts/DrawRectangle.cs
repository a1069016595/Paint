using UnityEngine;  
using System.Collections.Generic;

public class DrawRectangle : MonoBehaviour
{
    public float val;

    public Vector2 start = Vector2.zero;//记下鼠标按下位置  
    public Vector2 end = Vector2.down;
    public Material rectMat = null;//画线的材质 不设定系统会用当前材质画线 结果不可控  
    private bool drawRectangle = false;//是否开始画线标志  


    public bool startPrint = false;

    public static DrawRectangle instance;

    bool isLock = false;

    bool isDrag = false;

    Vector2 dragPos;

    Printer printer;

    int lengthX;
    int lengthY;

    float minX;
    float maxX;
    float minY;
    float maxY;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        printer = Printer.instance;
    }

    public void Draw()
    {
        drawRectangle = true;//如果鼠标左键按下 设置开始画线标志  
        start = Printer.instance.GetPos();//记录按下位置  
    }

    public void SetPrint(bool val)
    {
        //    Debug.Log("jj");
        startPrint = val;
        if (val)
        {
            start = Vector3.zero;
            end = Vector3.down;
            isLock = false;
        }
    }

    void OnPostRender()
    {
        if (!startPrint)
        {
            return;
        }
        //画线这种操作推荐在OnPostRender()里进行 而不是直接放在Update，所以需要标志来开启  
        if (drawRectangle)
        {
            if (!isLock)
            {
                end = Printer.instance.GetPos();//鼠标当前位置  
            }
        }
        if (end == Vector2.down)
        {
            return;
        }
        GL.PushMatrix();//保存摄像机变换矩阵  
        rectMat.SetPass(0);

        GL.Begin(GL.LINES);
        GL.Color(Color.black);//设置方框的边框颜色 边框不透明  
        GL.Vertex3(start.x, val, start.y);
        LerpVector(new Vector2(start.x, start.y), new Vector2(end.x, start.y));
        GL.Vertex3(end.x, val, start.y);
        GL.Vertex3(end.x, val, start.y);
        LerpVector(new Vector2(end.x, start.y), new Vector2(end.x, end.y));
        GL.Vertex3(end.x, val, end.y);
        GL.Vertex3(end.x, val, end.y);
        LerpVector(new Vector2(end.x, end.y), new Vector2(start.x, end.y));
        GL.Vertex3(start.x, val, end.y);
        GL.Vertex3(start.x, val, end.y);
        LerpVector(new Vector2(start.x, end.y), new Vector2(start.x, start.y));
        GL.Vertex3(start.x, val, start.y);
        GL.End();

        GL.PopMatrix();//恢复摄像机投影矩阵  
    }

    void LerpVector(Vector2 pos, Vector2 pos1)
    {
        float length = Vector2.Distance(pos, pos1);
        int time = (int)(length / 0.03f);
        for (int i = 0; i < time; i++)
        {
            Vector2 result = Vector2.Lerp(pos, pos1, i * 0.03f);
            GL.Vertex3(result.x, val, result.y);
        }
    }

    public void SetStart(Vector3 pos)
    {
        start = pos;
    }

    public void SetEnd(Vector3 pos)
    {
        end = pos;
    }

    public void LockArea()
    {
        CheckXY();

        lengthX = Mathf.Abs(printer.GetPixelX(maxX) - printer.GetPixelX(minX));
        lengthY = Mathf.Abs(printer.GetPixelY(maxY) - printer.GetPixelY(minY));
        isLock = true;
    }

    public bool IsLock()
    {

        return isLock;
    }

    public void SetDrag()
    {
        isDrag = true;
        dragPos = Printer.instance.GetPos();
    }

    public void SetNotDrag()
    {
        isDrag = false;
    }

    public void Clear()
    {
        drawRectangle = false;
        isLock = false;
    }

    public void Drag()
    {
        if (printer.isInCaculate)
        {
            return;
        }
        Vector2 pos = Printer.instance.GetPos();
        float moveX = dragPos.x - pos.x;
        float moveY = dragPos.y - pos.y;
        if (Mathf.Abs( moveX )< 0.005f &&Mathf.Abs( moveY) < 0.005f)
        {
            return;
        }
        dragPos = pos;
        start = new Vector2(start.x - moveX, start.y - moveY);
        end = new Vector2(end.x - moveX, end.y - moveY);

        CheckXY();

        printer.SetArea(maxX, maxY, lengthX, lengthY);
    }

    public bool IsInArea()
    {
        Vector2 pos = Printer.instance.GetPos();

        CheckXY();
        return pos.x > minX && pos.x < maxX && pos.y > minY && pos.y < maxY;
    }

    public void GetArea()
    {
        CheckXY();

        printer.SelectArea(maxX, maxY, lengthX, lengthY);
    }

    private void CheckXY()
    {
        minX = start.x > end.x ? end.x : start.x;
        maxX = start.x > end.x ? start.x : end.x;
        minY = start.y > end.y ? end.y : start.y;
        maxY = start.y > end.y ? start.y : end.y;
    }
}