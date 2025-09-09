using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム全体の時間管理・災害イベントの発火・死亡/エンディング判定・
/// シーンロード時の参照再取得とフラグ初期化を担う中枢クラス。
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>グローバルに参照される単一インスタンス。</summary>
    public static GameManager instance { get; private set; }

    // ====== 参照（シーン内オブジェクト） ======
    public ShakeHouse shakeHouse;   // 地震演出の制御
    public FireEvents fireEvents;   // 火災の段階制御
    public GameObject fire;         // 火災の見た目ルート
    [SerializeField] GameObject game;          // ルート（未使用でも参照保持）
    [SerializeField] Transform gameParents;    // ルート親（未使用でも参照保持）
    [SerializeField] Breaker breaker;          // ブレーカー演出

    // ====== 進行管理 ======
    private bool gameStarted = false; // ゲーム内時間を進めるかどうか

    [Header("ゲーム内時間計測")]
    public float gameTime = 0f;       // 起動後からの仮想経過時間（秒）

    // ====== 時間パラメータ（発生時刻/継続/強度） ======
    [Header("地震発生時間：1回目")]
    public float shakeStartTimeFirst = 90f;

    [Header("地震発生時間：2回目")]
    public float shakeStartTimeSeconds = 210f;

    [Header("火災発生時間")]
    public float fireStartTime = 98f;

    [Header("地震の収まる時間（1回目の致死判定の閾値）")]
    public float earthquaketime_FirstDontDie = 125f;

    [Header("地震の全体時間：1回目")]
    public float earthquaketime_First = 20f;

    [Header("地震の全体時間：2回目")]
    public float earthquaketime_Seconds = 60f;

    [Header("地震の大きさ：1回目")]
    public float earthquakePower_First = 0.5f;

    [Header("地震の大きさ：2回目")]
    public float earthquakePower_Seconds = 1.0f;

    [Header("ゲームオーバーの時間")]
    public float gameoverTime = 240f;

    // ====== 状態フラグ（災害・進行・所持・イベント） ======
    // 災害イベント（発火済み/発生中の管理）
    public bool isFirstErath = false; // 1回目地震の発火済み管理（綴りは互換のため維持）
    public bool isSecondEarth = false;
    public bool isFire = true;
    public bool isFirstBreakerDown = false;
    public bool isBreakerDown = false;

    public bool isGameEnd = false;    // エンディング画面表示トリガー

    // 死亡要因フラグ（到達原因の識別）
    public bool isFireDie = false;
    public bool isFirstEarthDontDie = false;
    public bool isFirstEarthDie = false;
    public bool isSecondEarthDie = false;

    // 死亡状態
    public bool isPlayerDead = false;

    // ループ終了（早期終了分岐）
    public bool isLoopEnd = false;

    // エンディング分岐（UI 出し分け用の到達フラグ群）
    public bool isEnd_1 = false;
    public bool isEnd_2 = false;
    public bool isEnd_3 = false;
    public bool isEnd_4 = false;
    public bool isEnd_5 = false;
    public bool isEnd_6 = false;
    public bool isEnd_7 = false;

    // 所持イベントアイテム（達成条件の材料）
    public bool isHaveSlipper = false;
    public bool isHaveWhistle = false;
    public bool isHaveHandLight = false;
    public bool isHaveGloves = false;
    public bool isHaveBottle = false;
    public bool isHaveRadio = false;
    public bool isHaveFirstAidKit = false;
    public bool isHavePhone = false;

    // 手持ち/使用状態
    public bool isHandBousaiBook = false;
    public bool isHandPhone = false;
    public bool isHandItemUse = false;

    // イベント達成（電話/地図の確認）
    public bool isMapWatch = false;
    public bool isCallPhone = false;

    // メモ種別（UI 表示の切り替え基準）
    public int memo = 0;

    // セリフイベント（自己セリフの解放トリガー）
    public bool selfSpleak_1 = false;
    public bool selfSpleak_2 = false;
    public bool selfSpleak_3 = false;
    public bool selfSpleak_4 = false;
    public bool selfSpleak_5 = false;

    // ====== 火災の段階制御 ======
    [Header("火災の段階発火間隔（秒）")]
    [SerializeField] private float fireStepIntervalSeconds = 25f; // 段階ごとの更新間隔

    /// <summary>時間経過に応じて順次有効化する火災インデックス群。</summary>
    private readonly List<List<int>> fireIndexes = new List<List<int>>()
    {
        new List<int> { 0 },
        new List<int> { 1, 2 },
        new List<int> { 3, 4, 5 },
        new List<int> { 6, 7, 8, 9 },
        new List<int> { 10, 11, 12, 13 }
    };

    /// <summary>一度進行させた火災インデックスの記録。</summary>
    private readonly HashSet<int> firedFireIndices = new HashSet<int>();

    /// <summary>処理済みの火災ステップの最大値（連続スキップ時の追いつきに使用）。</summary>
    private int processedFireStepMax = -1;

    private void Awake()
    {
        // シーンをまたいで単一インスタンスを維持する
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // 進行中のみゲーム内時間を加算する
        if (!gameStarted) return;
        gameTime += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        // 進行中のみ時間依存イベントを評価する
        if (!gameStarted) return;

        HandleFirstEarthquakeIfDue();
        HandleFireIfDue();
        HandleBreakerIfDue();
        HandleSecondEarthquakeIfDue();
        HandleDeathsIfDue();
        HandleSelfSpeakByMemo();
        HandleSelfSpeakByTime();
    }

    // ====== 時間依存イベント：ハンドラ群 ======

    /// <summary>所定時刻に1回目の地震を開始する。</summary>
    private void HandleFirstEarthquakeIfDue()
    {
        if (isFirstErath) return;
        if (gameTime < shakeStartTimeFirst) return;

        if (shakeHouse != null)
        {
            Debug.Log("地震だよ（1回目）");
            shakeHouse.AddEarthquakeEvent(earthquaketime_First, earthquakePower_First);
            shakeHouse.StartShake();
        }
        isFirstErath = true;
    }

    /// <summary>
    /// 所定時刻以降に火災を可視化し、段階的な火災進行を評価する。
    /// また、火災開始直後～収束までの致死条件を適用する。
    /// </summary>
    private void HandleFireIfDue()
    {
        if (gameTime < fireStartTime) return;

        if (isFire)
        {
            FireOpen();
            CheckFireUpOnce();
        }

        // 火災開始から一定時間内の致死ウィンドウを評価
        bool deadlyWindow = (gameTime >= fireStartTime + 4f) && (gameTime <= earthquaketime_FirstDontDie);
        if (deadlyWindow && !isFirstEarthDontDie)
        {
            isFirstEarthDie = true;
        }
    }

    /// <summary>火災開始に連動したブレーカー落ちを一度だけ適用する。</summary>
    private void HandleBreakerIfDue()
    {
        if (isFirstBreakerDown) return;
        if (gameTime < fireStartTime) return;

        isFirstBreakerDown = true;
        if (breaker != null)
        {
            breaker.SetAllObjectsInactive();
        }
    }

    /// <summary>所定時刻に2回目の地震を開始する。</summary>
    private void HandleSecondEarthquakeIfDue()
    {
        if (isSecondEarth) return;
        if (gameTime < shakeStartTimeSeconds) return;

        if (shakeHouse != null)
        {
            Debug.Log("地震だよ（2回目）");
            shakeHouse.AddEarthquakeEvent(earthquaketime_Seconds, earthquakePower_Seconds);
            shakeHouse.StartShake();
        }
        isSecondEarth = true;
    }

    /// <summary>時間超過によるゲームオーバー原因（第二地震）を適用する。</summary>
    private void HandleDeathsIfDue()
    {
        if (gameTime >= gameoverTime)
        {
            isSecondEarthDie = true;
        }
    }

    /// <summary>メモ閲覧状況に応じて自己セリフの解放フラグを立てる。</summary>
    private void HandleSelfSpeakByMemo()
    {
        if (memo == 1) selfSpleak_2 = true;
        if (memo == 2) selfSpleak_3 = true;
        if (memo == 3) selfSpleak_4 = true;
    }

    /// <summary>経過時間に応じて自己セリフを解放する（1回目地震の一定時間後）。</summary>
    private void HandleSelfSpeakByTime()
    {
        if (gameTime >= shakeStartTimeFirst + 40f)
        {
            selfSpleak_5 = true;
        }
    }

    /// <summary>火災の見た目を有効化する（未有効時のみ）。</summary>
    private void FireOpen()
    {
        if (fire != null && !fire.activeSelf)
        {
            fire.SetActive(true);
        }
    }

    /// <summary>
    /// 火災の段階を時間経過に合わせて一度ずつ進行させる。
    /// 各インデックスは一回だけ有効化され、フレームを跨いだ追いつきも考慮する。
    /// </summary>
    private void CheckFireUpOnce()
    {
        if (!isFire || fireEvents == null) return;

        float elapsed = gameTime - fireStartTime;
        int currentStep = Mathf.FloorToInt(elapsed / fireStepIntervalSeconds);

        if (currentStep <= processedFireStepMax) return;

        for (int step = processedFireStepMax + 1; step <= currentStep && step < fireIndexes.Count; step++)
        {
            var indices = fireIndexes[step];
            for (int i = 0; i < indices.Count; i++)
            {
                int idx = indices[i];
                if (firedFireIndices.Contains(idx)) continue;
                fireEvents.FireUp(idx);
                firedFireIndices.Add(idx);
            }
            processedFireStepMax = step;
        }
    }

    // ====== 分岐評価（エンディング） ======

    /// <summary>
    /// 現在の状態をもとにエンディング分岐フラグを設定する。
    /// 優先度：早期終了 → 火災残存 → ブレーカー未操作 → 地図未確認 → 連絡未実施 → 所持不備 → スリッパ未所持 → クリア。
    /// </summary>
    public void EndFrag()
    {
        if (gameTime < earthquaketime_FirstDontDie)
        {
            isLoopEnd = true; return;
        }
        if (isFire)
        {
            isEnd_4 = true; return;
        }
        if (!isBreakerDown)
        {
            isEnd_5 = true; return;
        }
        if (!isMapWatch)
        {
            isEnd_6 = true; return;
        }
        if (!isCallPhone)
        {
            isEnd_7 = true; return;
        }
        if (!IsAnyItemHeld())
        {
            isEnd_2 = true; return;
        }
        if (!isHaveSlipper)
        {
            isEnd_3 = true; return;
        }
        isEnd_1 = true;
    }

    /// <summary>クリア判定に必要な所持品が全て揃っているかを返す。</summary>
    bool IsAnyItemHeld()
    {
        return isHaveHandLight && isHaveBottle && isHavePhone &&
               isHaveGloves && isHaveSlipper && isHaveWhistle && isHaveRadio;
    }

    // ====== 外部からの進行制御 ======

    /// <summary>ゲーム内時間の進行を開始する（プレイヤー操作も解放）。</summary>
    public bool IsGameStarted() => gameStarted;

    /// <summary>ゲームを開始する。</summary>
    public void StartGame()
    {
        gameStarted = true;
        if (CharacterMove.instance != null)
        {
            CharacterMove.instance.isGameStarted = true;
        }
    }

    /// <summary>ゲームを一時停止する（時間加算停止）。</summary>
    public void StopGame() => gameStarted = false;

    /// <summary>アプリケーションを終了する。</summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ====== シーン遷移フック ======

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// シーンロード完了時に必要な参照を取得し直し、火災の初期状態と各フラグを初期化する。
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        shakeHouse = FindObjectOfType<ShakeHouse>();
        breaker = FindObjectOfType<Breaker>();
        fireEvents = FindObjectOfType<FireEvents>();

        var fireObj = GameObject.Find("Fire"); // シーン内の Fire ルート
        if (fireObj != null) fire = fireObj;

        if (fireEvents != null) fireEvents.FireInActive();
        if (fire != null) fire.SetActive(false);

        ResetFlags();
    }

    /// <summary>現在のシーンを再読み込みする（最初からやり直し）。</summary>
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// 進行・災害・死亡/終了・所持・イベント・火災段階の内部状態を既定値に戻す。
    /// シーン開始時のクリーンな状態を保証する。
    /// </summary>
    private void ResetFlags()
    {
        // 進行
        gameStarted = false;
        gameTime = 0f;

        // 災害
        isFirstErath = false;
        isSecondEarth = false;
        isFire = true;
        isFirstBreakerDown = false;
        isBreakerDown = false;

        // 死亡/終了
        isFireDie = false;
        isFirstEarthDie = false;
        isFirstEarthDontDie = false;
        isSecondEarthDie = false;
        isPlayerDead = false;
        isLoopEnd = false;
        isGameEnd = false;

        // メモ/セリフ
        memo = 0;
        selfSpleak_1 = false;
        selfSpleak_2 = false;
        selfSpleak_3 = false;
        selfSpleak_4 = false;
        selfSpleak_5 = false;

        // エンディング
        isEnd_1 = false;
        isEnd_2 = false;
        isEnd_3 = false;
        isEnd_4 = false;
        isEnd_5 = false;
        isEnd_6 = false;
        isEnd_7 = false;

        // 所持
        isHaveSlipper = false;
        isHaveWhistle = false;
        isHaveHandLight = false;
        isHaveGloves = false;
        isHaveBottle = false;
        isHaveRadio = false;
        isHaveFirstAidKit = false;
        isHavePhone = false;

        // 手持ち/イベント
        isHandBousaiBook = false;
        isHandPhone = false;
        isHandItemUse = false;
        isMapWatch = false;
        isCallPhone = false;

        // 火災段階
        firedFireIndices.Clear();
        processedFireStepMax = -1;
    }
}
