using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public enum ClickType
{
    Down,
    Up,
    normal,
}

public class InputController : MonoBehaviour
{
    public ToningUI toningCtr;
    public Printer printer;

    public Camera mainCamera;

    public float magnifiedSpeed;

    public enum InputState
    {
        selectColor,
        paint,
        none,
    }

    public InputState curState;

    void Start()
    {
        curState = InputState.none;
        mainCamera = Camera.main;
    }
    //499 1302   574 28
    void Update()
    {
        if (!UIMgr.GetInstance().IsMainUI())
        {
            return;
        }
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        if (results.Count > 0)
        {
            if (results[0].gameObject.name == "Palette")
            {
                if (Input.GetMouseButtonDown(0))
                {
                    curState = InputState.selectColor;
                }
                if (Input.GetMouseButton(0) && curState == InputState.selectColor)
                {
                    toningCtr.ClickPaltte(results);
                }
            }
        }


        if (Input.GetMouseButtonUp(0) && (curState != InputState.none))
        {
            if (curState == InputState.paint)
            {
                printer.GetMouseInput(ClickType.Up);
                printer.ClearSegment();
            }
            curState = InputState.none;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Input.mousePosition;

            if (pos.x > 499 && pos.x < 1302 && pos.y > 28 && pos.y < 574)
            {
                curState = InputState.paint;
                printer.GetMouseInput(ClickType.Down);
            }
        }
        if (curState == InputState.paint)
        {
            if (Input.GetMouseButton(0))
            {
                printer.GetMouseInput(ClickType.normal);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            printer.ClearPaint();
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            mainCamera.fieldOfView -= magnifiedSpeed;
            if (mainCamera.fieldOfView < 1)
            {
                mainCamera.fieldOfView = 1;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            mainCamera.fieldOfView += magnifiedSpeed;
            if (mainCamera.fieldOfView > 90)
            {
                mainCamera.fieldOfView = 90;
            }
        }

        //if (Input.GetKey(KeyCode.W))
        //{
        //    mainCamera.fieldOfView -= magnifiedSpeed;
        //    if (mainCamera.fieldOfView < 1)
        //    {
        //        mainCamera.fieldOfView = 1;
        //    }
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    mainCamera.fieldOfView += magnifiedSpeed;
        //    if (mainCamera.fieldOfView > 90)
        //    {
        //        mainCamera.fieldOfView = 90;
        //    }
        //}

        //if (Input.GetAxis("Mouse ScrollWheel") > 0)
        //{
        //    mainCamera.orthographicSize-=magnifiedSpeed;
        //    if (mainCamera.orthographicSize < 0.02f)
        //    {
        //        mainCamera.orthographicSize = 0.02f;
        //    }
        //}
        //else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        //{

        //    mainCamera.orthographicSize += magnifiedSpeed;
        //    if (mainCamera.orthographicSize > 6)
        //    {
        //        mainCamera.orthographicSize = 6;
        //    }
        //}
    }
}
