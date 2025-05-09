using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteEditor : MonoBehaviour
{
    Action _keypadKeyAction;

    LineRender lineRender;
    RectTransform viewportRect;
    RectTransform contentRect;

    void Start()
    {
        Init(); 
    }

    public void Init()
    {
        Managers.Input.KeyAction -= AddNormalNote;
        Managers.Input.KeyAction -= AddDragNote;
        Managers.Input.KeyAction -= AddSlideNote;
        Managers.Input.KeyAction -= AddFlickNote;
        Managers.Input.KeyAction -= _keypadKeyAction;

        _keypadKeyAction = () =>
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                AddNormalNote();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                AddDragNote();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                AddSlideNote();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                AddFlickNote();
            }
        };

        Managers.Input.KeyAction += _keypadKeyAction;

        lineRender = GameObject.Find("LineRender").GetComponent<LineRender>();
        viewportRect = GameObject.Find("Viewport").GetComponent<RectTransform>();
        contentRect = GameObject.Find("Content").GetComponent<RectTransform>();

    }


    void Update()
    {

    }

    public void AddNormalNote()
    {
        Vector2 localMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(viewportRect, Input.mousePosition, null, out localMousePos);
        Vector2 anchoredMousePos = localMousePos + lineRender.contentRect.anchoredPosition;
        float x = anchoredMousePos.x;

        Vector2 localContent;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            contentRect,
            Input.mousePosition,
            null,
            out localContent);
        float y = localContent.y + contentRect.rect.height;

        int vIndex = -1;
        int hIndex = Mathf.FloorToInt(y / 25);

        for (int i = 0; i < lineRender.verticalXs.Count - 1; i++)
        {
            if (x >= lineRender.verticalXs[i] && x < lineRender.verticalXs[i + 1])
            {
                vIndex = i;
                break;
            }
        }
        Debug.Log($"수직선 : {vIndex}, 수평선 : {hIndex}");


    }

    public void AddDragNote()
    {
        

    }

    public void AddSlideNote()
    {
        Vector2 localContent;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            contentRect,
            Input.mousePosition,
            null,
            out localContent);

        // 2) "아래 기준" Y좌표 계산: 
        //    localContent.y (위가 0, 아래가 –contentHeight) 에 contentHeight를 더하면
        //    아래(0)에서부터 위(contentHeight)로 증가하는 Y가 됨
        float pointerYFromBottom = localContent.y + contentRect.rect.height;

        // 확인 로그
        Debug.Log($"[Pointer Y from bottom] {pointerYFromBottom}");
    }

    public void AddFlickNote()
    {

    }
    public void OnDestroy()
    {
        // 객체가 파괴될 때도 구독 해제
        Managers.Input.KeyAction -= AddNormalNote;
        Managers.Input.KeyAction -= AddDragNote;
        Managers.Input.KeyAction -= AddSlideNote;
        Managers.Input.KeyAction -= AddFlickNote;
        Managers.Input.KeyAction -= _keypadKeyAction;
    }
}
