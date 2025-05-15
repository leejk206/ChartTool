using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// 채보 데이터를 관리하고 노트를 렌더링하는 매니저 클래스
/// </summary>
public class ChartManager
{
    // 노트 게임오브젝트 참조를 저장하는 컬렉션
    public List<Dictionary<int, GameObject>> Notes;        // 실제 생성된 노트 게임오브젝트 참조
    public List<List<int>> NormalNotes;                   // 일반 노트 데이터
    public List<List<int>> DragNotes;                     // 드래그 노트 데이터
    public List<List<int>> SlideNotes;                    // 슬라이드 노트 데이터
    public List<List<int>> UpFlickNotes;                    // up 플릭 노트 데이터
    public List<List<int>> DownFlickNotes;                    // down 플릭 노트 데이터

    /// <summary>
    /// 매니저 초기화 메서드
    /// </summary>
    public void Init()
    {
        #region NotesForJSON
        // 각 노트 타입별 데이터 리스트 초기화 (7개의 세로 라인)
        NormalNotes = new List<List<int>>();
        for (int i = 0; i < 7; i++) NormalNotes.Add(new List<int>());

        DragNotes = new List<List<int>>();
        for (int i = 0; i < 7; i++) DragNotes.Add(new List<int>());

        SlideNotes = new List<List<int>>();
        for (int i = 0; i < 7; i++) SlideNotes.Add(new List<int>());

        UpFlickNotes = new List<List<int>>();
        for (int i = 0; i < 7; i++) UpFlickNotes.Add(new List<int>());

        DownFlickNotes = new List<List<int>>();
        for (int i = 0; i < 7; i++) DownFlickNotes.Add(new List<int>());
        #endregion

        // 실제 노트 게임오브젝트 참조를 저장할 딕셔너리 초기화
        Notes = new();
        for (int i = 0; i < 7; i++) Notes.Add(new Dictionary<int, GameObject>());
    }

    public void OnUpdate()
    {
        // 업데이트 로직 (현재 미사용)
    }

    /// <summary>
    /// JSON 직렬화를 위한 데이터 래퍼 클래스
    /// </summary>
    [System.Serializable]
    private class NotesDataWrapper
    {
        public List<List<int>> NormalNotes;
        public List<List<int>> DragNotes;
        public List<List<int>> SlideNotes;
        public List<List<int>> UpFlickNotes;
        public List<List<int>> DownFlickNotes;
    }

    /// <summary>
    /// 채보 데이터를 JSON 파일로 저장
    /// </summary>
    public void SaveChartToJson(string filePath)
    {
        // 데이터 래퍼 객체 생성
        NotesDataWrapper wrapper = new NotesDataWrapper
        {
            NormalNotes = NormalNotes,
            DragNotes = DragNotes,
            SlideNotes = SlideNotes,
            UpFlickNotes = UpFlickNotes,
            DownFlickNotes = DownFlickNotes
        };

        // JSON 형식으로 직렬화하여 파일 저장
        string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
        System.IO.File.WriteAllText(filePath, json);

        Debug.Log("채보 저장 완료");
        Debug.Log($"저장 경로 : {filePath}, 파일명 : chart.json");
    }

    /// <summary>
    /// JSON 파일에서 채보 데이터 로드
    /// </summary>
    public void LoadChartFromJson(string filePath)
    {
        // 파일이 없는 경우 새로운 파일 생성
        if (!System.IO.File.Exists(filePath))
        {
            Debug.LogWarning("채보 로드 실패, 새로운 파일 생성: " + filePath);

            // 초기화 후 저장
            Init();
            SaveChartToJson(filePath);
            return;
        }

        // 파일에서 JSON 데이터 읽기
        string json = System.IO.File.ReadAllText(filePath);

        // JSON 데이터 역직렬화
        NotesDataWrapper wrapper = JsonConvert.DeserializeObject<NotesDataWrapper>(json);

        // null 체크 후 데이터 할당
        NormalNotes = wrapper.NormalNotes ?? CreateEmptyNoteList();
        DragNotes = wrapper.DragNotes ?? CreateEmptyNoteList();
        SlideNotes = wrapper.SlideNotes ?? CreateEmptyNoteList();
        UpFlickNotes = wrapper.UpFlickNotes ?? CreateEmptyNoteList();
        DownFlickNotes = wrapper.DownFlickNotes ?? CreateEmptyNoteList();
    }

    /// <summary>
    /// 빈 노트 리스트 생성
    /// </summary>
    private List<List<int>> CreateEmptyNoteList()
    {
        var list = new List<List<int>>();
        for (int i = 0; i < 7; i++) list.Add(new List<int>());
        return list;
    }

