using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ChartLoader
{
    // 예쁘게(indent) 출력하려면 두 번째 인자를 true로
    public static void Save(ChartData chart, string fileName)
    {
        string json = JsonUtility.ToJson(chart, true);
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, json);
        Debug.Log($"Chart saved to: {path}");
    }

    public static ChartData Load(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(path))
        {
            Debug.LogError($"File not found: {path}");
            return null;
        }
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<ChartData>(json);
    }
}