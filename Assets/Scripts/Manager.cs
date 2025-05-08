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
            Debug.Log($"������ �����Ƿ� �� ��Ʈ ������ �����մϴ�: {path}");

            // �� ��Ʈ ����
            currentChart = new ChartData
            {
                bpm = 120f, // �⺻ BPM (���ϸ� ���� ����)
                notes = new System.Collections.Generic.List<NoteData>()
            };

            // ���Ϸ� ����
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
