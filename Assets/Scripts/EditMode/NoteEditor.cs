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

    public (int, int, float, float, float) GetMousePointer()
    {
        float centerX = -1f, centerY = -1f;

        // 마우스 좌표 반환
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
        
        // 라인 위치 계산 (25px 간격)
        int lineIndex = Mathf.RoundToInt(y / 25);  // 가장 가까운 라인의 인덱스
        float lineY = lineIndex * 25;  // 실제 라인의 Y 좌표
        
        // 노트의 중심이 라인 위치가 되도록 설정
        centerY = lineY;
        int hIndex = lineIndex;

        // 가장 가까운 수직선 찾기
        float minDistance = float.MaxValue;
        for (int i = 0; i < lineRender.verticalXs.Count; i++)
        {
            float distance = Mathf.Abs(x - lineRender.verticalXs[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                vIndex = i;
                if (i < lineRender.verticalXs.Count - 1)
                {
                    centerX = (lineRender.verticalXs[i] + lineRender.verticalXs[i + 1]) / 2f;
                }
                else
                {
                    centerX = lineRender.verticalXs[i];
                }
            }
        }

        float noteWidth = (lineRender.verticalXs[1] - lineRender.verticalXs[0]);

        if (vIndex == -1f || hIndex == -1f || centerX == -1f || centerY == -1f)
        {
            return (-1, -1, -1f, -1f, -1f);
        }


        return (vIndex, hIndex, centerX, centerY, noteWidth);
    }

    public void AddNormalNote()
    {
        (int vIndex, int hIndex, float centerX, float centerY, float noteWidth) = GetMousePointer();
        if (vIndex == -1) return;

        if (!Managers.Chart.NormalNotes[vIndex].Contains(hIndex))
        {
            GameObject NotePrefab = Resources.Load<GameObject>("Prefabs/Notes/NormalNote");
            GameObject noteInstance = GameObject.Instantiate(NotePrefab, contentRect);

            RectTransform rt = noteInstance.GetComponent<RectTransform>();
            rt.pivot = new Vector2(0.5f, 0.5f); // 중앙 피벗으로 변경
            rt.anchorMin = new Vector2(0.5f, 0f);
            rt.anchorMax = new Vector2(0.5f, 0f);
            rt.anchoredPosition = new Vector2(centerX, centerY);
            rt.sizeDelta = new Vector2(noteWidth + 1, 15f); // 높이 15px (라인 위아래로 7.5px씩)

            Managers.Chart.NormalNotes[vIndex].Add(hIndex);
            Managers.Chart.Notes[vIndex].Add(hIndex, noteInstance);
        }
        else
        {
            if (Managers.Chart.Notes[vIndex].TryGetValue(hIndex, out GameObject go))
            {
                GameObject.Destroy(go);
                Managers.Chart.Notes[vIndex].Remove(hIndex);
            }

            Managers.Chart.NormalNotes[vIndex].Remove(hIndex);
        }

        

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
