// NoteMover.cs
using UnityEngine;

public class NoteMover : MonoBehaviour
{
    float spawnY;               // 스폰할 때의 Y
    float spawnTime;            // 스폰 시각 (Time.time)
    float speed;                // 픽셀/초
    RectTransform rt;
    PlayController pc;
    
    void Awake()
    {
        rt = GetComponent<RectTransform>();
        pc = GameObject.Find("PlayController").GetComponent<PlayController>();
    }
    public void Init(float initialY, float speedY)
    {
        spawnY = initialY;
        speed = speedY;
        spawnTime = Time.time;
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, spawnY);
    }


    void Update()
    {
        if (!pc.isPlaying) return;

        float elapsed = Time.time - pc.playStartTime;
        float newY = spawnY - speed * elapsed;
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, newY);

        if (newY < -100f)
            Destroy(gameObject);
    }
}