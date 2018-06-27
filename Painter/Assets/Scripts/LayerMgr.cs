using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LayerMgr
{
    List<LayerData> layerDataList;

    LayerData curLayerData;

    int curPage;
    int maxPage;
    int pageVal;
    int xSize;
    int ySize;

    public static LayerMgr instance;

    byte[] color;

    LayerData curPrintLayer;

    public static LayerMgr GetInstance()
    {
        if (instance == null)
        {
            instance = new LayerMgr();
        }
        return instance;
    }

    public LayerMgr()
    {
        layerDataList = new List<LayerData>();
    }

    public void Init(int _xSize, int _ySize, byte[] colors)
    {
        xSize = _xSize;
        ySize = _ySize;
     
        curPage = 1;
        maxPage = 1;
        color=new byte[colors.Length];
        Array.Copy(colors,color,colors.Length);
       // color = colors;
        //Debug.Log(color[0] + "  " + color[1] + "  " + color[2] + "  ");
        curPrintLayer= AddLayer(true);
        Printer.instance.CaculateLayer();
    }

    public LayerData AddLayer(bool canPrint=false)
    {
        LayerData data = new LayerData(xSize, ySize);
        data.ChangeColorData(color);
        if(canPrint)
        {
            data.SetCanPrint(true);
        }
        else
        {
            data.SetCanPrint(false);
        }
        data.SetCanSee(true);
        layerDataList.Add(data);
        pageVal++;
        
        maxPage = pageVal / 3 + 1;
        if (pageVal > 0 && pageVal % 3 == 0)
        {
            maxPage = pageVal / 3;
        }
        return data;
    }

    public void DeleteLayer(int val)
    {
        int removeVal = (curPage - 1) * 3 + val;
        layerDataList.RemoveAt(val);
        // Debug.Log(val);
        pageVal--;

        maxPage = pageVal / 3 + 1;
        if (pageVal > 0 && pageVal % 3 == 0)
        {
            if(curPage>1)
            {
                curPage--;
            }
            maxPage = pageVal / 3 ;
        }
        Printer.instance.CaculateLayer();
    }

    public bool NextPage()
    {
        if (curPage < maxPage)
        {
            curPage++;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool PersiousPage()
    {
        if (curPage == 1)
        {
            return false;
        }
        else
        {
            curPage--;
            return true;
        }
    }

    public List<LayerData> GetCurPageData()
    {
        List<LayerData> result = new List<LayerData>();
        int start = (curPage - 1) * 3;
        for (int i = start; i < start + 3; i++)
        {
            if (i >= layerDataList.Count)
            {
                result.Add(null);
            }
            else
            {
                result.Add(layerDataList[i]);
            }
        }
        return result;
    }

    public void SetCanSee(int val, bool canSee)
    {
        int num = (curPage - 1) * 3 + val;
        layerDataList[num].SetCanSee(canSee);
        Printer.instance.CaculateLayer();
    }

    public void SetCanPrint(int val, bool canPrint)
    {
        ClearCanPrint();
        int num = (curPage - 1) * 3 + val;
        layerDataList[num].SetCanPrint(canPrint);
        if (canPrint)
            curPrintLayer = layerDataList[num];
        else
            curPrintLayer = null;
    }

    private void ClearCanPrint()
    {
        foreach (LayerData item in layerDataList)
        {
            item.SetCanPrint(false);
        }
    }

    public LayerData GetCurPrintLayer()
    {
        return curPrintLayer;
    }

    public List<LayerData> GetCanSeeLayer()
    {
        List<LayerData> list = new List<LayerData>();
        foreach (LayerData item in layerDataList)
        {
            if (item.canSee)
            {
                list.Add(item);
            }
        }
        return list;
    }

    public string GetPageText()
    {
        return curPage.ToString() + "/" + maxPage.ToString();
    }
}
