using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }


    ChartManager _chart = new ChartManager();
    InputManager _input = new InputManager();

    public static ChartManager Chart { get { return Instance._chart; } }
    public static InputManager Input { get { return Instance._input; } }

    private ChartData currentChart;
    public string fileName = "MyChart.json";
    void Start()
    {
        Init();
    }

    static void Init()
    {
        if (s_instance != null)
            return;

        GameObject go = GameObject.Find("Managers");
        if (go == null)
        {
            // 프리팹에서 로드해서 인스턴스화
            go = Resources.Load<GameObject>("Prefabs/Managers");
            go = Instantiate(go);
            go.name = "Managers";

            // 씬 전환 시 파괴되지 않도록
        }
        // s_instance 세팅
        s_instance = go.GetComponent<Managers>();

        s_instance._chart.Init();

        DontDestroyOnLoad(go);
    }


    void Update()
    {
        s_instance._chart.OnUpdate();
        s_instance._input.OnUpdate();

    }

    #region UI
    public void LoadEditMode()
    {
        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnEditModeLoaded;

        // EditMode 씬 로드
        SceneManager.LoadScene("EditMode");
    }

    // 씬 로딩 완료 시 호출됨
    private void OnEditModeLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "EditMode")
        {
            SceneManager.sceneLoaded -= OnEditModeLoaded;

            // 한 프레임 대기 후 초기화 (오브젝트 완전히 로드된 뒤)
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            StartCoroutine(DeferredEditModeInit());
        }
    }

    // 초기화 코루틴 – 한 프레임 기다려야 오브젝트가 확실히 살아 있음
    private IEnumerator DeferredEditModeInit()
    {
        yield return null;

        InitFunc init = new();
        init.EditModeInit();
    }

    public void LoadPlayMode()
    {
        SceneManager.sceneLoaded += OnPlayModeLoaded;
        SceneManager.LoadScene("PlayMode");
    }

    private void OnPlayModeLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "PlayMode")
        {
            SceneManager.sceneLoaded -= OnPlayModeLoaded;

            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            StartCoroutine(DeferredPlayModeInit());
        }
    }

    private IEnumerator DeferredPlayModeInit()
    {
        yield return null;

        InitFunc init = new();
        init.PlayModeInit();
    }

    public void LoadChart()
    {
        Chart.LoadChartFromJson(Path.Combine(Application.persistentDataPath, "chart.json"));
        Chart.RenderNotesFromData();
        
    }

    public void SaveChart()
    {
        Chart.SaveChartToJson(Path.Combine(Application.persistentDataPath, "chart.json"));
    }
    #endregion

}
