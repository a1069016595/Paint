using UnityEngine;
using System.Collections;
using System.IO;

public class FileMgr
{
    public static Texture2D LoadPng(string filePath)
    {
        //  filePath = Application.dataPath + @"/_Image/grid.png";
        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        System.Drawing.Image img = System.Drawing.Image.FromStream(fs);
        //System.Drawing.Image.FromFile(filePath); //方法二加载图片方式。

        MemoryStream ms = new MemoryStream();
        img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

        Texture2D _tex2 = new Texture2D(1024, 1024, TextureFormat.RGB24, false);
        _tex2.LoadImage(ms.ToArray());
        fs.Close();
        ms.Close();
        return _tex2;
    }

    public static bool SaveRenderTextureToPNG(Texture inputTex, Material mat, string contents, string pngName)
    {
        RenderTexture temp = RenderTexture.GetTemporary(inputTex.width, inputTex.height, 0, RenderTextureFormat.ARGB32);
        //  Texture tex = new Texture();
        //  tex = inputTex;
        Graphics.Blit(inputTex, temp, mat);
        bool ret = SaveRenderTextureToPNG(temp, contents, pngName);
        RenderTexture.ReleaseTemporary(temp);
        return ret;

    }


    //将RenderTexture保存成一张png图片  
    private static bool SaveRenderTextureToPNG(RenderTexture rt, string contents, string pngName)
    {
        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D png = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        png.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        byte[] bytes = png.EncodeToPNG();
        if (!Directory.Exists(contents))
            Directory.CreateDirectory(contents);
        FileStream file = File.Open(contents + "/" + pngName + ".png", FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);
        file.Close();
        Texture2D.DestroyImmediate(png);
        png = null;
        RenderTexture.active = prev;
        return true;

    }
}
