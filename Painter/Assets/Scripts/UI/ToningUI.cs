using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ToningUI : UIBase
{
    public Slider rSlider;
    public Slider gSlider;
    public Slider bSlider;
    public Slider colorSlider;

    public RawImage image;
    public RawImage sliderImage;
    public RawImage palette;

    public InputField rInputField;
    public InputField gInputField;
    public InputField bInputField;

    Texture2D texture;
    Texture2D paletteTexture;


    Color[] colorArray;
    Color[] sliderColorArray;
    Color[] paletteColorArray;

    public RawImage SelectPaletteImage;

    bool isStartSelect = false;

    public Color CurColor
    {
        get { return colorArray[0]; }
    }

    void Awake()
    {
        texture = new Texture2D(1, 1);
        colorArray = new Color[1];
        paletteTexture = new Texture2D(255, 255);
        paletteColorArray = new Color[255 * 255];
        image.texture = texture;
        MakeSliderTexture();
        MakeBackGrounp();
        SliderChange();
        Init();
        EventSys.GetInstance().changeToningColor += ToningUI_changeToningColor;
    }

    void ToningUI_changeToningColor(Color32 c)
    {
     //   colorArray[0] = c;
        rSlider.value = (float)c.r/255;
        gSlider.value = (float)c.g/255;
        bSlider.value = (float)c.b/255;
        ChangeInputField(c);
    }


    void Update()
    {
        SetResultColor();
    }

    void SetResultColor()
    {
        colorArray[0] = new Color(rSlider.value, gSlider.value, bSlider.value);
        //Debug.Log(colorArray[0]);
        texture.SetPixels(colorArray);
        texture.Apply();
    }
    void MakeBackGrounp()
    {
        Texture2D backTexture = new Texture2D(100, 100);

        Color[] backTextureColor = new Color[100 * 100];

        for (int i = 0; i < backTextureColor.Length; i++)
        {
            backTextureColor[i] = Color.gray;
        }

        for (int j = 0; j < 6; j++)
        {
            for (int i = 0; i < 100; i++)
            {
                backTextureColor[j * 100 + i] = Color.yellow;
                backTextureColor[(99 - j) * 100 + i] = Color.yellow;

                backTextureColor[i * 100 + j] = Color.yellow;
                backTextureColor[i * 100 + 99 - j] = Color.yellow;
            }
        }
        backTexture.SetPixels(backTextureColor);
        backTexture.Apply();
        GetComponent<RawImage>().texture = backTexture;
    }

    public void OnSliderChange()
    {
        ChangeInputField(colorArray[0]);
    }

    void MakeSliderTexture()
    {
        Texture2D sliderTexture = new Texture2D(1, 6 * 255);
        sliderColorArray = new Color[6 * 255 * 1];

        for (int i = 0; i < sliderColorArray.Length; i++)
        {
            sliderColorArray[i] = Color.white;
        }

        //for (int i = 0; i < 6; i++)
        //{
        for (int j = 0; j < 255; j++)
        {
            sliderColorArray[j] = new Color(1, (float)j / 255, 0);
        }
        for (int j = 0; j < 255; j++)
        {
            sliderColorArray[j + 255] = new Color(1 - (float)j / 255, 1, 0);
        }
        for (int j = 0; j < 255; j++)
        {
            sliderColorArray[j + 255 * 2] = new Color(0, 1, (float)j / 255);
        }
        for (int j = 0; j < 255; j++)
        {
            sliderColorArray[j + 255 * 3] = new Color(0, 1 - (float)j / 255, 1);
        }
        for (int j = 0; j < 255; j++)
        {
            sliderColorArray[j + 255 * 4] = new Color((float)j / 255, 0, 1);
        }
        for (int j = 0; j < 255; j++)
        {
            sliderColorArray[j + 255 * 5] = new Color(1, 0, 1 - (float)j / 255);
        }
        //}
        sliderTexture.SetPixels(sliderColorArray);
        sliderTexture.Apply();
        sliderImage.texture = sliderTexture;
    }

    void MakePalette()
    {
        Color endColor = GetSliderColor();
        // paletteTexture = new Texture2D(255, 255);
        float rVal = 1 - endColor.r;
        float gVal = 1 - endColor.g;
        float bVal = 1 - endColor.b;
        for (int i = 0; i < 255; i++)
        {
            for (int j = 0; j < 255; j++)
            {
                float value = 1 - (float)j / 254;
                Color val;

                val = new Color(endColor.r + rVal * value, endColor.g + gVal * value, endColor.b + bVal * value);
                val = val * (float)i / 254;
                val.a = 1;
                paletteColorArray[i * 255 + j] = val;
            }
        }
        paletteTexture.SetPixels(paletteColorArray);
        paletteTexture.Apply();
        palette.texture = paletteTexture;
    }

    public void ClickPaltte(List<RaycastResult> results)
    {
        if (results.Count == 0)
        {
            isStartSelect = false;
            return;
        }
        if (results[0].gameObject.name == "Palette")
        {
            if (Input.GetMouseButtonDown(0))
            {
                isStartSelect = true;
            }
            if (Input.GetMouseButton(0) && isStartSelect)
            {
                GetInputPos();
            }
        }
    }

    private void GetInputPos()
    {
        SelectPaletteImage.rectTransform.position = Input.mousePosition;

        SetRGBSlider();
    }

    void SetRGBSlider()
    {
        Color theColor = GetPaletteColor();
        SelectPaletteImage.color = theColor;
        rSlider.value = theColor.r;
        gSlider.value = theColor.g;
        bSlider.value = theColor.b;

        ChangeInputField(theColor);
    }

    private void ChangeInputField(Color theColor)
    {
        rInputField.text = ((int)(theColor.r * 255)).ToString();
        gInputField.text = ((int)(theColor.g * 255)).ToString();
        bInputField.text = ((int)(theColor.b * 255)).ToString();
    }

    Color GetPaletteColor()
    {
        Color val = Color.red;
        float valX = (SelectPaletteImage.rectTransform.localPosition.x + 75) / 150;
        float valY = (SelectPaletteImage.rectTransform.localPosition.y + 75) / 150;
        int x = (int)(valX * 255);
        int y = (int)(valY * 255);
        int pos = x + y * 255;
        if (pos >= paletteColorArray.Length)
        {
            pos = paletteColorArray.Length - 1;
        }
        val = paletteColorArray[pos];

        return val;
    }

    Color GetSliderColor()
    {
        int val = (int)(colorSlider.value * 6 * 255);
        //Debug.Log(val);
        //Debug.Log(sliderColorArray.Length);
        if (val >= sliderColorArray.Length)
        {
            val = sliderColorArray.Length - 1;
        }
        //Debug.Log("fafwa  "+val);
        return sliderColorArray[val];
    }

    private void SliderChange()
    {
        MakePalette();
        SetRGBSlider();
    }

    public override void SetCanOperate()
    {
        rSlider.enabled = true;
        gSlider.enabled = true;
        bSlider.enabled = true;
        colorSlider.enabled = true;

        rInputField.enabled = true;
        gInputField.enabled = true;
        bInputField.enabled = true;
    }

    public override void SetNotOperate()
    {
        rSlider.enabled = false;
        gSlider.enabled = false;
        bSlider.enabled = false;
        colorSlider.enabled = false;

        rInputField.enabled = false;
        gInputField.enabled = false;
        bInputField.enabled = false;
    }

    public override void Init()
    {
        m_uiName = UIName.Toning;
        m_uiType = UIType.Main;
        base.Init();
    }
}