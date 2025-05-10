using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayManager
{
    [Header("차트 JSON 파일 경로 (persistentDataPath 하위)")]
    public string jsonFileName = "chart.json";

    [Header("BPM 설정")]
    public float BPM = 120f;

    [Header("노트가 배치된 Content RectTransform")]
    public RectTransform contentRect;

    [Header("비트 하나당 세로 픽셀 간격 (Edit 모드와 동일하게)")]
    public float beatHeight = 25f;

    float speedY;
    float startTime;
    bool isPlaying = false;

    // Start is called before the first frame update
    public void Init()
    {
        float secondsPerBeat = 60f / BPM;
        speedY = beatHeight / secondsPerBeat;
    }

    public void BeginPlay()
    {
        // 1) 화면 위에 남은 노트 제거
        Managers.Chart.ClearAllNotesFromScene();

        // 2) JSON에서 NormalNotes 불러오기
        string path = Path.Combine(Application.persistentDataPath, jsonFileName);
        Managers.Chart.LoadChartFromJson(path);

        // 3) Edit 모드에서 쓰던 Render 함수로 씬 위에 노트들 생성
        Managers.Chart.RenderNotesFromData();

        // 4) Scroll 시작 위치 초기화
        contentRect.anchoredPosition = Vector2.zero;

        // 5) 시간 기록 후 스크롤 시작
        startTime = Time.time;
        isPlaying = true;
    }

    // Update is called once per frame
    public void OnUpdate()
    {
        if (!isPlaying) return;

        float elapsed = Time.time - startTime;
        // 아래 방향으로 스크롤
        contentRect.anchoredPosition = new Vector2(
            contentRect.anchoredPosition.x,
            -speedY * elapsed
        );
    }
}
