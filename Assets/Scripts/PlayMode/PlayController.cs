using UnityEngine;
using Newtonsoft.Json; // Json.NET 설치한 경우
using System.IO;

public class PlayController : MonoBehaviour
{
    [Header("BPM 설정")]
    public float BPM = 120f;
    [Header("한 박자당 세로 픽셀")]
    public float beatHeight = 25f;
    [Header("차트 JSON 파일 이름")]
    public string jsonFileName = "chart.json";
    public AudioClip bgmClip;

    bool isPlaying;
    float startTime;

    /// <summary>
    /// ScrollView 없이 Canvas 위에 모든 노트를 스폰하고,
    /// 각각 NoteMover를 통해 아래로 스크롤(이동)시킵니다.
    /// </summary>
    public void PlayModeRenderNotesNoScroll()
    {
        // 1) 씬에 남은 노트만 삭제 (데이터는 LoadChart 이후 세팅된 상태)
        Managers.Chart.ClearAllNotesFromScene();

        // 2) Canvas RectTransform 가져오기
        RectTransform canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();

        // 3) 열 개수, 전체 폭, 시작 X, 셀 너비 계산
        int numLines = Managers.Chart.NormalNotes.Count;   // 보통 7
        float totalW = canvasRect.rect.width;
        float startX = -totalW / 2f;
        float cellWidth = totalW / (numLines - 1);

        // 4) 노트 이동 속도 계산 (px/sec)
        float speedY = beatHeight * BPM / 60f;

        // 5) JSON 데이터 순회하며 노트 인스턴스화 & NoteMover 부착
        for (int v = 0; v < numLines; v++)
        {
            float centerX = startX + cellWidth * v;
            foreach (int h in Managers.Chart.NormalNotes[v])
            {
                float centerY = h * beatHeight + (beatHeight / 2f);

                // 프리팹 로드 & 부모는 Canvas
                GameObject prefab = Resources.Load<GameObject>("Prefabs/Notes/NormalNote");
                GameObject noteInstance = Instantiate(prefab, canvasRect);

                // RectTransform 세팅
                RectTransform rt = noteInstance.GetComponent<RectTransform>();
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchorMin = new Vector2(0.5f, 0f);
                rt.anchorMax = new Vector2(0.5f, 0f);
                rt.anchoredPosition = new Vector2(centerX, centerY);
                rt.sizeDelta = new Vector2(cellWidth, beatHeight);

                // NoteMover 부착
                var mover = noteInstance.AddComponent<NoteMover>();
                mover.speed = speedY;

                // 런타임 관리용 딕셔너리에 저장
                Managers.Chart.Notes[v].Add(h, noteInstance);
            }
        }

        Debug.Log("PlayMode용 노트 렌더링 완료");
    }

    // 예: Play 시작 시
    public void BeginPlay()
    {
        if (bgmClip != null)
        {
            AudioController.Instance.PlayBGM(bgmClip);
            Debug.Log("bgmcilp not null");
        }
        else
        {
            bgmClip = Resources.Load<AudioClip>($"AudioClip1");
            AudioController.Instance.PlayBGM(bgmClip);
            Debug.Log("bgmcilp null");

        }

        string path = Path.Combine(Application.persistentDataPath, "chart.json");

        // 1) 씬 오브젝트만 삭제
        Managers.Chart.ClearAllNotesFromScene();

        // 2) JSON 데이터 로드 (데이터 구조는 Init() or wrapper로 채워짐)
        Managers.Chart.LoadChartFromJson(path);

        Managers.Chart.PlayModeRenderNotesFromData();

        startTime = Time.time;
        isPlaying = true;
    }
}