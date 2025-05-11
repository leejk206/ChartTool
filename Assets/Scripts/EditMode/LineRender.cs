using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LineRender : MonoBehaviour
{
    public GameObject rootCanvas;
    public GameObject content;
    GameObject verticalLinePrefab;
    public GameObject scrollView;
    public RectTransform contentRect;
    RectTransform scrollViewRect;

    GameObject wholeHorizontalLinePrefab;
    GameObject halfHorizontalLinePrefab;
    GameObject quaterHorizontalLinePrefab;
    GameObject eighthHorizontalLinePrefab;
    GameObject sixteenthHorizontalLinePrefab;

    float contentHeight;
    float beatHeight;

    int horizontalLineCount;

    public List<GameObject> verticalLines;
    public List<GameObject> horizontalLines;

    public List<float> verticalXs;
    public List<float> horizontalYs;

    bool isPlayMode = false;

    public void Init()
    {
        rootCanvas = GameObject.Find("RootCanvas"); // 캔버스
        content = GameObject.Find("Content"); // 스크롤 안의 내용물
        verticalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/VerticalLine"); // 수직선
        scrollView = GameObject.Find("ScrollView"); // 스크롤 오브젝트
        contentRect = content.GetComponent<RectTransform>(); // 내용물 transform
        scrollViewRect = scrollView.GetComponent<RectTransform>(); // 스크롤 transform

        wholeHorizontalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/HorizontalLineWhole"); // 1박 수평선
        halfHorizontalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/HorizontalLineHalf"); // 1/2박 수평선
        quaterHorizontalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/HorizontalLineQuater"); // 1/4박 수평선
        eighthHorizontalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/HorizontalLineEighth"); // 1/8박 수평선
        sixteenthHorizontalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/HorizontalLineSixteenth"); // 1/16박 수평선

        verticalLines = new();
        horizontalLines = new();
        verticalXs = new();
        horizontalYs = new();

        // 스크롤 영역 조정
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, 50000);

        contentHeight = contentRect.sizeDelta.y; // 내용물 수직 크기
        beatHeight = (rootCanvas.GetComponent<RectTransform>().rect.height) / 4;


        int numLines = 8;
        float totalWidth = (rootCanvas.GetComponent<RectTransform>().rect.width) * 0.92f;

        TimeController.Timer.ExecuteAfterTime(0.01f, () => scrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0);

        if (SceneManager.GetActiveScene().name == "PlayMode")
            isPlayMode = true;

        // 수직선 제작
        for (int i = 0; i < numLines; i++)
        {
            float x = -(totalWidth / 2f) + (totalWidth / (numLines - 1)) * i;
            GameObject vLine = Instantiate(verticalLinePrefab, content.transform);
            RectTransform vrt = vLine.GetComponent<RectTransform>();

            // 아래에서 시작해서 위로 그리도록 설정
            vrt.pivot = new Vector2(0.5f, 0f);               // 아래 기준
            vrt.anchorMin = new Vector2(0.5f, 0f);           // Content 아래
            vrt.anchorMax = new Vector2(0.5f, 0f);

            vrt.anchoredPosition = new Vector2(x, 0);        // Content의 아래쪽에 위치
            vrt.sizeDelta = new Vector2(vrt.sizeDelta.x, 50000f); // 위로 뻗는 높이

            if (isPlayMode)
            {
                Image img = vLine.GetComponent<Image>();
                if (img != null)
                {
                    Color color = img.color;
                    color.a = 0f; // 완전 투명
                    img.color = color;
                }
            }

            verticalLines.Add(vLine);
        }

        //수평선 제작(1박)
        for (int i = 0; i <= 125; i++)
        {
            float y = i * 400f;

            GameObject hLine = Instantiate(wholeHorizontalLinePrefab, content.transform);
            RectTransform hrt = hLine.GetComponent<RectTransform>();

            // pivot과 anchor를 아래 기준으로 설정
            hrt.pivot = new Vector2(0f, 0f);
            hrt.anchorMin = new Vector2(0f, 0f);
            hrt.anchorMax = new Vector2(1f, 0f); // 가로로 Stretch 되도록

            hrt.anchoredPosition = new Vector2(0f, y); // 아래에서 위로 배치
            hrt.sizeDelta = new Vector2(0f, 12f); // 높이 1px, 너비는 anchor로 결정됨

            if (isPlayMode)
            {
                Image img = hLine.GetComponent<Image>();
                if (img != null)
                {
                    Color color = img.color;
                    color.a = 0f; // 완전 투명
                    img.color = color;
                }
            }

            horizontalLines.Add(hLine);
        }

        //수평선 제작(1/2박)
        for (int i = 0; i <= 250; i++)
        {
            float y = i * 200f;

            if (i % 2 != 0)
            {

                GameObject hLine = Instantiate(halfHorizontalLinePrefab, content.transform);
                RectTransform hrt = hLine.GetComponent<RectTransform>();

                // pivot과 anchor를 아래 기준으로 설정
                hrt.pivot = new Vector2(0f, 0f);
                hrt.anchorMin = new Vector2(0f, 0f);
                hrt.anchorMax = new Vector2(1f, 0f); // 가로로 Stretch 되도록

                hrt.anchoredPosition = new Vector2(0f, y); // 아래에서 위로 배치
                hrt.sizeDelta = new Vector2(0f, 8f); // 높이 1px, 너비는 anchor로 결정됨

                if (isPlayMode)
                {
                    Image img = hLine.GetComponent<Image>();
                    if (img != null)
                    {
                        Color color = img.color;
                        color.a = 0f; // 완전 투명
                        img.color = color;
                    }
                }

                horizontalLines.Add(hLine);
            }
        }

        //수평선 제작(1/4박)
        for (int i = 0; i <= 500; i++)
        {
            float y = i * 100f;

            if (i % 2 != 0)
            {

                GameObject hLine = Instantiate(quaterHorizontalLinePrefab, content.transform);
                RectTransform hrt = hLine.GetComponent<RectTransform>();

                // pivot과 anchor를 아래 기준으로 설정
                hrt.pivot = new Vector2(0f, 0f);
                hrt.anchorMin = new Vector2(0f, 0f);
                hrt.anchorMax = new Vector2(1f, 0f); // 가로로 Stretch 되도록

                hrt.anchoredPosition = new Vector2(0f, y); // 아래에서 위로 배치
                hrt.sizeDelta = new Vector2(0f, 6f);

                if (isPlayMode)
                {
                    Image img = hLine.GetComponent<Image>();
                    if (img != null)
                    {
                        Color color = img.color;
                        color.a = 0f; // 완전 투명
                        img.color = color;
                    }
                }

                horizontalLines.Add(hLine);
            }
        }

        //수평선 제작(1/8박)
        for (int i = 0; i <= 1000; i++)
        {
            float y = i * 50f;

            if (i % 2 != 0)
            {

                GameObject hLine = Instantiate(eighthHorizontalLinePrefab, content.transform);
                RectTransform hrt = hLine.GetComponent<RectTransform>();

                // pivot과 anchor를 아래 기준으로 설정
                hrt.pivot = new Vector2(0f, 0f);
                hrt.anchorMin = new Vector2(0f, 0f);
                hrt.anchorMax = new Vector2(1f, 0f); // 가로로 Stretch 되도록

                hrt.anchoredPosition = new Vector2(0f, y); // 아래에서 위로 배치
                hrt.sizeDelta = new Vector2(0f, 4f);

                if (isPlayMode)
                {
                    Image img = hLine.GetComponent<Image>();
                    if (img != null)
                    {
                        Color color = img.color;
                        color.a = 0f; // 완전 투명
                        img.color = color;
                    }
                }

                horizontalLines.Add(hLine);
            }
        }

        //수평선 제작(1/16박)
        for (int i = 0; i <= 2000; i++)
        {
            float y = i * 25f;

            if (i % 2 != 0)
            {

                GameObject hLine = Instantiate(sixteenthHorizontalLinePrefab, content.transform);
                RectTransform hrt = hLine.GetComponent<RectTransform>();

                // pivot과 anchor를 아래 기준으로 설정
                hrt.pivot = new Vector2(0f, 0f);
                hrt.anchorMin = new Vector2(0f, 0f);
                hrt.anchorMax = new Vector2(1f, 0f); // 가로로 Stretch 되도록

                hrt.anchoredPosition = new Vector2(0f, y); // 아래에서 위로 배치
                hrt.sizeDelta = new Vector2(0f, 2f);

                if (isPlayMode)
                {
                    Image img = hLine.GetComponent<Image>();
                    if (img != null)
                    {
                        Color color = img.color;
                        color.a = 0f; // 완전 투명
                        img.color = color;
                    }
                }

                horizontalLines.Add(hLine);
            }
        }

        //수평선과 수직선들의 좌표를 담는 리스트
        foreach (var vLine in verticalLines)
            verticalXs.Add(vLine.GetComponent<RectTransform>().anchoredPosition.x);
        foreach (var hLine in horizontalLines)
            horizontalYs.Add(hLine.GetComponent<RectTransform>().anchoredPosition.y);

        verticalXs.Sort();
        horizontalYs.Sort();
    }

    void Start()
    {
        Init();
    }




    void Update()
    {

    }
}
