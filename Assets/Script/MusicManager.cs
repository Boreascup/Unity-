using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip backgroundMusic; // 背景音乐
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true; // 设置循环播放
        audioSource.playOnAwake = true; // 游戏启动时播放
        audioSource.volume = 0.5f; // 音量控制
        audioSource.Play(); // 播放背景音乐
    }
}

