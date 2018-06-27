using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public  class UIBase : MonoBehaviour
{

    protected UIType m_uiType;
    protected UIName m_uiName;

    protected UIMgr uiMgr;


    public UIType UIType
    {
        get { return m_uiType; }
    }

    public UIName UIName
    {
        get { return m_uiName; }
    }

    public virtual void ShowUI()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
    }

    public virtual void HideUI()
    {
        gameObject.SetActive(false);
    }

    public virtual void SetCanOperate()
    {

    }

    public virtual void SetNotOperate()
    {

    }

    public virtual void Init()
    {
        uiMgr = UIMgr.GetInstance();
        uiMgr.ResignUi(this);
    }

    protected T GetChild<T>(string name)
    {
        return transform.FindChild(name).GetComponent<T>();
    }
}