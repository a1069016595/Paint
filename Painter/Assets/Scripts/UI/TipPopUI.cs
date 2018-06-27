using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TipPopUI : UIBase
{
    Button confirmButton;
    Text tipText;

    void Awake()
    {
        Init();
        confirmButton = GetChild<Button>("ConfirmButton");
        tipText = GetChild<Text>("TipText");

        confirmButton.onClick.AddListener(OnClickConfirmButton);
    }

    private void OnClickConfirmButton()
    {
        UIMgr.GetInstance().HidePopUI();
    }

    public override void Init()
    {
        m_uiName = UIName.Tip;
        m_uiType = UIType.Pop;
        base.Init();
    }

    public override void SetCanOperate()
    {
        base.SetCanOperate();
        confirmButton.enabled = true;
    }

    public override void SetNotOperate()
    {
        base.SetNotOperate();
        confirmButton.enabled = false;
    }

    public void SetTipText(string text)
    {
        tipText.text = text;
    }
}
