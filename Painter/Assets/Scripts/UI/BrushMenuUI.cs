using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public enum BrushType
{
    pencil,
    pen,
    eraser,
}

public enum PrintTool
{
    magicBand,
    getColor,
    paintPail,
    mosaic,
    chooseArea,
    none,
}

public class BrushMenuUI : UIBase
{

    Dropdown brushTypeDropDown;
    Slider brushSizeSlider;
    Slider concentrationSlider;
    BrushType curType;

    Toggle magicBandToggle;
    Toggle getColorToggle;
    Toggle paintPailToggle;
    Toggle chooseAreaToggle;
    Toggle bezierToggle;

    PrintTool curTool;

    Button cancelMagicBandButton;
    Button cancelChooseAreaButton;
    Toggle mosaicToggle;
    Slider mosaicAreaSlider;
    Slider mosaicValSlider;

    void Awake()
    {
        Init();
        brushTypeDropDown = GetChild<Dropdown>("BrushTypeDropdown");
        brushSizeSlider = GetChild<Slider>("BrushSizeSlider");
        concentrationSlider = GetChild<Slider>("BrushConcentrationSlider");
        magicBandToggle = GetChild<Toggle>("magicBandToggle");
        getColorToggle = GetChild<Toggle>("getColorToggle");
        paintPailToggle = GetChild<Toggle>("paintPailToggle");
        bezierToggle = GetChild<Toggle>("bezierToggle");
        cancelMagicBandButton = GetChild<Button>("CancelMagicBandButton");
        mosaicToggle = GetChild<Toggle>("MosaicToggle");
        mosaicAreaSlider = GetChild<Slider>("MosaicAreaSlider");
        mosaicValSlider = GetChild<Slider>("MosaicValSlider");
        chooseAreaToggle = GetChild<Toggle>("ChooseAreaToggle");
        cancelChooseAreaButton = GetChild<Button>("CancelChooseAreaButton");

        List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
        list.Add(new Dropdown.OptionData("铅笔"));
        list.Add(new Dropdown.OptionData("笔"));
        list.Add(new Dropdown.OptionData("橡皮擦"));
        brushTypeDropDown.options = list;

        magicBandToggle.onValueChanged.AddListener(MagicBandToggleChange);
        getColorToggle.onValueChanged.AddListener(GetColorToggleChange);
        paintPailToggle.onValueChanged.AddListener(PaintPailToggleChange);
        bezierToggle.onValueChanged.AddListener(BezierToggleChange);
        cancelMagicBandButton.onClick.AddListener(CancelMagicBandButtonClick);
        mosaicToggle.onValueChanged.AddListener(MosaicToggleChange);
        mosaicAreaSlider.onValueChanged.AddListener(MosaicSliderValValueChange);
        mosaicValSlider.onValueChanged.AddListener(MosaicSliderValValueChange);
        chooseAreaToggle.onValueChanged.AddListener(ChooseToggleValueChange);
        cancelChooseAreaButton.onClick.AddListener(ChooseAreaButtonClick);
        EventSys.GetInstance().magicBandEnd += BrushMenuUI_magicBandEnd;
        EventSys.GetInstance().chooseAreaEnd += BrushMenuUI_chooseAreaEnd;
    }

    void BrushMenuUI_chooseAreaEnd()
    {
        chooseAreaToggle.isOn = false;
    }

    void BrushMenuUI_magicBandEnd()
    {
        magicBandToggle.isOn = false;
    }

    private void CancelMagicBandButtonClick()
    {
        Printer.instance.SetNoMaskArea();
    }

    private void MagicBandToggleChange(bool val)
    {
        if (val)
        {
            curTool = PrintTool.magicBand;
        }
        else if (curTool == PrintTool.magicBand)
        {
            curTool = PrintTool.none;
        }
        SetTool();
    }

    private void GetColorToggleChange(bool val)
    {
        if (val)
        {
            curTool = PrintTool.getColor;
        }
        else if (curTool == PrintTool.getColor)
        {
            curTool = PrintTool.none;
        }
        SetTool();
    }

    private void PaintPailToggleChange(bool val)
    {
        if (val)
        {
            curTool = PrintTool.paintPail;
        }
        else if (curTool == PrintTool.paintPail)
        {
            curTool = PrintTool.none;
        }
        SetTool();
    }

    private void MosaicToggleChange(bool val)
    {
        if (val)
        {
            curTool = PrintTool.mosaic;
        }
        else if (curTool == PrintTool.mosaic)
        {
            curTool = PrintTool.none;
        }
        SetTool();
        MosaicSliderValValueChange(0);
    }

    private void ChooseToggleValueChange(bool val)
    {
        if (val)
        {
            curTool = PrintTool.chooseArea;
            SetTool();
        }
    }

    private void ChooseAreaButtonClick()
    {
        if (curTool == PrintTool.chooseArea)
        {
            curTool = PrintTool.none;
            SetTool();
        }
        Printer.instance.SetDrawRect(false);
    }

    private void MosaicSliderValValueChange(float val)
    {
        MosaicData data = new MosaicData((int)mosaicAreaSlider.value, (int)mosaicValSlider.value);
        Printer.instance.SetToolVal(PrintTool.mosaic, data);
    }

    private void BezierToggleChange(bool val)
    {
        Printer.instance.SetUseBizier(val);
    }

    private void SetTool()
    {
        Printer.instance.SetPrintTool(curTool);
    }

    public override void Init()
    {
        m_uiName = UIName.Menu;
        m_uiType = UIType.Main;
        base.Init();
    }

    void Update()
    {
        curType = (BrushType)brushTypeDropDown.value;
        Printer.instance.SetBrushData(curType, (int)brushSizeSlider.value, concentrationSlider.value);
    }
}
