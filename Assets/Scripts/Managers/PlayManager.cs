using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayManager
{
    [Header("��Ʈ JSON ���� ��� (persistentDataPath ����)")]
    public string jsonFileName = "chart.json";

    [Header("BPM ����")]
    public float BPM = 120f;

    [Header("��Ʈ�� ��ġ�� Content RectTransform")]
    public RectTransform contentRect;

    [Header("��Ʈ �ϳ��� ���� �ȼ� ���� (Edit ���� �����ϰ�)")]
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
        // �Ʒ� �������� ��ũ��
        contentRect.anchoredPosition = new Vector2(
            contentRect.anchoredPosition.x,
            -speedY * elapsed
        );
    }
}
