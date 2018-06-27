using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadFilePopUI : UIBase
{
    Button confirmButton;
    Button closeButton;
    InputField pathInputField;
    InputField fileNameInputfield;


    void Awake()
    {
        Init();
        closeButton = GetChild<Button>("CloseButton");
        confirmButton = GetChild<Button>("ConfirmeButton");
        pathInputField = GetChild<InputField>("PathInputField");
        fileNameInputfield = GetChild<InputField>("FileNameInputField");

        confirmButton.onClick.AddListener(OnClick_confirmButton);
        closeButton.onClick.AddListener(OnClick_closeButton);
    }

    private void OnClick_closeButton()
    {
        uiMgr.HidePopUI();
    }

    private void OnClick_confirmButton()
    {
        string path = pathInputField.text + fileNameInputfield.text;
        Texture2D texture=null;
        try
        {
           texture= FileMgr.LoadPng(path);
        }
        catch (System.Exception)
        {
            uiMgr.ShowTipUI("文件不存在");
            return;
        }
        Printer.instance.SetTexture(texture);
        uiMgr.ShowTipUI("加载成功");
    }

    public override void SetCanOperate()
    {
        base.SetCanOperate();
        confirmButton.enabled = true;
        pathInputField.enabled = true;
        fileNameInputfield.enabled = true;
    }

    public override void SetNotOperate()
    {
        base.SetNotOperate();
        confirmButton.enabled = false;
        pathInputField.enabled = false;
        fileNameInputfield.enabled = false;
    }

    public override void Init()
    {
        m_uiName = UIName.LoadFile;
        m_uiType = UIType.Pop;
        base.Init();
    }
}
