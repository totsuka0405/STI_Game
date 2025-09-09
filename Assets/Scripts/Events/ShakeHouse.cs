using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 建物（Rigidbody）に対して地震の揺れを力として与える演出コントローラ。
/// 事前に登録された複数の地震イベント（継続時間・最大震度）を順次再生する。
/// </summary>
public class ShakeHouse : MonoBehaviour
{
    /// <summary>
    /// 単一の地震イベント（継続時間と最大震度）。
    /// </summary>
    public class EarthquakeEvent
    {
        public float duration;
        public float maxMagnitude;

        public EarthquakeEvent(float duration, float maxMagnitude)
        {
            this.duration = duration;
            this.maxMagnitude = maxMagnitude;
        }
    }

    [Header("揺れプロファイル")]
    [SerializeField] private float dampingSpeed = 1.0f; // 時間減衰（経過に応じた持続時間の消費速度）
    [SerializeField] private float shakeFrequency = 20f;   // 震動の角周波数（Sin/Cos の時間係数）
    [SerializeField] private float shakeAmplitude = 3.0f;  // 震動の基本振幅（最終は震度でスケール）

    [Header("サウンド")]
    [SerializeField] private AudioClip shakeSound;  // 地震ループSE
    [SerializeField] private AudioSource shakeSource;

    // 実体制御
    private Rigidbody rb;

    // 再生状態
    private bool isShaking = false;      // 現在再生中か
    private float shakeTimeRemaining;      // 現在イベントの残り時間
    private float currentMaxShakeMagnitude;// 現在イベントの最大震度
    private int currentEventIndex = 0;   // 再生中イベントのインデックス

    // 登録済み地震イベント
    private readonly List<EarthquakeEvent> earthquakeEvents = new List<EarthquakeEvent>();

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }
    }

    private void Update()
    {
        if (isShaking && shakeTimeRemaining > 0f)
        {
            ApplyShakeForce(); // 揺れの力を与える（時間経過に応じた震度カーブ）
        }
        else if (isShaking && shakeTimeRemaining <= 0f)
        {
            AdvanceToNextEvent(); // 現イベント終了→停止→次があれば開始
        }
    }

    /// <summary>
    /// 地震イベントを追加登録する（再生順は追加順）。
    /// </summary>
    public void AddEarthquakeEvent(float duration, float maxMagnitude)
    {
        earthquakeEvents.Add(new EarthquakeEvent(duration, maxMagnitude));
    }

    /// <summary>
    /// まだ再生していないイベントがあれば、そのイベントの再生を開始する。
    /// </summary>
    public void StartShake()
    {
        if (currentEventIndex < earthquakeEvents.Count)
        {
            isShaking = true;
            shakeTimeRemaining = earthquakeEvents[currentEventIndex].duration;
            currentMaxShakeMagnitude = earthquakeEvents[currentEventIndex].maxMagnitude;

            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlayLoopSEWithFadeIn(shakeSound, shakeSource, 8);
            }
        }
    }

    /// <summary>
    /// 現在の揺れを停止し、速度/回転速度をリセットする。
    /// </summary>
    private void StopShake()
    {
        isShaking = false;
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    /// <summary>
    /// 時間経過に応じた震度カーブを評価し、Rigidbody にインパルスとして力を加える。
    /// ・前1/3：0→最大へ線形増加
    /// ・中1/3：最大
    /// ・後1/3：最大→0へ線形減少（この期間にサウンドのフェードアウトを要求）
    /// </summary>
    private void ApplyShakeForce()
    {
        if (rb == null || currentEventIndex >= earthquakeEvents.Count) return;

        var current = earthquakeEvents[currentEventIndex];
        float total = current.duration;
        float elapsed = total - shakeTimeRemaining;

        float third = total / 3f;
        float shakeMagnitude;

        if (elapsed <= third)
        {
            // 立ち上がり
            shakeMagnitude = Mathf.Lerp(0f, currentMaxShakeMagnitude, elapsed / third);
        }
        else if (elapsed <= 2f * third)
        {
            // 最大維持
            shakeMagnitude = currentMaxShakeMagnitude;
        }
        else
        {
            // 収束（サウンドはフェードアウト要求）
            shakeMagnitude = Mathf.Lerp(currentMaxShakeMagnitude, 0f, (elapsed - 2f * third) / third);
            if (SoundManager.instance != null)
            {
                SoundManager.instance.StopLoopSEWithFadeOut(shakeSource, 8);
            }
        }

        // サイン/コサインの合成で横揺れベクトルを生成
        float shakeOffsetX = Mathf.Sin(Time.time * shakeFrequency * 1.0f) * shakeMagnitude * shakeAmplitude;
        float shakeOffsetZ = Mathf.Cos(Time.time * shakeFrequency * 0.5f) * shakeMagnitude * shakeAmplitude;
        Vector3 shakeForce = new Vector3(shakeOffsetX, 0f, shakeOffsetZ);

        rb.AddForce(shakeForce * Time.deltaTime, ForceMode.Impulse);

        // 残り時間の減衰
        shakeTimeRemaining -= Time.deltaTime * dampingSpeed;
    }

    /// <summary>
    /// 現イベントの終了処理と、次イベントへの遷移（存在する場合）を行う。
    /// </summary>
    private void AdvanceToNextEvent()
    {
        StopShake();

        currentEventIndex++;
        if (currentEventIndex < earthquakeEvents.Count)
        {
            StartShake();
        }
    }
}
