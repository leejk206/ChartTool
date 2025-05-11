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