    // 라인 렌더러와 컨텐츠 영역 참조
    LineRender lineRender;
    RectTransform contentRect;

    /// <summary>
    /// 에디트 모드에서 노트 데이터를 기반으로 노트 렌더링
    /// </summary>
    public void RenderNotesFromData()
    {
        ClearAllNotesFromScene();

        // 필요한 컴포넌트 참조 가져오기
        lineRender = GameObject.Find("LineRender").GetComponent<LineRender>();
        contentRect = GameObject.Find("Content").GetComponent<RectTransform>();

        // NormalNote 생성
        for (int vIndex = 0; vIndex < Managers.Chart.NormalNotes.Count; vIndex++)
        {
            foreach (int hIndex in Managers.Chart.NormalNotes[vIndex])
            {
                // 노트의 X 좌표 계산 (두 세로선 사이의 중앙)
                float centerX = lineRender.verticalXs.Count > vIndex + 1
                    ? (lineRender.verticalXs[vIndex] + lineRender.verticalXs[vIndex + 1]) / 2f
                    : lineRender.verticalXs[vIndex];

                // 노트의 Y 좌표 계산
                float centerY = hIndex * 25f;

                // 노트 프리팹 생성 및 설정
                GameObject NotePrefab = Resources.Load<GameObject>("Prefabs/Notes/NormalNote");
                GameObject noteInstance = GameObject.Instantiate(NotePrefab, contentRect);

                // RectTransform 설정
                RectTransform rt = noteInstance.GetComponent<RectTransform>();
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchorMin = new Vector2(0.5f, 0f);
                rt.anchorMax = new Vector2(0.5f, 0f);
                rt.anchoredPosition = new Vector2(centerX, centerY);
                rt.sizeDelta = new Vector2(lineRender.verticalXs[1] - lineRender.verticalXs[0], 15f);

                // 생성된 노트 참조 저장
                Managers.Chart.Notes[vIndex][hIndex] = noteInstance;
            }
        }

        // SlideNote 생성
        for (int vIndex = 0; vIndex < Managers.Chart.SlideNotes.Count; vIndex++)
        {
            foreach (int hIndex in Managers.Chart.SlideNotes[vIndex])
            {
                // 노트의 X 좌표 계산 (두 세로선 사이의 중앙)
                float centerX = lineRender.verticalXs.Count > vIndex + 1
                    ? (lineRender.verticalXs[vIndex] + lineRender.verticalXs[vIndex + 1]) / 2f
                    : lineRender.verticalXs[vIndex];

                // 노트의 Y 좌표 계산
                float centerY = hIndex * 25f;

                // 노트 프리팹 생성 및 설정
                GameObject NotePrefab = Resources.Load<GameObject>("Prefabs/Notes/SlideNote");
                GameObject noteInstance = GameObject.Instantiate(NotePrefab, contentRect);

                // RectTransform 설정
                RectTransform rt = noteInstance.GetComponent<RectTransform>();
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchorMin = new Vector2(0.5f, 0f);
                rt.anchorMax = new Vector2(0.5f, 0f);
                rt.anchoredPosition = new Vector2(centerX, centerY);
                rt.sizeDelta = new Vector2(lineRender.verticalXs[1] - lineRender.verticalXs[0], 15f);

                // 생성된 노트 참조 저장
                Managers.Chart.Notes[vIndex][hIndex] = noteInstance;
            }
        }

        // UpFlickNote 생성
        for (int vIndex = 0; vIndex < Managers.Chart.UpFlickNotes.Count; vIndex++)
        {
            foreach (int hIndex in Managers.Chart.UpFlickNotes[vIndex])
            {
                // 노트의 X 좌표 계산 (두 세로선 사이의 중앙)
                float centerX = lineRender.verticalXs.Count > vIndex + 1
                    ? (lineRender.verticalXs[vIndex] + lineRender.verticalXs[vIndex + 1]) / 2f
                    : lineRender.verticalXs[vIndex];

                // 노트의 Y 좌표 계산
                float centerY = hIndex * 25f;

                // 노트 프리팹 생성 및 설정
                GameObject NotePrefab = Resources.Load<GameObject>("Prefabs/Notes/UpFlickNote");
                GameObject noteInstance = GameObject.Instantiate(NotePrefab, contentRect);

                // RectTransform 설정
                RectTransform rt = noteInstance.GetComponent<RectTransform>();
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchorMin = new Vector2(0.5f, 0f);
                rt.anchorMax = new Vector2(0.5f, 0f);
                rt.anchoredPosition = new Vector2(centerX, centerY);
                rt.sizeDelta = new Vector2(lineRender.verticalXs[1] - lineRender.verticalXs[0], 15f);

                // 생성된 노트 참조 저장
                Managers.Chart.Notes[vIndex][hIndex] = noteInstance;
            }
        }

        // DownFlickNote 생성
        for (int vIndex = 0; vIndex < Managers.Chart.DownFlickNotes.Count; vIndex++)
        {
            foreach (int hIndex in Managers.Chart.DownFlickNotes[vIndex])
            {
                // 노트의 X 좌표 계산 (두 세로선 사이의 중앙)
                float centerX = lineRender.verticalXs.Count > vIndex + 1
                    ? (lineRender.verticalXs[vIndex] + lineRender.verticalXs[vIndex + 1]) / 2f
                    : lineRender.verticalXs[vIndex];

                // 노트의 Y 좌표 계산
                float centerY = hIndex * 25f;

                // 노트 프리팹 생성 및 설정
                GameObject NotePrefab = Resources.Load<GameObject>("Prefabs/Notes/DownFlickNote");
                GameObject noteInstance = GameObject.Instantiate(NotePrefab, contentRect);

                // RectTransform 설정
                RectTransform rt = noteInstance.GetComponent<RectTransform>();
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchorMin = new Vector2(0.5f, 0f);
                rt.anchorMax = new Vector2(0.5f, 0f);
                rt.anchoredPosition = new Vector2(centerX, centerY);
                rt.sizeDelta = new Vector2(lineRender.verticalXs[1] - lineRender.verticalXs[0], 15f);

                // 생성된 노트 참조 저장
                Managers.Chart.Notes[vIndex][hIndex] = noteInstance;
            }
        }
    }

