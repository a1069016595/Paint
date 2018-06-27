using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewFilePopUI : UIBase
{
    InputField xInputField;
    InputField yInputField;

    Button confirmButton;
    Button closeButton;

    void Awake()
    {
        Init();

        xInputField = GetChild<InputField>("XInputField");
        yInputField = GetChild<InputField>("YInputField");
        confirmButton = GetChild<Button>("ConfirmButton");
        closeButton = GetChild<Button>("CloseButton");

        confirmButton.onClick.AddListener(OnClick_confirmButton);
        closeButton.onClick.AddListener(OnClick_closeButton);
    }

    private void OnClick_closeButton()
    {
        uiMgr.HidePopUI();
    }

    private void OnClick_confirmButton()
    {
        uiMgr.ShowTipUI("新建成功");
    }

    public override void Init()
    {
        m_uiName = UIName.FoundedFile;
        m_uiType = UIType.Pop;
        base.Init();
    }

    public override void SetCanOperate()
    {
        base.SetCanOperate();

        xInputField.enabled = true;
        yInputField.enabled = true;
        confirmButton.enabled = true;
        closeButton.enabled = true;
    }

    public override void SetNotOperate()
    {
        base.SetNotOperate();
        xInputField.enabled = false;
        yInputField.enabled = false;
        confirmButton.enabled = false;
        closeButton.enabled = false;
    }
}
