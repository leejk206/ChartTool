using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이 모드에서 라인을 렌더링하고 이동시키는 컴포넌트
/// </summary>
public class PlayModeLineRender : MonoBehaviour
{
    [SerializeField] private PlayController playController;
    float lineSpacing = 200f;                 // 라인 간격 (16비트 단위, 한 박자당 라인 하나일 경우 400px, 반 박자당 라인 하나일 경우 200px)
    public int visibleLinesCount = 4;                 // 화면에 표시될 라인 수
    
    private GameObject horizontalLinePrefab;          // 라인 프리팹
    private List<GameObject> horizontalLinePool;      // 라인 오브젝트 풀
    private Canvas canvas;                            // 루트 캔버스
    private float screenHeight;                       // 화면 높이
    private bool isPlaying = false;                   // 플레이 상태
    private float moveSpeed;                          // 라인 이동 속도
    private List<float> lineInitialYPositions;        // 각 라인의 초기 Y 위치

    [HideInInspector] public float playStartTime;

    RectTransform canvasRect;
    List<RectTransform> lineRTs = new List<RectTransform>();
    PlayController pc;

    public List<GameObject> verticalLines;

    void Awake()
    {
        // PlayController 참조
        if (playController == null)
            playController = FindObjectOfType<PlayController>();

        // RootCanvas RectTransform 가져오기
        canvas = GameObject.Find("RootCanvas").GetComponent<Canvas>();
        if (canvas != null)
            screenHeight = canvas.GetComponent<RectTransform>().rect.height;

        if (playController != null)
            lineSpacing = playController.beatHeight * 16f;

        lineInitialYPositions = new List<float>();
    }

    void OnEnable()
    {
        ResetState();
    }

    private void ResetState()
    {
        isPlaying = false;
        moveSpeed = 0f;

        if (horizontalLinePool != null)
        {
            foreach (var line in horizontalLinePool)
            {
                if (line != null)
                {
                    Destroy(line);
                }
            }
            horizontalLinePool.Clear();
        }

        Start();
    }

    void Start()
    {
        if (playController == null) return;

        // 프리팹 로드
        horizontalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/HorizontalLineWhole");
        if (horizontalLinePrefab == null) return;

        horizontalLinePool = new List<GameObject>();
        
        InitializeLinePool();
        ResetLines();
        isPlaying = false; // 시작 시 isPlaying을 false로 설정

        verticalLines = new();

        float totalWidth = (GameObject.Find("RootCanvas").GetComponent<RectTransform>().rect.width) * 0.92f;
        GameObject verticalLinePrefab = Resources.Load<GameObject>("Prefabs/Line/VerticalLine"); // 세로선

        // 세로선 생성
        for (int i = 0; i < 8; i++)
        {
            float x = -(totalWidth / 2f) + (totalWidth / (8 - 1)) * i;
            GameObject vLine = Instantiate(verticalLinePrefab, canvas.transform);
            RectTransform vrt = vLine.GetComponent<RectTransform>();

            // 피봇과 앵커를 아래 기준으로 설정
            vrt.pivot = new Vector2(0.5f, 0f);               // 피봇 설정
            vrt.anchorMin = new Vector2(0.5f, 0f);           // Content 피봇
            vrt.anchorMax = new Vector2(0.5f, 0f);

            vrt.anchoredPosition = new Vector2(x, 0);        // Content에서 피봇까지 거리
            vrt.sizeDelta = new Vector2(vrt.sizeDelta.x, 5000f); // 세로선 길이 설정

            verticalLines.Add(vLine);
        }
    }

    void ResetLines()
    {
        if (horizontalLinePool == null) return;

        if (playController.lineForBeat != -1 && playController.lineForBeat != 0)
        {
            lineSpacing = (16 / playController.lineForBeat) * 25f;
        }

        lineInitialYPositions.Clear();
        // 모든 라인을 초기 위치로 재배치
        for (int i = 0; i < horizontalLinePool.Count; i++)
        {
            float yPos = (i * lineSpacing);
            PositionLine(horizontalLinePool[i], yPos);
            lineInitialYPositions.Add(yPos);
        }
    }

    public void OnPlayStart()
    {
        if (playController == null) return;

        isPlaying = true;
        moveSpeed = playController.beatHeight * 16f * (playController.BPM / 60f);
        ResetLines();
    }

    public void OnPlayStop()
    {
        isPlaying = false;
        ResetLines();
    }

    GameObject CreateHorizontalLine()
    {
        if (canvas == null) return null;

        // 라인    생성 및 설정
        GameObject line = Instantiate(horizontalLinePrefab, canvas.transform);
        RectTransform rt = line.GetComponent<RectTransform>();

        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchorMin = new Vector2(0.5f, 0f);
        rt.anchorMax = new Vector2(0.5f, 0f);
        rt.sizeDelta = new Vector2(canvas.GetComponent<RectTransform>().rect.width, 4f); // 한 박자 라인은 더 굵게

        // 한 박자 라인은 더 진하게 표시
        Image lineImage = line.GetComponent<Image>();
        if (lineImage != null)
        {
            Color color = lineImage.color;
            color.a = 1f;
            lineImage.color = color;
        }

        return line;
    }

    void InitializeLinePool()
    {
        // 오브젝트 풀 초기화
        for (int i = 0; i < visibleLinesCount; i++)
        {
            GameObject line = CreateHorizontalLine();
            horizontalLinePool.Add(line);
        }
        ResetLines();
    }

    void PositionLine(GameObject line, float yPosition)
    {
        if (line == null) return;
        RectTransform rt = line.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0f, yPosition);
    }

    void Update()
    {
        if (!playController || !playController.isPlaying) return;

        float elapsed = Time.time - playController.playStartTime;
        float speed = playController.beatHeight * 16f * (playController.BPM / 60f);

        for (int i = 0; i < horizontalLinePool.Count; i++)
        {
            GameObject line = horizontalLinePool[i];
            if (line == null) continue;

            RectTransform rt = line.GetComponent<RectTransform>();
            
            // 절대 위치 계산
            float newY = lineInitialYPositions[i] - speed * elapsed;

            // 화면 아래로 벗어난 라인을 위로 재배치
            if (newY < -100f)
            {
                float highestY = FindHighestLineY();
                newY = highestY + lineSpacing;
                lineInitialYPositions[i] = newY + speed * elapsed; // 새로운 초기 위치 계산
            }

            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, newY);
        }
    }

    float FindHighestLineY()
    {
        if (horizontalLinePool == null) return 0f;

        float highest = float.MinValue;
        foreach (GameObject line in horizontalLinePool)
        {
            if (line == null) continue;
            float y = line.GetComponent<RectTransform>().anchoredPosition.y;
            if (y > highest)
                highest = y;
        }
        return highest;
    }

}

