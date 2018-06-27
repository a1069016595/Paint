using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LayerUI : UIBase
{
    public Toggle canSeeToggle;
    public Toggle canPrintToggle;
    public RawImage image;

    Texture2D texture;

    bool isSelect = false;

    LayerData data;

    void Awake()
    {
        canSeeToggle = GetChild<Toggle>("canSeeToggle");
        canPrintToggle = GetChild<Toggle>("canPrintToggle");
        image = GetChild<RawImage>("Texture");
        canPrintToggle.onValueChanged.AddListener(CanPrintValueChnage);
        canSeeToggle.onValueChanged.AddListener(CanSeeValueChange);
        this.GetComponent<Button>().onClick.AddListener(OnClick);

        InvokeRepeating("ApplyTexture",1,0.2f);
    }

    void ApplyTexture()
    {
        if (canPrintToggle.isOn&&data!=null)
        {
            texture.LoadRawTextureData(data.GetColorArray());
            texture.Apply();
        }
    }

    private void CanSeeValueChange(bool val)
    {
        transform.parent.GetComponent<LayerMenuUI>().CanSeeChange(transform, val);
    }

    private void CanPrintValueChnage(bool val)
    {
        transform.parent.GetComponent<LayerMenuUI>().CanPrintChange(transform, val);
    }

    public void SetLayer(LayerData _data)
    {
        data = _data;
        texture = new Texture2D(data.xSize, data.ySize, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear;
        texture.LoadRawTextureData(data.GetColorArray());
        texture.Apply();
        image.texture = texture;
        canPrintToggle.onValueChanged.RemoveAllListeners();
        canSeeToggle.onValueChanged.RemoveAllListeners();
        canSeeToggle.isOn = data.canSee;
        canPrintToggle.isOn = data.canPrint;
        canPrintToggle.onValueChanged.AddListener(CanPrintValueChnage);
        canSeeToggle.onValueChanged.AddListener(CanSeeValueChange);
    }

    private void OnClick()
    {
        if(isSelect)
        {
            transform.parent.GetComponent<LayerMenuUI>().DisSelectLayer(this.transform);
        }
        else
        {
            transform.parent.GetComponent<LayerMenuUI>().SeleteLayer(this.transform);
        }
    }

    public void SetSelect()
    {
        isSelect = true;
        this.GetComponent<RawImage>().color = new Color(0.7f, 0.7f, 1, 0.5f);
    }

    public void SetNotSelect()
    {
        isSelect = false;
        this.GetComponent<RawImage>().color = new Color(1, 1, 1, 0.5f);
    }
}