    /// <summary>
    /// 플레이 모드에서 노트 데이터를 기반으로 노트 렌더링
    ///// </summary>
    //public void PlayModeRenderNotesFromData(float beatHeight = 25f)
    //{
    //    // 1) 기존 노트 모두 제거
    //    ClearAllNotesFromScene();

    //    // 2) 캔버스 RectTransform 가져오기
    //    var canvasRect = GameObject.Find("RootCanvas")
    //                               .GetComponent<RectTransform>();

    //    // 3) 레이아웃 계산
    //    int numLines = Managers.Chart.NormalNotes.Count;
    //    float totalW = canvasRect.rect.width;
    //    float startX = -totalW / 2f;
    //    float cellWidth = totalW / (numLines - 1) * 0.75f;

    //    // 4) BPM 기반 속도 계산
    //    //    beatHeight*16 → 1박(16틱)당 픽셀
    //    //    (BPM/60)    → 초당 박자 수
    //    var pc = GameObject.Find("PlayController").GetComponent<PlayController>();
    //    float speedY = beatHeight * 16f * (pc.BPM / 60f);

    //    // 5) 노트 생성 및 절대 시간 기반 초기화
    //    for (int v = 0; v < numLines; v++)
    //    {
    //        float centerX = startX + cellWidth * v + cellWidth;
    //        foreach (int h in Managers.Chart.NormalNotes[v])
    //        {
    //            float centerY = h * beatHeight + (beatHeight / 2f);
    //            var prefab = Resources.Load<GameObject>("Prefabs/Notes/NormalNote");
    //            var noteGO = GameObject.Instantiate(prefab, canvasRect);
    //            var rt = noteGO.GetComponent<RectTransform>();
    //            rt.pivot = new Vector2(0.5f, 0.5f);
    //            rt.anchorMin = new Vector2(0.5f, 0f);
    //            rt.anchorMax = new Vector2(0.5f, 0f);
    //            rt.sizeDelta = new Vector2(cellWidth, beatHeight);
    //            rt.anchoredPosition = new Vector2(centerX, centerY);

    //            // NoteMover.Init(spawnY, speedY) 에 BPM 기반 speedY 전달
    //            var mover = noteGO.AddComponent<NoteMover>();
    //            mover.Init(centerY, speedY);

    //            Managers.Chart.Notes[v].Add(h, noteGO);
    //        }
    //    }
    //}

    /// <summary>
    /// 씬에서 모든 노트 제거
    /// </summary>
    public void ClearAllNotesFromScene()
    {
        foreach (var column in Managers.Chart.Notes)
        {
            foreach (var kvp in column)
            {
                GameObject.Destroy(kvp.Value);
            }
            column.Clear();
        }
    }
}
