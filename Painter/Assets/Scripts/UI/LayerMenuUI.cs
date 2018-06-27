using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LayerMenuUI : UIBase
{
  

    LayerUI layer1;

    LayerUI layer2;

    LayerUI layer3;

    Text pageMesText;
    Button newLayerButton;
    Button deleteLayerButton;
    Button persiousPageButton;
    Button nextPageButton;

    int curSelect;

    void Awake()
    {
        layer1 = GetChild<LayerUI>("Layer1");
        layer2 = GetChild<LayerUI>("Layer2");
        layer3 = GetChild<LayerUI>("Layer3");

        pageMesText = GetChild<Text>("PageMesText");
        newLayerButton = GetChild<Button>("NewLayer");
        deleteLayerButton = GetChild<Button>("DeleteLayer");
        persiousPageButton = GetChild<Button>("PreviousPage");
        nextPageButton = GetChild<Button>("NextPage");

        newLayerButton.onClick.AddListener(NewLayerButton);
        deleteLayerButton.onClick.AddListener(DeleteLayerButton);
        persiousPageButton.onClick.AddListener(PersiousPageButton);
        nextPageButton.onClick.AddListener(NextPageButton);
        Init();
        Invoke("InitLayer",1);
    }

    public void InitLayer()
    {
        SetLayer();
        SetText();
    }

    private void NewLayerButton()
    {
        LayerMgr.GetInstance().AddLayer();
        SetLayer();
        SetText();
    }

    private void DeleteLayerButton()
    {
        if(curSelect==-1)
        {
            return;
        }
        LayerMgr.GetInstance().DeleteLayer(curSelect);
        SetLayer();
        SetText();
        curSelect = -1;
        SetAllNotSelect();
    }

    private void PersiousPageButton()
    {
        if( LayerMgr.GetInstance().PersiousPage())
        {
            SetLayer();
            SetText();
            curSelect = -1;
        }
    }

    private void NextPageButton()
    {
        if (LayerMgr.GetInstance().NextPage())
        {
            SetLayer();
            SetText();
            curSelect = -1;
        }
    }


    public override void Init()
    {
        m_uiName = UIName.Menu;
        m_uiType = UIType.Main;
        base.Init();
    }

    public override void SetCanOperate()
    {
        base.SetCanOperate();
    }

    public override void SetNotOperate()
    {
        base.SetNotOperate();
    }

    public void CanSeeChange(Transform obj, bool val)
    {
        LayerMgr.GetInstance().SetCanSee(GetObjVal(obj), val);
        SetLayer();
    }

    public void CanPrintChange(Transform obj, bool val)
    {
        LayerMgr.GetInstance().SetCanPrint(GetObjVal(obj), val);
        SetLayer();
    }

    private int GetObjVal(Transform obj)
    {
        if (obj.name == "Layer1")
        {
            return 0;
        }
        else if (obj.name == "Layer2")
        {
            return 1;
        }
        else if (obj.name == "Layer3")
        {
            return 2;
        }
        else
        {
            Debug.Log("error");
            return -1;
        }
    }

    private LayerUI GetLayerUI(int val)
    {
        if (val == 0)
        {
            return layer1;
        }
        else if (val == 1)
        {
            return layer2;
        }
        else if (val == 2)
        {
            return layer3;
        }
        else
        {
            Debug.Log("error");
            return null;
        }
    }

    public void SeleteLayer(Transform obj)
    {
        SetAllNotSelect();
        curSelect = GetObjVal(obj);
        obj.GetComponent<LayerUI>().SetSelect();
    }

    public void DisSelectLayer(Transform obj)
    {
        curSelect = -1;
        obj.GetComponent<LayerUI>().SetNotSelect();
    }

    private void SetAllNotSelect()
    {
        layer1.SetNotSelect();
        layer2.SetNotSelect();
        layer3.SetNotSelect();
    }

    public void SetLayer()
    {
        List<LayerData> list = LayerMgr.GetInstance().GetCurPageData();

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == null)
            {
                GetLayerUI(i).gameObject.SetActive(false);
            }
            else
            {
                GetLayerUI(i).gameObject.SetActive(true);
                GetLayerUI(i).SetLayer(list[i]);
            }

        }
    }

    public void SetText()
    {
        pageMesText.text ="页数："+ LayerMgr.GetInstance().GetPageText();
    }
}
