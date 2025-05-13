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

    int horizontalLineCount;

    public List<GameObject> verticalLines;
    public List<GameObject> horizontalLines;

    public List<GameObject> quaterLines;
    public List<GameObject> eighthLines;
    public List<GameObject> sixteenthLines;

    public List<float> verticalXs;
    public List<float> horizontalYs;

    bool isPlayMode = false;

    PlayController pc;

    public void Init()
    {
        rootCanvas = GameObject.Find("RootCanvas"); // 루트캔버스
        content = GameObject.Find("Content"); // 컨텐츠 오브젝트
        verticalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/VerticalLine"); // 세로선
        scrollView = GameObject.Find("ScrollView"); // 스크롤뷰
        contentRect = content.GetComponent<RectTransform>(); // 컨텐츠 transform
        scrollViewRect = scrollView.GetComponent<RectTransform>(); // 스크롤뷰 transform

        wholeHorizontalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/HorizontalLineWhole"); // 1박
        halfHorizontalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/HorizontalLineHalf"); // 1/2박
        quaterHorizontalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/HorizontalLineQuater"); // 1/4박
        eighthHorizontalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/HorizontalLineEighth"); // 1/8박
        sixteenthHorizontalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/HorizontalLineSixteenth"); // 1/16박

        verticalLines = new();
        horizontalLines = new();
        verticalXs = new();
        horizontalYs = new();

        quaterLines = new();
        eighthLines = new();
        sixteenthLines = new();

        // 컨텐츠 크기 설정
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, 50000);

        int numLines = 8;
        float totalWidth = (rootCanvas.GetComponent<RectTransform>().rect.width) * 0.92f;

        TimeController.Timer.ExecuteAfterTime(0.01f, () => scrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0);

        if (SceneManager.GetActiveScene().name == "PlayMode")
            isPlayMode = true;

        // 세로선 생성
        for (int i = 0; i < numLines; i++)
        {
            float x = -(totalWidth / 2f) + (totalWidth / (numLines - 1)) * i;
            GameObject vLine = Instantiate(verticalLinePrefab, content.transform);
            RectTransform vrt = vLine.GetComponent<RectTransform>();

            // 피봇과 앵커를 아래 기준으로 설정
            vrt.pivot = new Vector2(0.5f, 0f);               // 피봇 설정
            vrt.anchorMin = new Vector2(0.5f, 0f);           // Content 피봇
            vrt.anchorMax = new Vector2(0.5f, 0f);

            vrt.anchoredPosition = new Vector2(x, 0);        // Content에서 피봇까지 거리
            vrt.sizeDelta = new Vector2(vrt.sizeDelta.x, 50000f); // 세로선 길이 설정

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

        //가로선 생성(1박)
        for (int i = 0; i <= 125; i++)
        {
            float y = i * 400f;

            GameObject hLine = Instantiate(wholeHorizontalLinePrefab, content.transform);
            RectTransform hrt = hLine.GetComponent<RectTransform>();

            // pivot과 anchor를 아래 기준으로 설정
            hrt.pivot = new Vector2(0f, 0f);
            hrt.anchorMin = new Vector2(0f, 0f);
            hrt.anchorMax = new Vector2(1f, 0f); // 가로로 Stretch 되도록

            hrt.anchoredPosition = new Vector2(0f, y); // 아래에서 얼마나 떨어질지
            hrt.sizeDelta = new Vector2(0f, 12f); // 세로선 길이 1px, 실제 길이는 anchor에 의해 결정됨

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

        //가로선 생성(1/2박)
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

                hrt.anchoredPosition = new Vector2(0f, y); // 아래에서 얼마나 떨어질지
                hrt.sizeDelta = new Vector2(0f, 8f); // 세로선 길이 1px, 실제 길이는 anchor에 의해 결정됨

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

        //가로선 생성(1/4박)
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

                hrt.anchoredPosition = new Vector2(0f, y); // 아래에서 얼마나 떨어질지
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
                quaterLines.Add(hLine);
            }
        }

        // (1/8)
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

                hrt.anchoredPosition = new Vector2(0f, y); // 아래에서 얼마나 떨어질지
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
                eighthLines.Add(hLine);
            }
        }

        //가로선 생성(1/16박)
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

                hrt.anchoredPosition = new Vector2(0f, y); // 아래에서 얼마나 떨어질지
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
                sixteenthLines.Add(hLine);
            }
        }

        //좌표 저장
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

    public bool isShowQuater = true;
    public void ShowQuater()
    {
        if (isShowQuater)
        {
            isShowQuater = false;
            foreach (var lines in quaterLines)
            {
                Image img = lines.GetComponent<Image>();
                if (img != null)
                {
                    Color color = img.color;
                    color.a = 0f;
                    img.color = color;
                }
            }
        }
        else
        {
            isShowQuater = true;
            foreach (var lines in quaterLines)
            {
                Image img = lines.GetComponent<Image>();
                if (img != null)
                {
                    Color color = img.color;
                    color.a = 100f;
                    img.color = color;
                }
            }
        }
    }

    public bool isShowEighth = true;
    public void ShowEighth()
    {
        if (isShowEighth)
        {
            isShowEighth = false;
            foreach (var lines in eighthLines)
            {
                Image img = lines.GetComponent<Image>();
                if (img != null)
                {
                    Color color = img.color;
                    color.a = 0f;
                    img.color = color;
                }
            }
        }
        else
        {
            isShowEighth = true;
            foreach (var lines in eighthLines)
            {
                Image img = lines.GetComponent<Image>();
                if (img != null)
                {
                    Color color = img.color;
                    color.a = 100f;
                    img.color = color;
                }
            }
        }
    }

    public bool isShowSIxteenth = true;
    public void ShowSixteenth()
    {
        if (isShowSIxteenth)
        {
            isShowSIxteenth = false;
            foreach (var lines in sixteenthLines)
            {
                Image img = lines.GetComponent<Image>();
                if (img != null)
                {
                    Color color = img.color;
                    color.a = 0f;
                    img.color = color;
                }
            }
        }
        else
        {
            isShowSIxteenth = true;
            foreach (var lines in sixteenthLines)
            {
                Image img = lines.GetComponent<Image>();
                if (img != null)
                {
                    Color color = img.color;
                    color.a = 100f;
                    img.color = color;
                }
            }
        }
    }
}
