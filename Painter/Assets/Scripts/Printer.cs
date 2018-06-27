using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Printer : MonoBehaviour
{
    public int size;
    public Texture2D texture;
    int max;
    public Color theColor;

    public int brushSize;


    Bezier mBezier;
    Segment mSegment;
    bool UseBezier;

    public ToningUI toningCtr;

    bool isPrintToOutLine = false;


    int length;


    Color[] brushArray;

    //  byte[] colorByteArray;

    public AnimationCurve curve;

    private bool TextureNeedUpdate = false;

    private Material material;

    public static Printer instance;

    Pencil pencil;
    Pen pen;
    Eraser eraser;

    BaseBrush curBrush;

    bool useMask;

    bool[] maskArea;

    LayerMgr layerMgr;

    byte[] whiteColor;

    byte[] showColor;

    PrintTool curTool = PrintTool.none;
    BaseTool paintTool;

    Color32[] selectAreaColor;
    Area selectArea;

    int[] colorPixelCache;
    Color32[] colorCache;

    void Awake()
    {
        instance = this;
        showColor = new byte[size * size * 4];
        material = GetComponent<MeshRenderer>().material;
        pencil = new Pencil();
        pencil.Init(this, size, size, curve);
        pen = new Pen();
        pen.Init(this, size, size, curve);
        eraser = new Eraser();
        eraser.Init(this, size, size, curve);
        curBrush = pen;

        layerMgr = LayerMgr.GetInstance();
    }

    void Start()
    {
        max = size * size;
        texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear;

        InitTexture();

        max = size * size;
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", texture);
        mBezier = new Bezier();
        mSegment = new Segment();
        TextureNeedUpdate = true;
        texture.LoadRawTextureData(whiteColor);
        texture.Apply();
        LayerMgr.GetInstance().Init(size, size, whiteColor);
    }

    public void SetBrushData(BrushType brushType, int _brushSize, float Concentration)
    {
        brushSize = _brushSize;
        switch (brushType)
        {
            case BrushType.pen:
                curBrush = pen;
                break;
            case BrushType.eraser:
                curBrush = eraser;
                break;
            case BrushType.pencil:
                curBrush = pencil;
                break;
            default:
                break;
        }
        curBrush.SetBrushData(brushSize, Concentration);
    }

    public void SetPrintTool(PrintTool tool)
    {
        if (curTool != tool && paintTool != null)
        {
            paintTool.CloseTool();
        }

        curTool = tool;


        switch (curTool)
        {
            case PrintTool.magicBand:
                paintTool = new MagicBandTool();
                break;
            case PrintTool.getColor:
                paintTool = new GetColorTool();
                break;
            case PrintTool.paintPail:
                paintTool = new PaintPailTool();
                break;
            case PrintTool.mosaic:
                paintTool = new MosaicTool();
                break;
            case PrintTool.chooseArea:
                paintTool = new ChooseAreaTool();
                break;
            case PrintTool.none:
                paintTool = null;
                break;
            default:
                break;
        }
    }

    public void SetToolVal(PrintTool tool, ToolData val)
    {
        if (curTool == tool)
        {
            paintTool.SetToolVal(val);
        }
    }

    public Texture2D GetTexture()
    {
        Texture texture = new Texture();

        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Bilinear;
        tex.LoadRawTextureData(layerMgr.GetCurPrintLayer().GetColorArray());
        tex.Apply();
        return tex;
    }

    public void SetTexture(Texture2D tex)
    {
        //  texture = tex;
        Color32[] colors = tex.GetPixels32();
        // Debug.Log(colorByteArray.Length);
        for (int i = 0; i < colors.Length; i++)
        {
            SetByteColor(i, colors[i]);
        }
        texture.LoadRawTextureData(layerMgr.GetCurPrintLayer().GetColorArray());
        texture.Apply();
    }

    void ShowBezierCurve(Vector2 first, Vector2 control, Vector2 control1, Vector2 end)
    {
        Vector2 result = Vector2.down;
        int val = (int)Vector2.Distance(first, control) + (int)Vector2.Distance(control, control1) + (int)Vector2.Distance(control1, end);
        int val3 = val * val * val;
        Vector2 pos;
        for (int i = 0; i < val; i++)
        {
            pos = (val - i) * (val - i) * (val - i) * first / val3 + 3 * i * (val - i) * (val - i) * control / val3
                  + 3 * control1 * i * i * (val - i) / val3 + i * i * i * end / val3;
            int x = (int)pos.x;
            int y = (int)pos.y;
            //  ChangePointColor(x, y, theColor, brushSize);
            curBrush.ChangePointColor(x, y, theColor, brushSize);
        }
    }

    void ShowCurve(Vector2 start, Vector2 end)
    {

        int x0 = (int)start.x;
        int y0 = (int)start.y;
        int x1 = (int)end.x;
        int y1 = (int)end.y;
        int dx = Mathf.Abs(x1 - x0); // TODO: try these? http://stackoverflow.com/questions/6114099/fast-integer-abs-function
        int dy = Mathf.Abs(y1 - y0);
        int sx, sy;
        if (x0 < x1) { sx = 1; } else { sx = -1; }
        if (y0 < y1) { sy = 1; } else { sy = -1; }
        int err = dx - dy;
        bool loop = true;
        //			int minDistance=brushSize-1;
        int minDistance = (int)(brushSize >> 2);
        //int minDistance = 0;
        // int minDistance = 0;

        //Debug.Log(minDistance);
        int pixelCount = 0;
        int a = 0;
        int e2;

        while (loop)
        {
            pixelCount++;

            if (pixelCount > minDistance)
            {
                pixelCount = 0;
                //ChangePointColor(x0, y0, theColor, brushSize);
                curBrush.ChangePointColor(x0, y0, theColor, brushSize);
                a++;
            }
            if ((x0 == x1) && (y0 == y1)) loop = false;
            e2 = 2 * err;
            if (e2 > -dy)
            {
                err = err - dy;
                x0 = x0 + sx;
            }
            if (e2 < dx)
            {
                err = err + dx;
                y0 = y0 + sy;
            }
        }
        //ChangePointColor(x1, y1, theColor, brushSize);
    }


    void Update()
    {
        theColor = toningCtr.CurColor;
        if (showColor != null)
        {
            texture.LoadRawTextureData(showColor);
            texture.Apply();
        }
    }

    public void CaculateLayer()
    {
        Loom.RunAsync(() =>
           {
               //  showColor = new byte[size * size * 4];
               List<LayerData> layerList = layerMgr.GetCanSeeLayer();
               if (layerList.Count == 0)
               {
                   System.Array.Copy(whiteColor, showColor, whiteColor.Length);
                   //     showColor = whiteColor;
               }
               if (layerList.Count == 1)
               {
                   System.Array.Copy(layerList[0].GetColorArray(), showColor, whiteColor.Length);
               }
               else
               {
                   for (int i = 0; i < layerList.Count - 1; i++)
                   {
                       //layerList[i]
                       if (i == 0)
                       {
                           showColor = CacluateColor(layerList[1].GetColorArray(), layerList[0].GetColorArray());
                       }
                       else
                       {
                           showColor = CacluateColor(layerList[i + 1].GetColorArray(), showColor);
                       }
                   }
               }
               if (showColor == null || showColor.Length < size * size * 4)
               {
                   return;
               }
           });

    }

    public byte[] CacluateColor(byte[] colors1, byte[] colors2)
    {
        byte[] result = new byte[colors1.Length];
        int length = size * size;
        for (int i = 0; i < length; i++)
        {
            int val = i << 2;
            if (Color32Equal(GetColor(val, colors2), Color.white))
            {
                result[val] = colors1[val];
                result[val + 1] = colors1[val + 1];
                result[val + 2] = colors1[val + 2];
            }
            else
            {
                result[val] = colors2[val];
                result[val + 1] = colors2[val + 1];
                result[val + 2] = colors2[val + 2];
            }
        }
        return result;
    }

    public void GetMouseInput(ClickType clickType)
    {
        if (layerMgr.GetCurPrintLayer() == null)
        {
            return;
        }
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] array = Physics.RaycastAll(ray);
        if (array.Length != 0)
        {
            isPrintToOutLine = false;
            hit = array[0];
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity);
            int X = (int)(hit.textureCoord.x * size);
            int Y = (int)(hit.textureCoord.y * size);
            if (curTool != PrintTool.none)
            {
                if (paintTool == null)
                {
                    Debug.Log("error");
                }
                paintTool.ReciveInput(X, Y, clickType);
                return;
            }

            if (Vector2.Distance(mSegment.GetFirst(), new Vector2(X, Y)) > (brushSize / 3) || mSegment.GetFirst() == Vector2.down)
            {
                AddPointToLine(X, Y);
            }
            if (clickType == ClickType.Down)
            {
                curBrush.ChangePointColor(X, Y, theColor, brushSize);
            }
        }
        else
        {
            if (curTool != PrintTool.none)
            {
                return;
            }
            Vector2 pos = GetWorldPoint();
            pos = ChangeToPaintPos(pos);
            if (!isPrintToOutLine)
            {
                if (UseBezier)
                {
                    if (!mBezier.IsNull())
                    {
                        mBezier.SetLine1(pos);
                    }
                }
                else
                {
                    mSegment.Add(pos);
                }
                isPrintToOutLine = true;
            }
            else
            {
                SetLine(pos);
            }
        }
        if (UseBezier)
        {
            if (mBezier.IsFull())
            {
                TwoPoint point = mBezier.GetControl();
                ShowBezierCurve(mBezier.GetSecond(), point.GetFirst(), point.GetSecond(), mBezier.GetThird());
                mBezier.Init();
            }
        }
        else
        {
            if (mSegment.IsFull())
            {
                ShowCurve(mSegment.GetFirst(), mSegment.GetSecond());
                mSegment.Init();
            }
        }
        TextureNeedUpdate = true;
    }

    public Vector2 GetPos()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] array = Physics.RaycastAll(ray);
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity);

        if (array.Length != 0)
        {
            hit = array[0];
            float x = 5 - hit.textureCoord.x * 10;
            float y = 5 - hit.textureCoord.y * 10;
            return new Vector3(x, y);
        }
        else
        {
            return Vector3.down;
        }
    }

    public int GetPixelX(float x)
    {
        float num = (5 - x) / 10;

        int pixelX = (int)(num * size);
        return pixelX;
    }

    public int GetPixelY(float y)
    {
        float num = (5 - y) / 10;

        int pixelY = (int)(num * size);
        return pixelY;
    }

    public void SelectArea(float _maxX, float _maxY, int lengthX, int lengthY)
    {
        selectAreaColor = new Color32[size * size];

        int minX = GetPixelX(_maxX);
        int minY = GetPixelY(_maxY);
        // Debug.Log(minX + "  " + maxX + "  " + minY + "  " + maxY);
        int count = 0;

        colorPixelCache = new int[(lengthX + 1) * (lengthY + 1)];
        colorCache = new Color32[(lengthY + 1) * (lengthX + 1)];

        for (int i = minX; i < minX + lengthX; i++)
        {
            for (int j = minY; j < minY + lengthY; j++)
            {
                count++;
                int val = i + j * size;
                colorPixelCache[count] = val;
                colorCache[count] = Color.white;
                selectAreaColor[count] = GetColor(val);
            }
        }
        selectArea = new Area(minX, minY, lengthX, lengthY);
    }

    public bool isInCaculate = false;

    public void SetArea(float _maxX, float _maxY, int lengthX, int lengthY)
    {
        Loom.RunAsync(() =>
          {
              isInCaculate = true;

              if (colorPixelCache != null)
              {
                  for (int i = 0; i < colorPixelCache.Length; i++)
                  {
                      if (colorPixelCache[i] != 0)
                          SetByteColor(colorPixelCache[i], colorCache[i]);
                  }
              }


              Array.Clear(colorPixelCache, 0, colorPixelCache.Length);
              Array.Clear(colorCache, 0, colorCache.Length);

              int minX = GetPixelX(_maxX);
              int minY = GetPixelY(_maxY);

              int count = 0;
              for (int i = minX; i < minX + lengthX; i++)
              {
                  for (int j = minY; j < minY + lengthY; j++)
                  {
                      count++;
                      if (i < 0 || i >= size || j < 0 || j >= size)
                      {
                          colorPixelCache[count] = 0;
                          continue;
                      }
                      int val = i + j * size;
                      colorPixelCache[count] = val;
                      colorCache[count] = GetColor(val);
                      SetByteColor(val, selectAreaColor[count]);
                  }
              }
              isInCaculate = false;
              selectArea = new Area(minX, minY, lengthX, lengthY);
          });
    }

    public void ClearPaint()
    {
        TextureNeedUpdate = true;
        ClearSegment();
        texture.LoadRawTextureData(whiteColor);
        texture.Apply();
    }

    public void ClearSegment()
    {
        if (UseBezier)
            mBezier.Clear();
        else
            mSegment.Clear();
    }

    private Vector2 ChangeToPaintPos(Vector2 pos)
    {
        int x;
        int y;
        if (pos.x < -5 + transform.position.x)
        {
            x = size;
        }
        else if (pos.x > 5 + transform.position.x)
        {
            x = 0;
        }
        else
        {
            float val = 1 - (pos.x + 5 - transform.position.x) / 10;
            x = (int)Mathf.Lerp(0, size, val);
        }
        if (pos.y < -5 + transform.position.y)
        {
            y = size;
        }
        else if (pos.y > 5 + transform.position.y)
        {
            y = 0;
        }
        else
        {
            float val = 1 - (pos.y + 5 - transform.position.z) / 10;
            y = (int)Mathf.Lerp(0, size, val);
        }
        return new Vector2(x, y);
    }

    private void SetLine(Vector2 pos)
    {
        if (UseBezier)
        {
            mBezier.SetLine(pos);
        }
        else
        {
            mSegment.SetLine(pos);
        }
    }

    private void AddPointToLine(int x, int y)
    {
        if (UseBezier)
        {
            mBezier.Add(new Vector2(x, y));
        }
        else
        {
            mSegment.Add(new Vector2(x, y));
        }
    }

    public void InitTexture()
    {
        whiteColor = new byte[size * size * 4];
        for (int j = 0; j < size; j++)
        {
            for (int i = 0; i < size; i++)
            {
                int val = (i + j * size) * 4;
                whiteColor[val] = 255;
                whiteColor[val + 1] = 255;
                whiteColor[val + 2] = 255;
            }
        }
    }

    public void ResetTexture()
    {
        if (TextureNeedUpdate)
        {
            TextureNeedUpdate = false;
            texture.LoadRawTextureData(layerMgr.GetCurPrintLayer().GetColorArray());
            texture.Apply();
        }
    }

    public Vector2 GetWorldPoint()
    {
        Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9.07f);
        Vector3 world1 = Camera.main.ScreenToWorldPoint(pos);
        return new Vector2(world1.x, world1.z);
    }


    public void SetByteColor(int pixel, Color32 c, byte[] colors = null)
    {
        if (colors != null)
        {
            pixel = pixel * 4;
            colors[pixel] = c.r;
            colors[pixel + 1] = c.g;
            colors[pixel + 2] = c.b;
            colors[pixel + 3] = 0;
            return;
        }

        if (useMask)
        {
            if (!maskArea[pixel])
            {
                return;
            }
        }
        pixel = pixel * 4;
        if (layerMgr != null)
        {
            if (layerMgr.GetCurPrintLayer() != null)
            {
                layerMgr.GetCurPrintLayer().SetPixel(pixel, c);
                if (layerMgr.GetCurPrintLayer() == layerMgr.GetCanSeeLayer()[0])
                {
                    showColor[pixel] = c.r;
                    showColor[pixel + 1] = c.g;
                    showColor[pixel + 2] = c.b;
                }
                else
                {
                    if (!Color32Equal(layerMgr.GetCanSeeLayer()[0].GetPixel(pixel >> 2), Color.white))
                    {
                        return;
                    }
                    else
                    {
                        showColor[pixel] = c.r;
                        showColor[pixel + 1] = c.g;
                        showColor[pixel + 2] = c.b;
                    }
                }
            }
        }
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

        R = R1 + (R2 - R1) * Alpha;
        G = G1 + (G2 - G1) * Alpha;
        B = B1 + (B2 - B1) * Alpha;
        return new Color(R, G, B);
    }

    public bool ColorEqual(Color32 color1, Color32 color2)
    {
        return color1.r == color2.r && color1.g == color2.g && color1.b == color2.b;
    }

    public Color32 GetColor(int pixel)
    {
        return layerMgr.GetCurPrintLayer().GetPixel(pixel);
    }

    public Color32 GetColor(int pixel, byte[] val)
    {
        byte r = val[pixel];
        byte g = val[pixel + 1];
        byte b = val[pixel + 2];
        return new Color32(r, g, b, 0);
    }

    public void SetByteColor(int x, int y, Color32 c)
    {
        SetByteColor(x + y * size, c);
    }

    public Color32 GetColor(int x, int y, byte[] val = null)
    {
        return GetColor(x + y * size);
    }

    int stackSize;
    int maxStackSize = 500;
    private int[] xstack = new int[500];
    private int[] ystack = new int[500];


    public void FloodFill(int x, int y, Color32 newColor, Color32 oldColor)
    {
        Loom.RunAsync(() =>
          {
              floodFillScanLineWithStack(x, y, newColor, oldColor);
          });
    }

    public void floodFillScanLineWithStack(int x, int y, Color32 newColor, Color32 oldColor)
    {
        int val = 30;
        if (Color32Equal(oldColor, newColor))
        {
            return;
        }
        Color32[] colors = null;
        if (selectArea != null)
        {
            colors = new Color32[selectArea.lengthX * 2 + selectArea.lengthY * 2];
            int maxX = selectArea.minX + selectArea.lengthX;
            int maxY = selectArea.minY + selectArea.lengthY;

            int lengthX = selectArea.lengthX;
            int lengthY = selectArea.lengthY;

            int minX = selectArea.minX;
            int minY = selectArea.minY;
            int count = 0;
            for (int i = minX; i < maxX; i++)
            {
                colors[count] = GetColor(i, minY);
                count++;
                SetByteColor(i, minY, Color.black);

            }
            for (int i = minX; i < maxX; i++)
            {
                colors[count] = GetColor(i, maxY);
                count++;
                SetByteColor(i, maxY, Color.black);
            }
            for (int i = minY; i < maxY; i++)
            {
                colors[count] = GetColor(minX, i);
                count++;
                SetByteColor(minX, i, Color.black);
            }
            for (int i = minY; i < maxY; i++)
            {
                colors[count] = GetColor(maxX, i);
                count++;
                SetByteColor(maxX, i, Color.black);
            }
        }

        emptyStack();
        int y1;
        bool spanLeft, spanRight;
        push(x, y);

        while (true)
        {
            x = popx();
            if (x == -1)
            {
                if (selectArea != null && colors != null)
                {
                    int maxX = selectArea.minX + selectArea.lengthX;
                    int maxY = selectArea.minY + selectArea.lengthY;

                    int lengthX = selectArea.lengthX;
                    int lengthY = selectArea.lengthY;

                    int minX = selectArea.minX;
                    int minY = selectArea.minY;
                    int count = 0;
                    for (int i = minX; i < maxX; i++)
                    {
                        SetByteColor(i, minY, colors[count]);
                        count++;
                    }
                    for (int i = minX; i < maxX; i++)
                    {
                        SetByteColor(i, maxY, colors[count]);
                        count++;
                    }
                    for (int i = minY; i < maxY; i++)
                    {
                        SetByteColor(minX, i, colors[count]);
                        count++;
                    }
                    for (int i = minY; i < maxY; i++)
                    {
                        SetByteColor(maxX, i, colors[count]);
                        count++;
                    }
                }
                return;
            }
            y = popy();
            y1 = y;
            while (y1 >= 0 && Color32Equal(GetColor(x, y1), oldColor,val))
            {
                y1--; // go to line top/bottom  
            }

            y1++; // start from line starting point pixel  
            spanLeft = spanRight = false;
            while (y1 < size && Color32Equal(GetColor(x, y1), oldColor,val))
            {

                SetByteColor(x, y1, newColor);
                if (!spanLeft && x > 0 && Color32Equal(GetColor(x - 1, y1), oldColor,val))// just keep left line once in the stack  
                {
                    push(x - 1, y1);
                    spanLeft = true;
                }
                else if (spanLeft && x > 0 && !Color32Equal(GetColor(x - 1, y1), oldColor,val))
                {
                    spanLeft = false;
                }
                if (!spanRight && x < size - 1 && Color32Equal(GetColor(x + 1, y1), oldColor,val)) // just keep right line once in the stack  
                {
                    push(x + 1, y1);
                    spanRight = true;
                }
                else if (spanRight && x < size - 1 && !Color32Equal(GetColor(x + 1, y1), oldColor,val))
                {
                    spanRight = false;
                }
                y1++;
            }
        }
    }

    public void FloodFillMaskThread(int x, int y, Color32 newColor, Color32 oldColor)
    {
        Loom.RunAsync(() =>
        {
            FloodFillMask(x, y, newColor, oldColor);
        });
    }

    private void FloodFillMask(int x, int y, Color32 newColor, Color32 oldColor)
    {
        byte[] byte1 = new byte[showColor.Length];
        byte[] byte2 = new byte[showColor.Length];

        maskArea = new bool[size * size];
        byte1 = new byte[layerMgr.GetCurPrintLayer().GetColorArray().Length];
        System.Array.Copy(layerMgr.GetCurPrintLayer().GetColorArray(), byte1, byte1.Length);

        byte2 = new byte[showColor.Length];
        System.Array.Copy(showColor, byte2, byte2.Length);

        emptyStack();
        int y1;
        bool spanLeft, spanRight;
        push(x, y);

        while (true)
        {
            x = popx();
            if (x == -1)
            {
                layerMgr.GetCurPrintLayer().ChangeColorData(byte1);
                showColor = byte2;
                return;
            }
            y = popy();
            y1 = y;
            while (y1 >= 0 && Color32Equal(GetColor(x, y1), oldColor))
            {
                y1--; // go to line top/bottom  
            }

            y1++; // start from line starting point pixel  
            spanLeft = spanRight = false;
            while (y1 < size && Color32Equal(GetColor(x, y1), oldColor))
            {
                SetByteColor(x, y1, newColor);
                maskArea[x + y1 * size] = true;

                if (!spanLeft && x > 0 && Color32Equal(GetColor(x - 1, y1), oldColor))// just keep left line once in the stack  
                {
                    push(x - 1, y1);
                    spanLeft = true;
                }
                else if (spanLeft && x > 0 && !Color32Equal(GetColor(x - 1, y1), oldColor))
                {
                    spanLeft = false;
                }
                if (!spanRight && x < size - 1 && Color32Equal(GetColor(x + 1, y1), oldColor)) // just keep right line once in the stack  
                {
                    push(x + 1, y1);
                    spanRight = true;
                }
                else if (spanRight && x < size - 1 && !Color32Equal(GetColor(x + 1, y1), oldColor))
                {
                    spanRight = false;
                }
                y1++;
            }
        }
    }

    public void MosaicTexture(int x, int y, int area, int val)
    {
        int time = area / val >> 2;
        for (int i = -time; i < time; i++)
        {
            for (int j = -time; j < time; j++)
            {
                int x1 = x + i * val;
                int y1 = y + j * val;
                if (x1 > 0 && x1 <= size && y1 > 0 && y1 <= size)
                {
                    MosaicTexture(x1, y1, val, GetColor(x1 + y1 * size));
                }
            }
        }
    }

    private void MosaicTexture(int x, int y, int val, Color32 c)
    {
        for (int i = x - val; i < x + val; i++)
        {
            for (int j = y - val; j < y + val; j++)
            {
                if (i < 0 || i > size || j < 0 || j > size)
                {

                }
                else
                {
                    SetByteColor(i, j, c);
                }

            }
        }
    }

    public void MovePixel()
    {

    }

    public void SetMaskArea()
    {
        useMask = true;
        maskArea = new bool[size * size];
    }

    public void SetNoMaskArea()
    {
        useMask = false;
    }

    public void SetDrawRect(bool val)
    {
        selectArea = null;
        DrawRectangle.instance.SetPrint(val);
    }

    public void SetUseBizier(bool val)
    {
        UseBezier = val;
    }

    public bool Color32Equal(Color32 a, Color32 b, int val = -1)
    {
        if (val == -1)
        {
            val = 30;
        }
        return Math.Abs(a.r - b.r) < val && Math.Abs(a.b - b.b) < val && Math.Abs(a.g - b.g) < val;
    }

    private void emptyStack()
    {
        while (popx() != -1)
        {
            popy();
        }
        stackSize = 0;
    }

    void push(int x, int y)
    {
        stackSize++;
        if (stackSize == maxStackSize)
        {
            int[] newXStack = new int[maxStackSize * 2];
            int[] newYStack = new int[maxStackSize * 2];
            Array.Copy(xstack, 0, newXStack, 0, maxStackSize);
            Array.Copy(ystack, 0, newYStack, 0, maxStackSize);
            xstack = newXStack;
            ystack = newYStack;
            maxStackSize *= 2;
        }
        xstack[stackSize - 1] = x;
        ystack[stackSize - 1] = y;
    }

    int popx()
    {
        if (stackSize == 0)
            return -1;
        else
            return xstack[stackSize - 1];
    }

    int popy()
    {
        int value = ystack[stackSize - 1];
        stackSize--;
        return value;
    }
}
