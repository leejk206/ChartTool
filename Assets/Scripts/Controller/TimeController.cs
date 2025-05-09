using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    #region Singleton

    private static TimeController _instance;
    public static TimeController Timer
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = GameObject.Find("[TimeController]");
                if (go == null)
                {
                    go = new GameObject("[TimeController]");
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<TimeController>();
                }
                else
                {
                    _instance = go.GetComponent<TimeController>();
                }
            }
            return _instance;
        }
    }

    #endregion

    private Coroutine _currentCoroutine;

    /// <summary>
    /// 일정 시간 후 작업 실행
    /// </summary>
    public void ExecuteAfterTime(float delay, Action task)
    {
        if (task == null) return;
        CoroutineRunner.Start(ExecuteTaskAfterTime(delay, task));
    }

    private IEnumerator ExecuteTaskAfterTime(float delay, Action task)
    {
        yield return new WaitForSeconds(delay);
        task?.Invoke();
        _currentCoroutine = null;
    }

    public void PauseCurrentCoroutine()
    {
        if (_currentCoroutine != null)
        {
            CoroutineRunner.Stop(_currentCoroutine);
            _currentCoroutine = null;
        }
    }
}


public static class CoroutineRunner
{
    private class RunnerBehaviour : MonoBehaviour { }

    private static RunnerBehaviour _runner;

    public static void Init()
    {
        if (_runner == null)
        {
            GameObject go = GameObject.Find("[CoroutineRunner]");
            if (go == null)
            {
                go = new GameObject("[CoroutineRunner]");
                UnityEngine.Object.DontDestroyOnLoad(go);
                _runner = go.AddComponent<RunnerBehaviour>();
            }
            else
            {
                _runner = go.GetComponent<RunnerBehaviour>();
            }
        }
    }

    public static Coroutine Start(IEnumerator routine)
    {
        Init();
        return _runner.StartCoroutine(routine);
    }

    public static void Stop(Coroutine coroutine)
    {
        if (_runner != null && coroutine != null)
            _runner.StopCoroutine(coroutine);
    }
}