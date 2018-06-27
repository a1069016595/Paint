using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextureMaker : MonoBehaviour
{

    public int num = 100;
    Color[] colorArray;
    Texture2D texture;
    int max;
    public float theLong;
    //public int range;
    public GameObject plane;

    public RawImage image;

    Color theColor;

    public Transform target;
    public float speed;

    public Camera camera;
    public Camera camera1;

    void Start()
    {
        texture = new Texture2D(num, num);
        colorArray = new Color[num * num];

        InitTexture();

        max = num * num;

        //  this.GetComponent<MeshRenderer>().material.SetTexture("_MainTexHeight", texture);
        plane.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", texture);
        // image.texture = texture;
        theColor = new Color(0, 0, 0, 0);
        ChangePos(target.position);
    }

    void Update()
    {
        //if (Input.GetKey(KeyCode.A))
        //{

        //    InitTexture();
        //}
        //if (Input.GetMouseButton(0))
        //{
        //    RaycastHit hit;
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit[] array = Physics.RaycastAll(ray);
        //    if (array.Length != 0)
        //    {
        //        hit = array[0];
        //        //Debug.Log(hit.point);

        //        float valX = 1 - (hit.point.x + 5) / 10;
        //        float valY = 1 - (hit.point.z + 5) / 10;

        //        int X = (int)Mathf.Lerp(0, num, valX);
        //        int Y = (int)Mathf.Lerp(0, num, valY);
        //        //Debug.Log(X + "  " + Y);
        //        Change(X, Y);
        //    }

        //}
        //TerrainData.

        Vector3 pos = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            pos = GetPos(0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            pos = GetPos(1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            pos = GetPos(2);
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos = GetPos(3);
        }
        target.transform.position += pos * Time.deltaTime * speed;
        camera.transform.position += pos * Time.deltaTime * speed;
        camera1.transform.position += pos * Time.deltaTime * speed;

        if (pos != Vector3.zero)
            ChangePos(target.position);
    }

    Vector3 GetPos(int val)
    {
        switch (val)
        {
            case 0:
                return Vector3.forward;
            case 1:
                return Vector3.left;
            case 2:
                return Vector3.back;
            case 3:
                return Vector3.right;
        }
        return Vector3.zero;
    }

    void ChangePos(Vector3 pos)
    {
        float valX = 1 - (pos.x + 5) / 10;
        float valY = 1 - (pos.z + 5) / 10;

        int X = (int)Mathf.Lerp(0, num, valX);
        int Y = (int)Mathf.Lerp(0, num, valY);
        Change(X, Y);
    }

    public void Change(int x, int y)
    {

        int val = x + y * num;
        if (val > max)
        {
            return;
        }

        //if(val>max)
        //{
        //    return;
        //}
        //  colorArray[val] = theColor;

        //if (x > 1 && y > 1 && x < num - 1 && y < num - 1)
        //{
        //    colorArray[x + y * num + 1] =theColor;
        //    colorArray[x + y * num - 1] = theColor;
        //    colorArray[x + (y + 1) * num] = theColor;
        //    colorArray[x + (y - 1) * num] = theColor;

        //    colorArray[x + (y + 1) * num + 1] =theColor;
        //    colorArray[x + (y - 1) * num + 1] =theColor;
        //    colorArray[x + (y + 1) * num - 1] =theColor;
        //    colorArray[x + (y - 1) * num - 1] = theColor;
        //}

        for (int i = x - 10; i < x + 11; i++)
        {
            for (int j = y - 10; j < y + 11; j++)
            {
                if (i >= 0 && i <= num && j >= 0 && j <= num)
                {
                    if (Vector2.Distance(new Vector2(i, j), new Vector2(x, y)) < 10.2f)
                    {
                        int theval = i + j * num;
                        if (theval < max && theval >= 0)
                        {
                            if (colorArray[theval] != theColor)
                            {
                                colorArray[theval] = theColor;
                            }
                        }
                    }
                }
            }
        }

        //for (int i = x - 10; i < x + 11; i++)
        //{

        //    if (i >= 0 && i <= num)
        //    {
        //        if (Vector2.Distance(new Vector2(i, y), new Vector2(x, y)) < 10.2f)
        //        {
        //            int theval = i + y * num;
        //            if (theval < max && theval >= 0)
        //            {

        //                if (colorArray[theval] != theColor)
        //                {
        //                    colorArray[theval] = theColor;
        //                    Debug.Log(theval);
        //                }

        //            }
        //        }
        //    }

        //}
        ResetTexture();
    }

    public void ResetTexture()
    {
        texture.SetPixels(colorArray);
        texture.Apply();


    }

    public void InitTexture()
    {
        for (int j = 0; j < num; j++)
        {
            for (int i = 0; i < num; i++)
            {
                //  
                colorArray[i + j * num] = new Color(1, 1, 1, 1);
            }
        }
        ResetTexture();
    }

    Color GetColor(int val, int val1)
    {
        if (val1 % 2 == 1)
            return val % 2 == 0 ? Color.white : Color.black;
        else
            return val % 2 == 1 ? Color.white : Color.black;
    }
}
