// NoteMover.cs
using UnityEngine;

public class NoteMover : MonoBehaviour
{
    float spawnY;               // 스폰 위치 Y
    float spawnTime;            // 생성 시간 (Time.time)
    float speed;                // 픽셀/초
    RectTransform rt;
    PlayController pc;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public void Init(float initialY, float speedY, PlayController controller)
    {
        spawnY = initialY;
        speed = speedY;
        spawnTime = Time.time;
        pc = controller;

        if (rt == null)
        {
            rt = GetComponent<RectTransform>();
        }

        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, spawnY);
    }

    void Update()
    {
        if (pc == null)
        {
            Debug.LogError($"[{gameObject.name}] Update - PlayController is null!");
            return;
        }

        if (!pc.isPlaying)
        {
            return;
        }

        float elapsed = Time.time - pc.playStartTime;
        float newY = spawnY - speed * elapsed;
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, newY);

        if (newY < -100f)
            Destroy(gameObject);
    }
}