// NoteMover.cs
using UnityEngine;

public class NoteMover : MonoBehaviour
{
    [Tooltip("�ʴ� �̵��� Y �ȼ� �ӵ�")]
    public float speed;

    RectTransform rt;
    float destroyY = -100f; // �� ������ �������� �ı�

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        // �� ������ �Ʒ��� �̵�
        rt.anchoredPosition += Vector2.down * speed * Time.deltaTime;

        if (rt.anchoredPosition.y < destroyY)
        {
            Destroy(gameObject);
        }
    }
}