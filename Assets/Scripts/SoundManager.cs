using System.Collections;
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

    public void StopLoopSE(AudioClip clip)
    {
        // 再生中のAudioSourceをリストから検索
        for (int i = loopSESources.Count - 1; i >= 0; i--)
        {
            AudioSource source = loopSESources[i];
            if (source.clip == clip)
            {
                source.Stop(); // 再生を停止
                Destroy(source.gameObject); // 一時オブジェクトを削除
                loopSESources.RemoveAt(i); // リストから削除
            }
        }
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

    public void PlayLoopSEWithFadeIn(AudioClip clip, Vector3 position, float fadeInDuration)
    {
        GameObject tempAudioSource = new GameObject("TempAudioSource");
        tempAudioSource.transform.position = position;

        AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();
        audioSource.volume = 0f; // 初期音量は0
        audioSource.clip = clip;
        audioSource.spatialBlend = 1.0f; // 3Dサウンドに設定
        audioSource.loop = true;
        audioSource.Play();

        loopSESources.Add(audioSource); // リストに追加

        // フェードインを開始
        StartCoroutine(FadeIn(audioSource, fadeInDuration, currentSEVolume));
    }

    private IEnumerator FadeIn(AudioSource audioSource, float duration, float targetVolume)
    {
        float startVolume = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
            yield return null;
        }

        audioSource.volume = targetVolume; // 最終的にターゲットボリュームを設定
    }

    public void StopLoopSEWithFadeOut(AudioClip clip, float fadeOutDuration)
    {
        // 再生中のAudioSourceをリストから検索
        for (int i = loopSESources.Count - 1; i >= 0; i--)
        {
            AudioSource source = loopSESources[i];
            if (source.clip == clip)
            {
                // フェードアウトを開始
                StartCoroutine(FadeOutAndStop(source, fadeOutDuration));
                loopSESources.RemoveAt(i); // リストから削除
            }
        }
    }

    private IEnumerator FadeOutAndStop(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / duration);
            yield return null;
        }

        audioSource.volume = 0f; // 完全に音を消す
        audioSource.Stop(); // 再生を停止
        Destroy(audioSource.gameObject); // 一時オブジェクトを削除
    }

}
