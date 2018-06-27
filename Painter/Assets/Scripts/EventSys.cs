using UnityEngine;
using System.Collections;

public delegate void NormalDele();
public delegate void ColorDele(Color32 c);
public class EventSys 
{
    private static EventSys instance;

    public static EventSys GetInstance()
    {
        if(instance==null)
        {
            instance = new EventSys();
        }
        return instance;
    }

    public event NormalDele magicBandEnd;
    public event NormalDele chooseAreaEnd;
    public event ColorDele changeToningColor;

    public void MagicBandEnd()
    {
        if(magicBandEnd!=null)
        {
            magicBandEnd();
        }
    }

    public void ChangeToningColor(Color32 c)
    {
        if(changeToningColor!=null)
        {
            changeToningColor(c);
        }
    }

    public void ChooseAreaEnd()
    {
        if(chooseAreaEnd!=null)
        {
            chooseAreaEnd();
        }
    }

}
