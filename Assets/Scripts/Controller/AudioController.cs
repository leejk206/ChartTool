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
                
            }
        }
        else
        {
            Destroy(gameObject);
        }

        if (bgmSource == null)
        {
            bgmSource = GetComponent<AudioSource>();
        }

    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSource == null)
        {
            bgmSource = GetComponent<AudioSource>();
        }

        if (clip == null)
        {
            return;
        }

        if (bgmSource == null)
        {
            return;
        }

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();

    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}