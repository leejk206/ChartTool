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
                    Debug.LogError("AudioSource�� AudioController ������Ʈ�� �����ϴ�!");
                else
                    Debug.Log("AudioSource �ڵ����� ����Ǿ����ϴ�.");
            }
        }
        else
        {
            Destroy(gameObject);
        }

        Debug.Log("AudioController Awake ����");
        if (bgmSource == null)
        {
            Debug.LogWarning("bgmSource�� �ν����Ϳ��� �Ҵ���� �ʾҽ��ϴ�.");
            bgmSource = GetComponent<AudioSource>();
            Debug.Log("GetComponent<AudioSource>() ���: " + (bgmSource != null));
        }

        Debug.Log("GetComponent<AudioSource>() ���: " + (bgmSource != null));
        Debug.Log($"[Awake] AudioController ������: {GetInstanceID()}");
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSource == null)
        {
            Debug.LogWarning("bgmSource�� null�̶� GetComponent �õ�");
            bgmSource = GetComponent<AudioSource>();
        }

        if (clip == null)
        {
            Debug.LogError("PlayBGM ����: ���޵� clip�� null�Դϴ�.");
            return;
        }

        if (bgmSource == null)
        {
            Debug.LogError("PlayBGM ����: bgmSource�� null�Դϴ� (GetComponent ����).");
            return;
        }

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();

        Debug.Log($"[PlayBGM ���� �Ϸ�] Clip: {clip.name}, isPlaying: {bgmSource.isPlaying}");
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}