// NoteMover.cs
using UnityEngine;

public class NoteMover : MonoBehaviour
{
    [Tooltip("초당 이동할 Y 픽셀 속도")]
    public float speed;

    RectTransform rt;
    float destroyY = -100f; // 이 값보다 내려가면 파괴

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        // 매 프레임 아래로 이동
        rt.anchoredPosition += Vector2.down * speed * Time.deltaTime;

        if (rt.anchoredPosition.y < destroyY)
        {
            Destroy(gameObject);
        }
    }
}