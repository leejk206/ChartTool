using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }
    [SerializeField] private AudioSource bgmSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (bgmSource == null)
            {
                bgmSource = GetComponent<AudioSource>();
                if (bgmSource == null)
                    Debug.LogError("AudioSource가 AudioController 오브젝트에 없습니다!");
                else
                    Debug.Log("AudioSource 자동으로 연결되었습니다.");
            }
        }
        else
        {
            Destroy(gameObject);
        }

        Debug.Log("AudioController Awake 진입");
        if (bgmSource == null)
        {
            Debug.LogWarning("bgmSource가 인스펙터에서 할당되지 않았습니다.");
            bgmSource = GetComponent<AudioSource>();
            Debug.Log("GetComponent<AudioSource>() 결과: " + (bgmSource != null));
        }

        Debug.Log("GetComponent<AudioSource>() 결과: " + (bgmSource != null));
        Debug.Log($"[Awake] AudioController 생성됨: {GetInstanceID()}");
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSource == null)
        {
            Debug.LogWarning("bgmSource가 null이라 GetComponent 시도");
            bgmSource = GetComponent<AudioSource>();
        }

        if (clip == null)
        {
            Debug.LogError("PlayBGM 실패: 전달된 clip이 null입니다.");
            return;
        }

        if (bgmSource == null)
        {
            Debug.LogError("PlayBGM 실패: bgmSource가 null입니다 (GetComponent 실패).");
            return;
        }

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();

        Debug.Log($"[PlayBGM 실행 완료] Clip: {clip.name}, isPlaying: {bgmSource.isPlaying}");
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}