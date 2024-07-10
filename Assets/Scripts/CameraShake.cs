using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // シェイクの強度（数値が大きいほどシェイクが小さくなる）
    public float shakeIntensity = 0.1f;
    // シェイクの持続時間
    public float shakeDuration = 5.0f;

    private Vector3 originalPosition; // カメラの初期位置
    private float shakeTimer = 0f; // シェイクの残り時間

    private bool shakeScheduled = false; // シェイクがスケジュールされているかのフラグ
    public float timeToShake = 120.0f; // 次のシェイクまでの時間（初期値は4秒）

    private Coroutine resetShakeCoroutine;

    // Animatorコンポーネントの取得
    //private Animator animator;

    void Start()
    {
        // 初期位置を記憶
        originalPosition = transform.localPosition;

        // Animatorコンポーネントを取得
        /*animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animatorコンポーネントが見つかりません");
        }*/
    }

    void Update()
    {
        // シェイクがスケジュールされていない場合、カウントダウンを行う
        if (!shakeScheduled)
        {
            timeToShake -= Time.deltaTime;

            // カウントダウンが終了したらシェイクをスケジュール
            if (timeToShake <= 0)
            {
                ScheduleShake();
                //PlayAnimation(); // アニメーションを再生
            }
        }

        // シェイクタイマーが0より大きい場合はシェイクを行う
        if (shakeTimer > 0)
        {
            PerformShake();
            shakeTimer -= Time.deltaTime;
        }
        else if (shakeTimer <= 0 && shakeScheduled)
        {
            // シェイクタイマーが0になったら元の位置に戻す
            shakeTimer = 0f;
            transform.localPosition = originalPosition;
        }
    }

    // シェイクをスケジュールするメソッド
    void ScheduleShake()
    {
        shakeScheduled = true;
        shakeTimer = shakeDuration;
        // シェイク終了後にスケジュールをリセットするコルーチンを開始
        if (resetShakeCoroutine != null)
        {
            StopCoroutine(resetShakeCoroutine);
        }
        resetShakeCoroutine = StartCoroutine(ResetShakeScheduledAfterDuration());
    }

    // シェイクスケジュールをリセットするコルーチン
    IEnumerator ResetShakeScheduledAfterDuration()
    {
        yield return new WaitForSeconds(shakeDuration);
        shakeScheduled = false;
        // 次のシェイクの時間を再設定（例：2分後）
        timeToShake = 600.0f; // 2分後に再度シェイクをスケジュール
    }

    // シェイクの実行
    void PerformShake()
    {
        // ランダムなオフセットを生成
        //Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity;
        Vector3 shakeOffset = new Vector3(Random.Range(-0.5f, 0.5f),
            Random.Range(-0.2f,0.2f), Random.Range(-0.5f, 0.5f))
            * shakeIntensity * Time.deltaTime;
        // シェイクを適用
        transform.localPosition = originalPosition + shakeOffset;
    }

    // 外部からシェイクを開始するためのメソッド
    public void ShakeCamera()
    {
        shakeTimer = shakeDuration;
        if (!shakeScheduled)
        {
            ScheduleShake();
        }
    }

    // アニメーションの再生
    /*void PlayAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("PlayAnimationTrigger"); // トリガー名はアニメーショントリガーに合わせてください
        }
    }*/
}

/*using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // シェイクの強度（数値が大きいほどシェイクが小さくなる）
    public int shakeWeakness = 20;
    // シェイクの持続時間
    public float shakeDuration = 5.0f;

    private Vector3 originalPosition; // カメラの初期位置
    private float shakeTimer = 0f; // シェイクの残り時間

    private bool shakeScheduled = false; // シェイクがスケジュールされているかのフラグ
    public float timeToShake = 120.0f; // 次のシェイクまでの時間（初期値は4秒）

    void Start()
    {
        // 初期位置を記憶
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        // シェイクがスケジュールされていない場合、カウントダウンを行う
        if (!shakeScheduled)
        {
            timeToShake -= Time.deltaTime;

            // カウントダウンが終了したらシェイクをスケジュール
            if (timeToShake <= 0)
            {
                ScheduleShake();
            }
        }

        // シェイクタイマーが0より大きい場合はシェイクを行う
        if (shakeTimer > 0)
        {
            PerformShake();
            shakeTimer -= Time.deltaTime;
        }
        else if (shakeTimer < 0)
        {
            // シェイクタイマーが0になったら元の位置に戻す
            shakeTimer = 0f;
            transform.localPosition = originalPosition;
        }
    }

    // シェイクをスケジュールするメソッド
    void ScheduleShake()
    {
        shakeScheduled = true;
        shakeTimer = shakeDuration;
        // シェイク終了後にスケジュールをリセットするコルーチンを開始
        StartCoroutine(ResetShakeScheduledAfterDuration());
    }

    // シェイクスケジュールをリセットするコルーチン
    IEnumerator ResetShakeScheduledAfterDuration()
    {
        yield return new WaitForSeconds(shakeDuration);
        shakeScheduled = false;
        // 次のシェイクの時間を再設定（例：2分後）
        timeToShake = 600.0f; // 2分後に再度シェイクをスケジュール
    }

    // シェイクの実行
    void PerformShake()
    {
        // ランダムなオフセットを生成
        Vector3 shakeOffset = Random.insideUnitSphere;
        // シェイクを適用
        transform.localPosition = originalPosition + shakeOffset / shakeWeakness;
    }

    // 外部からシェイクを開始するためのメソッド
    public void ShakeCamera()
    {
        shakeTimer = shakeDuration;
    }
}*/

