using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// BGM/SE の再生・ループ管理・フェードと、AudioMixer 経由の音量制御を担うサウンド中枢。
/// ・SE/BGM 再生（非ループ/ループ）
/// ・ループSEの一括管理（停止・フェード）
/// ・AudioMixer の dB 制御（0→-∞）
/// </summary>
public class SoundManager : MonoBehaviour
{
    /// <summary>シーン内で単一利用を想定した参照用インスタンス。</summary>
    public static SoundManager instance;

    [Header("Mixer & BGM")]
    [SerializeField] private AudioMixer audioMixer; // "SEVolume" / "BGMVolume" を持つことを想定
    [SerializeField] private AudioSource bgmSource; // BGM専用の AudioSource

    [Header("Runtime")]
    [SerializeField] private float currentSEVolume = 0.8f; // SEの現在ボリューム(0-1)
    private readonly List<AudioSource> loopSESources = new List<AudioSource>(); // ループSEの追跡

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // ====== SE（単発/ループ） ======

    /// <summary>指定の AudioSource で SE を単発再生する（3D化は発呼側の AudioSource に従う）。</summary>
    public void PlaySE(AudioClip clip, AudioSource audioSource)
    {
        if (clip == null || audioSource == null)
        {
            Debug.LogError("PlaySE: AudioClip もしくは AudioSource が未設定です。");
            return;
        }

        audioSource.volume = currentSEVolume;
        audioSource.clip = clip;
        audioSource.spatialBlend = 1.0f; // 3D サウンド想定
        audioSource.loop = false;
        audioSource.Play();
    }

    /// <summary>指定の AudioSource で SE をループ再生する。</summary>
    public void PlayLoopSE(AudioClip clip, AudioSource audioSource)
    {
        if (clip == null || audioSource == null)
        {
            Debug.LogError("PlayLoopSE: AudioClip もしくは AudioSource が未設定です。");
            return;
        }

        audioSource.volume = currentSEVolume;
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.spatialBlend = 1.0f;
        audioSource.Play();

        if (!loopSESources.Contains(audioSource))
            loopSESources.Add(audioSource);
    }

    /// <summary>指定のループSEを停止し、追跡リストから除外する。</summary>
    public void StopLoopSE(AudioSource audioSource)
    {
        if (audioSource == null || !loopSESources.Contains(audioSource)) return;

        audioSource.loop = false;
        audioSource.Stop();
        loopSESources.Remove(audioSource);
    }

    // ====== BGM ======

    /// <summary>同一曲の再再生を避けつつ BGM をループ再生する。</summary>
    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource == null || clip == null) return;
        if (bgmSource.clip == clip) return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    // ====== Mixer Volume ======

    /// <summary>SE 音量を 0–1 で設定し、AudioMixer の dB に変換して反映する。</summary>
    public void SetSEVolume(float volume)
    {
        if (audioMixer != null)
        {
            if (volume <= 0f) audioMixer.SetFloat("SEVolume", -80f);
            else audioMixer.SetFloat("SEVolume", Mathf.Log10(Mathf.Clamp01(volume)) * 20f);
        }

        currentSEVolume = Mathf.Clamp01(volume);

        // 既に鳴っているループSEの実音量も追随
        for (int i = loopSESources.Count - 1; i >= 0; i--)
        {
            var src = loopSESources[i];
            if (src == null)
            {
                loopSESources.RemoveAt(i);
                continue;
            }
            src.volume = currentSEVolume;
        }
    }

    /// <summary>BGM 音量を 0–1 で設定し、AudioMixer の dB に変換して反映する。</summary>
    public void SetBGMVolume(float volume)
    {
        if (audioMixer == null) return;

        if (volume <= 0f) audioMixer.SetFloat("BGMVolume", -80f);
        else audioMixer.SetFloat("BGMVolume", Mathf.Log10(Mathf.Clamp01(volume)) * 20f);
    }

    // ====== フェード付き ループSE ======

    /// <summary>ループSEをフェードインで開始する。</summary>
    public void PlayLoopSEWithFadeIn(AudioClip clip, AudioSource audioSource, float fadeInDuration)
    {
        if (clip == null || audioSource == null)
        {
            Debug.LogError("PlayLoopSEWithFadeIn: AudioClip もしくは AudioSource が未設定です。");
            return;
        }

        audioSource.volume = 0f;
        audioSource.clip = clip;
        audioSource.spatialBlend = 1.0f;
        audioSource.loop = true;
        audioSource.Play();

        if (!loopSESources.Contains(audioSource))
            loopSESources.Add(audioSource);

        StartCoroutine(FadeIn(audioSource, Mathf.Max(0f, fadeInDuration), currentSEVolume));
    }

    /// <summary>指定ループSEをフェードアウトし、停止する。</summary>
    public void StopLoopSEWithFadeOut(AudioSource audioSource, float fadeOutDuration)
    {
        if (audioSource == null) return;

        for (int i = loopSESources.Count - 1; i >= 0; i--)
        {
            if (loopSESources[i] == audioSource)
            {
                StartCoroutine(FadeOutAndStop(audioSource, Mathf.Max(0f, fadeOutDuration)));
                loopSESources.RemoveAt(i);
            }
        }
    }

    // ====== コルーチン（フェード実装） ======

    /// <summary>指定 AudioSource を0→targetVolumeへ線形フェードする。</summary>
    private IEnumerator FadeIn(AudioSource audioSource, float duration, float targetVolume)
    {
        float startVolume = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration && audioSource != null)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
            yield return null;
        }

        if (audioSource != null)
            audioSource.volume = targetVolume;
    }

    /// <summary>指定 AudioSource を現在音量→0へ線形フェードし、停止する。</summary>
    private IEnumerator FadeOutAndStop(AudioSource audioSource, float duration)
    {
        if (audioSource == null) yield break;

        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < duration && audioSource != null)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / duration);
            yield return null;
        }

        if (audioSource != null)
        {
            audioSource.volume = 0f;
            audioSource.Stop();
        }
    }
}
