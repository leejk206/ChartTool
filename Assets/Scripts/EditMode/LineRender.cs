using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineRender : MonoBehaviour
{
    GameObject rootCanvas;
    GameObject content;
    GameObject verticalLinePrefab;
    GameObject horizontalLinePrefab;

    float beatHeight;

    void Start()
    {
        rootCanvas = GameObject.Find("RootCanvas");
        content = GameObject.Find("Content");
        verticalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/VerticalLine");
        horizontalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/HorizontalLine");

        beatHeight = (rootCanvas.GetComponent<RectTransform>().rect.height) / 4;

        int numLines = 8;
        float totalWidth = (rootCanvas.GetComponent<RectTransform>().rect.width) * 0.92f;

        for (int i = 0; i < numLines; i++)
        {
            float x = -(totalWidth / 2f) + (totalWidth / (numLines - 1)) * i;
            GameObject vLine = Instantiate(verticalLinePrefab, content.transform);
            RectTransform vrt = vLine.GetComponent<RectTransform>();
            vrt.anchoredPosition = new Vector2(x, 0);
        }

        // 수평선 배치
        for (int i = 0; i <= 4; i++)
        {
            // 1박 단위 (두껍고 진하게)
            CreateHorizontalLine(-i * beatHeight, thickness: 2f, color: new Color(0.6f, 0.6f, 0.6f, 1f));

            if (i < 4)
            {
                // 1/2박 단위 (중간 두께+반투명)
                CreateHorizontalLine(-(i * beatHeight + beatHeight / 2f),
                                     thickness: 1f,
                                     color: new Color(0.7f, 0.7f, 0.7f, 0.7f));

                // 1/4박 단위 (얇고 연한)
                for (int k = 1; k < 4; k++)
                    CreateHorizontalLine(-(i * beatHeight + k * beatHeight / 4f),
                                         thickness: 0.5f,
                                         color: new Color(0.8f, 0.8f, 0.8f, 0.5f));

                // 1/8박 단위
                for (int k = 1; k < 8; k++)
                    CreateHorizontalLine(-(i * beatHeight + k * beatHeight / 8f),
                                         thickness: 0.5f,
                                         color: new Color(0.85f, 0.85f, 0.85f, 0.4f));

                // 1/16박 단위
                for (int k = 1; k < 16; k++)
                    CreateHorizontalLine(-(i * beatHeight + k * beatHeight / 16f),
                                         thickness: 0.5f,
                                         color: new Color(0.9f, 0.9f, 0.9f, 0.3f));
            }
        }
    }

    void CreateHorizontalLine(float y, float thickness, Color color)
    {
        GameObject hLine = Instantiate(horizontalLinePrefab, content.transform);
        RectTransform hrt = hLine.GetComponent<RectTransform>();

        // 위치 설정
        hrt.anchoredPosition = new Vector2(0, y);
        // 두께 설정
        hrt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, thickness);
        // 색상 설정
        var img = hLine.GetComponent<Image>();
        if (img != null) img.color = color;
    }


    void Update()
    {
        
    }
}
