using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class ChartManager
{
    public List<Dictionary<int, GameObject>> Notes;
    public List<List<int>> NormalNotes;
    public List<List<int>> DragNotes;
    public List<List<int>> SlideNotes;
    public List<List<int>> FlickNotes;


    public void Init()
    {
        #region NotesForJSON
        NormalNotes = new List<List<int>>();
        // �� ���� JSON���� �Ѱ��� y��ǥ �����͸� ��� �־�� ��.
        for (int i = 0; i < 7; i++) NormalNotes.Add(new List<int>());

        DragNotes = new List<List<int>>();
        for (int i = 0; i < 7; i++) DragNotes.Add(new List<int>());

        SlideNotes = new List<List<int>>();
        for (int i = 0; i < 7; i++) SlideNotes.Add(new List<int>());

        FlickNotes = new List<List<int>>();
        for (int i = 0; i < 7; i++) FlickNotes.Add(new List<int>());
        #endregion

        // Notes : ��Ʈ �� ����Ʈ ���� ���ӿ�����Ʈ�� �����ϴ� ����Ʈ
        Notes = new();
        // �� ���� y��ǥ�� �� ��ǥ ���� ���ӿ�����Ʈ�� ��� �־�� ��.
        for (int i = 0; i < 7; i++) Notes.Add(new Dictionary<int, GameObject>());




    }

    public void OnUpdate()
    {

    }

    [System.Serializable]
    private class NotesDataWrapper
    {
        public List<List<int>> NormalNotes;
        public List<List<int>> DragNotes;
        public List<List<int>> SlideNotes;
        public List<List<int>> FlickNotes;
    }



    public void SaveChartToJson(string filePath)
    {
        NotesDataWrapper wrapper = new NotesDataWrapper
        {
            NormalNotes = NormalNotes,
            DragNotes = DragNotes,
            SlideNotes = SlideNotes,
            FlickNotes = FlickNotes
        };

        // Newtonsoft.Json ���
        string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
        System.IO.File.WriteAllText(filePath, json);

        Debug.Log("���� �Ϸ�");
        Debug.Log($"���� ��� : {filePath}, ���ϸ� : chart.json");
    }

    public void LoadChartFromJson(string filePath)
    {
        if (!System.IO.File.Exists(filePath))
        {
            Debug.LogWarning("���� ����, �� ���� ����: " + filePath);

            // �ʱ�ȭ �� ����
            Init();
            SaveChartToJson(filePath);
            return;
        }

        string json = System.IO.File.ReadAllText(filePath);

        // Newtonsoft.Json ���
        NotesDataWrapper wrapper = JsonConvert.DeserializeObject<NotesDataWrapper>(json);

        // null üũ �� �⺻�� ����
        NormalNotes = wrapper.NormalNotes ?? CreateEmptyNoteList();
        DragNotes = wrapper.DragNotes ?? CreateEmptyNoteList();
        SlideNotes = wrapper.SlideNotes ?? CreateEmptyNoteList();
        FlickNotes = wrapper.FlickNotes ?? CreateEmptyNoteList();


        Debug.Log("�ҷ����� �Ϸ�");
    }

    private List<List<int>> CreateEmptyNoteList()
    {
        var list = new List<List<int>>();
        for (int i = 0; i < 7; i++) list.Add(new List<int>());
        return list;
    }

    LineRender lineRender;
    RectTransform contentRect;

    public void RenderNotesFromData()
    {
        ClearAllNotesFromScene();

        lineRender = GameObject.Find("LineRender").GetComponent<LineRender>();
        contentRect = GameObject.Find("Content").GetComponent<RectTransform>();
        for (int vIndex = 0; vIndex < Managers.Chart.NormalNotes.Count; vIndex++)
        {
            foreach (int hIndex in Managers.Chart.NormalNotes[vIndex])
            {
                float centerX = lineRender.verticalXs.Count > vIndex + 1
                    ? (lineRender.verticalXs[vIndex] + lineRender.verticalXs[vIndex + 1]) / 2f
                    : lineRender.verticalXs[vIndex]; // fallback

                float centerY = hIndex * 25f + 12.5f;

                GameObject NotePrefab = Resources.Load<GameObject>("Prefabs/Notes/NormalNote");
                GameObject noteInstance = GameObject.Instantiate(NotePrefab, contentRect);

                RectTransform rt = noteInstance.GetComponent<RectTransform>();
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchorMin = new Vector2(0.5f, 0f);
                rt.anchorMax = new Vector2(0.5f, 0f);
                rt.anchoredPosition = new Vector2(centerX, centerY);
                rt.sizeDelta = new Vector2(lineRender.verticalXs[1] - lineRender.verticalXs[0], 25f);

                Managers.Chart.Notes[vIndex][hIndex] = noteInstance;
            }
        }

        Debug.Log("��Ʈ ������ �Ϸ�");
    }

    public void PlayModeRenderNotesFromData(float beatHeight = 25f)
    {
        // 1) ���� ���� Note ������Ʈ�� ����
        ClearAllNotesFromScene();

        // 2) Canvas ���� �������� ���
        RectTransform canvasRect = GameObject.Find("RootCanvas")
                                            .GetComponent<RectTransform>();

        int numLines = Managers.Chart.NormalNotes.Count; // ���� 7
        float totalW = canvasRect.rect.width;
        float startX = -totalW / 2f;
        float cellWidth = totalW / (numLines - 1) * 0.75f;
        float screenH = canvasRect.rect.height;

        // 3) ��Ʈ ���� & NoteMover ����
        float speedY = screenH * (GameObject.Find("PlayController").GetComponent<PlayController>().BPM / 60f);

        for (int v = 0; v < numLines; v++)
        {
            float centerX = startX + cellWidth * v + cellWidth;
            foreach (int h in Managers.Chart.NormalNotes[v])
            {
                float centerY = h * beatHeight + (beatHeight / 2f);

                GameObject prefab = Resources.Load<GameObject>("Prefabs/Notes/NormalNote");
                GameObject noteGO = GameObject.Instantiate(prefab, canvasRect);

                // RectTransform ����
                var rt = noteGO.GetComponent<RectTransform>();
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchorMin = new Vector2(0.5f, 0f);
                rt.anchorMax = new Vector2(0.5f, 0f);
                rt.anchoredPosition = new Vector2(centerX, centerY);
                rt.sizeDelta = new Vector2(cellWidth, beatHeight);

                // NoteMover ������Ʈ ���� (������Ʈ���� Y�� �̵�)
                var mover = noteGO.AddComponent<NoteMover>();
                mover.speed = speedY;

                // ��ųʸ��� ����
                Managers.Chart.Notes[v].Add(h, noteGO);
            }
        }

        Debug.Log("Play ���� ��Ʈ ���� �Ϸ�");
    }

    public void ClearAllNotesFromScene()
    {
        foreach (var column in Managers.Chart.Notes)
        {
            foreach (var kvp in column)
            {
                GameObject.Destroy(kvp.Value);
            }
            column.Clear(); // Dictionary ���ε� �ʱ�ȭ
        }
    }
}
