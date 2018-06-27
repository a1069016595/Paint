using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SaveFilePopUI : UIBase
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
        Material mat= Resources.Load<Material>("PrintMat");
        FileMgr.SaveRenderTextureToPNG(Printer.instance.GetTexture(), mat, pathInputField.text, fileNameInputfield.text);
   //     FileMgr.DoPicture("C:/Hello.png", "C:/", Printer.instance.GetTexture());
        uiMgr.ShowTipUI("保存成功");
    }

    public override void Init()
    {
        m_uiName = UIName.SaveFile;
        m_uiType = UIType.Pop;
        base.Init();
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
}
