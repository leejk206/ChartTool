using UnityEngine;
using Newtonsoft.Json; // Json.NET ��ġ�� ���
using System.IO;
using System.Collections.Generic;

/// <summary>
/// 플레이 모드의 전반적인 동작을 제어하는 컨트롤러 클래스
/// </summary>
public class PlayController : MonoBehaviour
{
    [Header("BPM 설정")]
    public float BPM = 120f;                    // 곡의 BPM 값
    [HideInInspector]
    public float beatHeight = 25f;              // 한 박자가 차지하는 세로 픽셀 크기
    [Header("노트 JSON 파일 이름")]
    public string jsonFileName = "chart.json";   // 채보 데이터가 저장된 JSON 파일명
    public AudioClip bgmClip;                   // 재생할 배경음악

    [HideInInspector] public float playStartTime;

    public bool isPlaying;                             // 현재 플레이 중인지 여부
    public float startTime;                            // 플레이 시작 시간

    private void Awake()
    {
        ResetPlayState();
    }

    private void OnEnable()
    {
        ResetPlayState();
    }

    /// <summary>
    /// 플레이 상태를 초기화하는 메서드
    /// </summary>
    private void ResetPlayState()
    {
        isPlaying = false;
        startTime = 0f;
        playStartTime = 0f;

        // 노트 제거
        if (Managers.Chart != null)
        {
            Managers.Chart.ClearAllNotesFromScene();
        }

        // BGM 정지
        if (AudioController.Instance != null)
        {
            AudioController.Instance.StopBGM();
        }

        // 라인 렌더러 초기화
        PlayModeLineRender lineRender = GameObject.Find("PlayModeLineRender")?.GetComponent<PlayModeLineRender>();
        if (lineRender != null)
        {
            lineRender.OnPlayStop();
        }
    }

    /// <summary>
    /// 플레이 모드에서 노트를 렌더링하는 메서드
    /// </summary>
    private void PlayModeRenderNotes()
    {
        // 1박자당 시간(초) 계산
        float secondsPerBeat = 60f / BPM;
        // 1박자당 이동 거리(픽셀)를 시간으로 나누어 속도 계산
        float speedY = (beatHeight * 16f) / secondsPerBeat; // 16은 한 마디의 16비트를 의미
        float centerY = 800f; // 화면 중앙 Y 위치

        // 노트 데이터 순회
        for (int v = 0; v < Managers.Chart.NormalNotes.Count; v++)
        {
            foreach (int h in Managers.Chart.NormalNotes[v])
            {
                // 노트 프리팹 로드 및 생성
                GameObject notePrefab = Resources.Load<GameObject>("Prefabs/Notes/NormalNote");
                if (notePrefab == null)
                {
                    Debug.LogError("노트 프리팹을 찾을 수 없습니다: Prefabs/Notes/NormalNote");
                    continue;
                }

                GameObject noteInstance = Instantiate(notePrefab, GameObject.Find("RootCanvas").transform);

                // RectTransform 설정
                RectTransform rt = noteInstance.GetComponent<RectTransform>();
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchorMin = new Vector2(0.5f, 0f);
                rt.anchorMax = new Vector2(0.5f, 0f);

                // 노트 위치 설정
                float totalWidth = Screen.width * 0.92f;
                float x = -(totalWidth / 2f) + (totalWidth / 7f) * v;
                float y = centerY + (h * beatHeight);

                rt.anchoredPosition = new Vector2(x, y);
                rt.sizeDelta = new Vector2(50f, 20f);

                // NoteMover 컴포넌트 추가 후 Init 호출
                var mover = noteInstance.AddComponent<NoteMover>();
                mover.Init(y, speedY, this);  // 현재 PlayController 인스턴스를 전달

                // 생성된 노트 저장
                if (Managers.Chart.Notes.Count <= v)
                {
                    Managers.Chart.Notes.Add(new Dictionary<int, GameObject>());
                }
                Managers.Chart.Notes[v][h] = noteInstance;
            }
        }
    }

    /// <summary>
    /// 플레이 시작 메서드
    /// </summary>
    public void BeginPlay()
    {
        Debug.Log($"BeginPlay on “{gameObject.name}” ID={GetInstanceID()}");

        // 플레이 시작 시간 기록 및 상태 설정
        playStartTime = Time.time;
        startTime = Time.time;
        isPlaying = true;
        Debug.Log($"[BeginPlay] 상태 설정 완료 - Controller Instance ID: {gameObject.GetInstanceID()}, isPlaying: {isPlaying}, playStartTime: {playStartTime}");

        // 채보 데이터 로드 및 렌더링
        string path = Path.Combine(Application.persistentDataPath, "chart.json");
        Managers.Chart.LoadChartFromJson(path);
        Debug.Log($"[BeginPlay] 채보 데이터 로드 완료 - Controller Instance ID: {gameObject.GetInstanceID()}");
        
        PlayModeRenderNotes(); // 노트 렌더링 방식 변경
        Debug.Log($"[BeginPlay] 노트 렌더링 완료 - Controller Instance ID: {gameObject.GetInstanceID()}");

        // 라인 렌더러 플레이 시작 알림
        PlayModeLineRender lineRender = GameObject.Find("PlayModeLineRender").GetComponent<PlayModeLineRender>();
        if (lineRender != null)
        {
            lineRender.OnPlayStart();
            Debug.Log($"[BeginPlay] 라인 렌더러 시작 - Controller Instance ID: {gameObject.GetInstanceID()}");
        }

        // BGM 재생 설정
        if (bgmClip != null)
        {
            AudioController.Instance.PlayBGM(bgmClip);
        }
        else
        {
            bgmClip = Resources.Load<AudioClip>($"AudioClip1");
            AudioController.Instance.PlayBGM(bgmClip);
        }
        
        Debug.Log($"[BeginPlay] 종료 - Controller Instance ID: {gameObject.GetInstanceID()}, isPlaying: {isPlaying}, playStartTime: {playStartTime}");
    }

    /// <summary>
    /// 플레이 정지 메서드
    /// </summary>
    public void PlayStop()
    {
        // 플레이 상태 변경
        isPlaying = false;
        
        // BGM 정지
        AudioController.Instance.StopBGM();

        // 노트 제거
        Managers.Chart.ClearAllNotesFromScene();

        // 라인 렌더러 정지 알림
        PlayModeLineRender lineRender = GameObject.Find("PlayModeLineRender").GetComponent<PlayModeLineRender>();
        if (lineRender != null)
        {
            lineRender.OnPlayStop();
        }

        // 시작 시간 초기화
        startTime = 0f;
        playStartTime = 0f;
    }
}