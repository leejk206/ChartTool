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
        rootCanvas = GameObject.Find("RootCanvas"); // ĵ����
        content = GameObject.Find("Content"); // ��ũ�� ���� ���빰
        verticalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/VerticalLine"); // ������
        scrollView = GameObject.Find("ScrollView"); // ��ũ�� ������Ʈ
        contentRect = content.GetComponent<RectTransform>(); // ���빰 transform
        scrollViewRect = scrollView.GetComponent<RectTransform>(); // ��ũ�� transform

        wholeHorizontalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/HorizontalLineWhole"); // 1�� ����
        halfHorizontalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/HorizontalLineHalf"); // 1/2�� ����
        quaterHorizontalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/HorizontalLineQuater"); // 1/4�� ����
        eighthHorizontalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/HorizontalLineEighth"); // 1/8�� ����
        sixteenthHorizontalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/HorizontalLineSixteenth"); // 1/16�� ����

        verticalLines = new();
        horizontalLines = new();
        verticalXs = new();
        horizontalYs = new();

        // ��ũ�� ���� ����
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, 50000);

        contentHeight = contentRect.sizeDelta.y; // ���빰 ���� ũ��
        beatHeight = (rootCanvas.GetComponent<RectTransform>().rect.height) / 4;


        int numLines = 8;
        float totalWidth = (rootCanvas.GetComponent<RectTransform>().rect.width) * 0.92f;

        TimeController.Timer.ExecuteAfterTime(0.01f, () => scrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0);

        if (SceneManager.GetActiveScene().name == "PlayMode")
            isPlayMode = true;

        // ������ ����
        for (int i = 0; i < numLines; i++)
        {
            float x = -(totalWidth / 2f) + (totalWidth / (numLines - 1)) * i;
            GameObject vLine = Instantiate(verticalLinePrefab, content.transform);
            RectTransform vrt = vLine.GetComponent<RectTransform>();

            // �Ʒ����� �����ؼ� ���� �׸����� ����
            vrt.pivot = new Vector2(0.5f, 0f);               // �Ʒ� ����
            vrt.anchorMin = new Vector2(0.5f, 0f);           // Content �Ʒ�
            vrt.anchorMax = new Vector2(0.5f, 0f);

            vrt.anchoredPosition = new Vector2(x, 0);        // Content�� �Ʒ��ʿ� ��ġ
            vrt.sizeDelta = new Vector2(vrt.sizeDelta.x, 50000f); // ���� ���� ����

            if (isPlayMode)
            {
                Image img = vLine.GetComponent<Image>();
                if (img != null)
                {
                    Color color = img.color;
                    color.a = 0f; // ���� ����
                    img.color = color;
                }
            }

            verticalLines.Add(vLine);
        }

        //���� ����(1��)
        for (int i = 0; i <= 125; i++)
        {
            float y = i * 400f;

            GameObject hLine = Instantiate(wholeHorizontalLinePrefab, content.transform);
            RectTransform hrt = hLine.GetComponent<RectTransform>();

            // pivot�� anchor�� �Ʒ� �������� ����
            hrt.pivot = new Vector2(0f, 0f);
            hrt.anchorMin = new Vector2(0f, 0f);
            hrt.anchorMax = new Vector2(1f, 0f); // ���η� Stretch �ǵ���

            hrt.anchoredPosition = new Vector2(0f, y); // �Ʒ����� ���� ��ġ
            hrt.sizeDelta = new Vector2(0f, 12f); // ���� 1px, �ʺ�� anchor�� ������

            if (isPlayMode)
            {
                Image img = hLine.GetComponent<Image>();
                if (img != null)
                {
                    Color color = img.color;
                    color.a = 0f; // ���� ����
                    img.color = color;
                }
            }

            horizontalLines.Add(hLine);
        }

        //���� ����(1/2��)
        for (int i = 0; i <= 250; i++)
        {
            float y = i * 200f;

            if (i % 2 != 0)
            {

                GameObject hLine = Instantiate(halfHorizontalLinePrefab, content.transform);
                RectTransform hrt = hLine.GetComponent<RectTransform>();

                // pivot�� anchor�� �Ʒ� �������� ����
                hrt.pivot = new Vector2(0f, 0f);
                hrt.anchorMin = new Vector2(0f, 0f);
                hrt.anchorMax = new Vector2(1f, 0f); // ���η� Stretch �ǵ���

                hrt.anchoredPosition = new Vector2(0f, y); // �Ʒ����� ���� ��ġ
                hrt.sizeDelta = new Vector2(0f, 8f); // ���� 1px, �ʺ�� anchor�� ������

                if (isPlayMode)
                {
                    Image img = hLine.GetComponent<Image>();
                    if (img != null)
                    {
                        Color color = img.color;
                        color.a = 0f; // ���� ����
                        img.color = color;
                    }
                }

                horizontalLines.Add(hLine);
            }
        }

        //���� ����(1/4��)
        for (int i = 0; i <= 500; i++)
        {
            float y = i * 100f;

            if (i % 2 != 0)
            {

                GameObject hLine = Instantiate(quaterHorizontalLinePrefab, content.transform);
                RectTransform hrt = hLine.GetComponent<RectTransform>();

                // pivot�� anchor�� �Ʒ� �������� ����
                hrt.pivot = new Vector2(0f, 0f);
                hrt.anchorMin = new Vector2(0f, 0f);
                hrt.anchorMax = new Vector2(1f, 0f); // ���η� Stretch �ǵ���

                hrt.anchoredPosition = new Vector2(0f, y); // �Ʒ����� ���� ��ġ
                hrt.sizeDelta = new Vector2(0f, 6f);

                if (isPlayMode)
                {
                    Image img = hLine.GetComponent<Image>();
                    if (img != null)
                    {
                        Color color = img.color;
                        color.a = 0f; // ���� ����
                        img.color = color;
                    }
                }

                horizontalLines.Add(hLine);
            }
        }

        //���� ����(1/8��)
        for (int i = 0; i <= 1000; i++)
        {
            float y = i * 50f;

            if (i % 2 != 0)
            {

                GameObject hLine = Instantiate(eighthHorizontalLinePrefab, content.transform);
                RectTransform hrt = hLine.GetComponent<RectTransform>();

                // pivot�� anchor�� �Ʒ� �������� ����
                hrt.pivot = new Vector2(0f, 0f);
                hrt.anchorMin = new Vector2(0f, 0f);
                hrt.anchorMax = new Vector2(1f, 0f); // ���η� Stretch �ǵ���

                hrt.anchoredPosition = new Vector2(0f, y); // �Ʒ����� ���� ��ġ
                hrt.sizeDelta = new Vector2(0f, 4f);

                if (isPlayMode)
                {
                    Image img = hLine.GetComponent<Image>();
                    if (img != null)
                    {
                        Color color = img.color;
                        color.a = 0f; // ���� ����
                        img.color = color;
                    }
                }

                horizontalLines.Add(hLine);
            }
        }

        //���� ����(1/16��)
        for (int i = 0; i <= 2000; i++)
        {
            float y = i * 25f;

            if (i % 2 != 0)
            {

                GameObject hLine = Instantiate(sixteenthHorizontalLinePrefab, content.transform);
                RectTransform hrt = hLine.GetComponent<RectTransform>();

                // pivot�� anchor�� �Ʒ� �������� ����
                hrt.pivot = new Vector2(0f, 0f);
                hrt.anchorMin = new Vector2(0f, 0f);
                hrt.anchorMax = new Vector2(1f, 0f); // ���η� Stretch �ǵ���

                hrt.anchoredPosition = new Vector2(0f, y); // �Ʒ����� ���� ��ġ
                hrt.sizeDelta = new Vector2(0f, 2f);

                if (isPlayMode)
                {
                    Image img = hLine.GetComponent<Image>();
                    if (img != null)
                    {
                        Color color = img.color;
                        color.a = 0f; // ���� ����
                        img.color = color;
                    }
                }

                horizontalLines.Add(hLine);
            }
        }

        //���򼱰� ���������� ��ǥ�� ��� ����Ʈ
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
