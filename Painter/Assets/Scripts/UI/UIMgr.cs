using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum UIType
{
    Pop,//弹窗
    Main//主ui
}

public enum UIName
{
    //主ui
    Menu,//菜单
    Toning,//调色栏
    //弹窗
    SaveFile,//保存文件 
    FoundedFile,//新建文件
    LoadFile,//加载文件
    Tip,//提示
}

public class UIMgr
{
    Stack PopUIStack;

    public List<UIBase> uiList;

    private static UIMgr instance;

    public static UIMgr GetInstance()
    {
        if (instance == null)
        {
            instance = new UIMgr();
            instance.Init();
        }

        return instance;
    }

    private void Init()
    {
        PopUIStack = new Stack();
        uiList = new List<UIBase>();
    }

    public void ResignUi(UIBase ui)
    {
        uiList.Add(ui);
        if (ui.UIType == UIType.Pop)
        {
            ui.HideUI();
        }
    }

    public UIBase ShowPopUI(UIName name)
    {
        foreach (UIBase item in uiList)
        {
            if (item.UIName == name)
            {
                SetNotOperate();
                item.ShowUI();
                PopUIStack.Push(item);
                return item;
            }
        }
        return null;
    }

    public void HidePopUI()
    {
        UIBase ui = (UIBase)PopUIStack.Pop();
        ui.HideUI();
        SetCanOperate();
    }

    /// <summary>
    /// 禁止操作ui
    /// </summary>
    private void SetNotOperate()
    {
        if (PopUIStack.Count == 0)
        {
            foreach (UIBase item in uiList)
            {
                if (item.UIType == UIType.Main)
                {
                    item.SetNotOperate();
                }
            }
        }
        else
        {
            UIBase ui = (UIBase)PopUIStack.Peek();
            ui.SetNotOperate();
        }
    }

    /// <summary>
    /// 允许操作ui
    /// </summary>
    private void SetCanOperate()
    {
        if (PopUIStack.Count == 0)
        {
            foreach (UIBase item in uiList)
            {
                if (item.UIType == UIType.Main)
                {
                    item.SetCanOperate();
                }
            }
        }
        else
        {
            UIBase ui = (UIBase)PopUIStack.Peek();
            ui.SetCanOperate();
        }
    }

    public void ShowTipUI(string tipText)
    {
        TipPopUI tip = (TipPopUI)ShowPopUI(UIName.Tip);
        tip.SetTipText(tipText);
    }

    public bool IsMainUI()
    {
        return PopUIStack.Count == 0;
    }
}