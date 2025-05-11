using UnityEngine;
using Newtonsoft.Json; // Json.NET ��ġ�� ���
using System.IO;

public class PlayController : MonoBehaviour
{
    [Header("BPM ����")]
    public float BPM = 120f;
    [Header("�� ���ڴ� ���� �ȼ�")]
    public float beatHeight = 25f;
    [Header("��Ʈ JSON ���� �̸�")]
    public string jsonFileName = "chart.json";
    public AudioClip bgmClip;

    bool isPlaying;
    float startTime;

    /// <summary>
    /// ScrollView ���� Canvas ���� ��� ��Ʈ�� �����ϰ�,
    /// ���� NoteMover�� ���� �Ʒ��� ��ũ��(�̵�)��ŵ�ϴ�.
    /// </summary>
    public void PlayModeRenderNotesNoScroll()
    {
        // 1) ���� ���� ��Ʈ�� ���� (�����ʹ� LoadChart ���� ���õ� ����)
        Managers.Chart.ClearAllNotesFromScene();

        // 2) Canvas RectTransform ��������
        RectTransform canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();

        // 3) �� ����, ��ü ��, ���� X, �� �ʺ� ���
        int numLines = Managers.Chart.NormalNotes.Count;   // ���� 7
        float totalW = canvasRect.rect.width;
        float startX = -totalW / 2f;
        float cellWidth = totalW / (numLines - 1);

        // 4) ��Ʈ �̵� �ӵ� ��� (px/sec)
        float speedY = beatHeight * BPM / 60f;

        // 5) JSON ������ ��ȸ�ϸ� ��Ʈ �ν��Ͻ�ȭ & NoteMover ����
        for (int v = 0; v < numLines; v++)
        {
            float centerX = startX + cellWidth * v;
            foreach (int h in Managers.Chart.NormalNotes[v])
            {
                float centerY = h * beatHeight + (beatHeight / 2f);

                // ������ �ε� & �θ�� Canvas
                GameObject prefab = Resources.Load<GameObject>("Prefabs/Notes/NormalNote");
                GameObject noteInstance = Instantiate(prefab, canvasRect);

                // RectTransform ����
                RectTransform rt = noteInstance.GetComponent<RectTransform>();
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchorMin = new Vector2(0.5f, 0f);
                rt.anchorMax = new Vector2(0.5f, 0f);
                rt.anchoredPosition = new Vector2(centerX, centerY);
                rt.sizeDelta = new Vector2(cellWidth, beatHeight);

                // NoteMover ����
                var mover = noteInstance.AddComponent<NoteMover>();
                mover.speed = speedY;

                // ��Ÿ�� ������ ��ųʸ��� ����
                Managers.Chart.Notes[v].Add(h, noteInstance);
            }
        }

        Debug.Log("PlayMode�� ��Ʈ ������ �Ϸ�");
    }

    // ��: Play ���� ��
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

        // 1) �� ������Ʈ�� ����
        Managers.Chart.ClearAllNotesFromScene();

        // 2) JSON ������ �ε� (������ ������ Init() or wrapper�� ä����)
        Managers.Chart.LoadChartFromJson(path);

        Managers.Chart.PlayModeRenderNotesFromData();

        startTime = Time.time;
        isPlaying = true;
    }
}