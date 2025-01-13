using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioMixer audioMixer; // AudioMixerをインスペクターで設定
    [SerializeField] private AudioSource bgmSource; // BGM用のAudioSource
    private float currentSEVolume = 0.5f; // 現在のSEの音量
    private List<AudioSource> loopSESources = new List<AudioSource>(); // ループSE用AudioSourceリスト

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // SEを再生（オブジェクトから鳴らす用）
    public void PlaySE(AudioClip clip, Vector3 position, Transform parent)
    {
        GameObject tempAudioSource = new GameObject("TempAudioSource");
        tempAudioSource.transform.position = position;
        tempAudioSource.transform.SetParent(parent);
        AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();
        audioSource.volume = currentSEVolume; // 現在のSE音量を適用
        audioSource.clip = clip;
        audioSource.spatialBlend = 1.0f; // 3Dサウンドに設定
        audioSource.Play();

        Destroy(tempAudioSource, clip.length); // 再生終了後に削除
    }

    public void PlayLoopSE(AudioClip clip, Vector3 position)
    {
        GameObject tempAudioSource = new GameObject("TempAudioSource");
        tempAudioSource.transform.position = position;

        AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();
        audioSource.volume = currentSEVolume; // 現在のSE音量を適用
        audioSource.clip = clip;
        audioSource.spatialBlend = 1.0f; // 3Dサウンドに設定
        audioSource.loop = true;
        audioSource.Play();

        loopSESources.Add(audioSource); // リストに追加
    }


    // BGMを再生
    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip) return; // 同じ曲は再再生しない
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    // SEの音量設定
    public void SetSEVolume(float volume)
    {
        if (volume <= 0)
        {
            audioMixer.SetFloat("SEVolume", -80f); // 音量を最低値に設定（0の場合）
        }
        else
        {
            audioMixer.SetFloat("SEVolume", Mathf.Log10(volume) * 20); // AudioMixer用
        }
        currentSEVolume = volume;

        // ループSEの音量を更新
        foreach (var source in loopSESources)
        {
            if (source != null)
            {
                source.volume = volume;
            }
        }
    }

    // BGMの音量設定
    public void SetBGMVolume(float volume)
    {
        if (volume <= 0)
        {
            audioMixer.SetFloat("BGMVolume", -80f); // 音量を最低値に設定（0の場合）
        }
        else
        {
            audioMixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 20); // AudioMixer用
        }
    }
}
