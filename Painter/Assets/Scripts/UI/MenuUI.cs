using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuUI : UIBase
{

    public Button Btn_CreateFile;
    public Button Btn_LoadFile;
    public Button Btn_SaveFile;

    void Awake()
    {
        Init();
        Btn_CreateFile = GetChild<Button>("NewFile");
        Btn_LoadFile = GetChild<Button>("LoadFile");
        Btn_SaveFile = GetChild<Button>("SaveFile");

        Btn_LoadFile.onClick.AddListener(Btn_LoadFileOnClick);
        Btn_CreateFile.onClick.AddListener(Btn_CreateFileOnClick);
        Btn_SaveFile.onClick.AddListener(Btn_SaveFileOnClick);
    }

    private void Btn_SaveFileOnClick()
    {
        uiMgr.ShowPopUI(UIName.SaveFile);
    }

    private void Btn_CreateFileOnClick()
    {
        uiMgr.ShowPopUI(UIName.FoundedFile);
    }
    private void Btn_LoadFileOnClick()
    {
        uiMgr.ShowPopUI(UIName.LoadFile);
    }

    public override void Init()
    {
        m_uiType = UIType.Main;
        m_uiName = UIName.Menu;
        base.Init();
    }

    public override void SetCanOperate()
    {
        base.SetCanOperate();
        Btn_CreateFile.enabled = true;
        Btn_LoadFile.enabled = true;
        Btn_SaveFile.enabled = true; 
    }

    public override void SetNotOperate()
    {
        base.SetNotOperate();
        Btn_CreateFile.enabled = false;
        Btn_LoadFile.enabled = false;
        Btn_SaveFile.enabled = false; 
    }
}
