using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PictureHandleUI : UIBase
{
    Toggle mosaicToggle;
    Slider mosaicSlider;

    void Awake()
    {
        mosaicToggle = GetChild<Toggle>("MosaicToggle");
        mosaicSlider = GetChild<Slider>("MosaicSlider");
        Init();
    }

    public override void SetCanOperate()
    {
        base.SetCanOperate();
    }

    public override void SetNotOperate()
    {
        base.SetNotOperate();
    }

    public override void Init()
    {
        base.Init();
    }
}
