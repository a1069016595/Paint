using UnityEngine;
using System.Collections;

public class LayerData
{
    public bool canSee;
    public bool canPrint;
    public int xSize;
    public int ySize;
    byte[] colorArray;

    public LayerData(int _xSize, int _ySize)
    {
        colorArray = new byte[_xSize * _ySize * 4];
        xSize = _xSize;
        ySize = _ySize;
    }

    public void ChangeColorData(byte[] val)
    {
        System.Array.Copy(val, colorArray, val.Length);
    }

    public void SetCanSee(bool val)
    {
        canSee = val;
    }

    public void SetCanPrint(bool val)
    {
        canPrint = val;
    }

    public byte[] GetColorArray()
    {
        return colorArray;
    }

    public void SetPixel(int pixel,Color32 c)
    {
        
        colorArray[pixel] = c.r;
        colorArray[pixel + 1] = c.g;
        colorArray[pixel + 2] = c.b;
        colorArray[pixel + 3] = 0;
    }

    public Color32 GetPixel(int pixel)
    {
        pixel = pixel * 4;
        
        if(pixel >=colorArray.Length)
        {
            Debug.Log("error");
            return Color.white;
        }
        byte r = colorArray[pixel];
        byte g = colorArray[pixel + 1];
        byte b = colorArray[pixel + 2];
        byte a = colorArray[pixel + 3];


        return new Color32(r, g, b, 0);
    }
}
