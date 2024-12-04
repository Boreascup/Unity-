using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip backgroundMusic; // ��������
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true; // ����ѭ������
        audioSource.playOnAwake = true; // ��Ϸ����ʱ����
        audioSource.volume = 0.5f; // ��������
        audioSource.Play(); // ���ű�������
    }
}

