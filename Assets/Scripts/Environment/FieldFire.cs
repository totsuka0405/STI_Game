using UnityEngine;

/// <summary>
/// フィールド上の炎オブジェクトにループSEを再生させる。
/// </summary>
public class FieldFire : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioSource source;

    private bool hasPlayedFireSE = false; // 炎SEを既に再生したかどうか

    private void Update()
    {
        if (GameManager.instance == null || SoundManager.instance == null) return;

        // 初回のみ炎のループSEを再生
        if (GameManager.instance.isFire && !hasPlayedFireSE)
        {
            SoundManager.instance.PlayLoopSE(audioClip, source);
            hasPlayedFireSE = true;
        }
    }
}
