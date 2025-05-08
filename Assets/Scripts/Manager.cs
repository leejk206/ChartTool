using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    private ChartData currentChart;
    public string fileName = "MyChart.json";
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    #region UI
    public void LoadEditMode()
    {
        SceneManager.LoadScene("EditMode");
    }

    public void LoadPlayMode()
    {
        SceneManager.LoadScene("PlayMode");
    }

    public void LoadChart()
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(path))
        {
            Debug.Log($"파일이 없으므로 새 차트 파일을 생성합니다: {path}");

            // 빈 차트 생성
            currentChart = new ChartData
            {
                bpm = 120f, // 기본 BPM (원하면 변경 가능)
                notes = new System.Collections.Generic.List<NoteData>()
            };

            // 파일로 저장
            ChartLoader.Save(currentChart, fileName);
        }
        else
        {
            currentChart = ChartLoader.Load(fileName);
        }
    }

    public void SaveChart()
    {
        ChartLoader.Save(currentChart, "MyChart.json");
    }
    #endregion
}
