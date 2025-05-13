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
    float lineSpacing = 400f;                 // 라인 간격 (16비트 단위, 400 * 16 = 6400px)
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
    }

    void ResetLines()
    {
        if (horizontalLinePool == null) return;

        lineInitialYPositions.Clear();
        // 모든 라인을 초기 위치로 재배치
        for (int i = 0; i < horizontalLinePool.Count; i++)
        {
            float yPos = screenHeight + (i * lineSpacing);
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

