using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public ShakeHouse shakeHouse;
    public GameObject fire;
    public GameObject[] fires;

    private bool gameStarted = false;

    [Header("ゲーム内時間計測")]
    public float gameTime = 0f;
    [Header("地震発生時間：1回目")]
    public float shakeStartTimeFirst = 90f;
    [Header("地震発生時間：2回目")]
    public float shakeStartTimeSeconds = 210f;
    [Header("火災発生時間")]
    public float fireStartTime = 98f;

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

    // 災害イベントフラグ
    private bool isFirstErath = false;       // 地震
    private bool isSecondEarth = false;
    public bool isFire = true;          // 火事

    // 死亡原因フラグ
    public bool isFireDie = false;
    public bool isEarthDie = false;

    // 死亡フラグ
    public bool isPlayerDead = false;

    // クリアフラグ
    public bool isGameClear = false;

    // memoの種類
    public int memo = 0;


    // セリフイベントフラグ
    public bool selfSpleak_1 = false;
    public bool selfSpleak_2 = false;
    public bool selfSpleak_3 = false;
    public bool selfSpleak_4 = false;
    public bool selfSpleak_5 = false;


    // 時間ごとのインデックス範囲を配列で管理
    private readonly List<List<int>> fireIndexes = new List<List<int>>()
    {
        new List<int> { 0 },            // 10秒後
        new List<int> { 1, 2 },         // 20秒後
        new List<int> { 3, 4, 5 },      // 30秒後
        new List<int> { 6, 7, 8, 9 },   // 40秒後
        new List<int> { 10, 11, 12, 13 }// 50秒後
    };

    void Awake()
    {
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
        if (gameStarted)
        {
            gameTime += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (gameStarted)
        {
            if (!isFirstErath && gameTime >= shakeStartTimeFirst)
            {
                Debug.Log("地震だよ");
                shakeHouse.AddEarthquakeEvent(earthquaketime_First, earthquakePower_First);
                shakeHouse.StartShake();
                isFirstErath = true;
            }

            if (gameTime >= fireStartTime)
            {
                if (isFire)
                {
                    FireOpen();
                    CheckFireUp();
                }
            }

            if (!isSecondEarth && gameTime >= shakeStartTimeSeconds)
            {
                Debug.Log("地震だよ");
                shakeHouse.AddEarthquakeEvent(earthquaketime_Seconds, earthquakePower_Seconds);
                shakeHouse.StartShake();
                isSecondEarth = true;

            }

            if(gameTime >= gameoverTime)
            {
                isEarthDie = true;
            }

            if(memo == 1)
            {
                selfSpleak_2 = true;
            }

            if (memo == 2)
            {
                selfSpleak_3 = true;
            }

            if (memo == 3)
            {
                selfSpleak_4 = true;
            }

            if(gameTime >= shakeStartTimeFirst + 40f)
            {
                selfSpleak_5 = true;
            }
        }
    }

    void FireOpen()
    {
        fire.SetActive(true);
    }

    void FireUp(int count)
    {
        fires[count].SetActive(true);
    }

    private void CheckFireUp()
    {
        if (!isFire) return;

        // fireStartTimeからの経過時間を使って、どのインデックス範囲を呼び出すか決める
        for (int i = 0; i < fireIndexes.Count; i++)
        {
            float targetTime = fireStartTime + (i + 1) * 25f;

            if (gameTime >= targetTime)
            {
                // インデックスリストを順番に処理
                foreach (var index in fireIndexes[i])
                {
                    FireUp(index);
                }
            }
        }
    }

    public bool IsGameStarted()
    {
        return gameStarted;
    }

    public void StartGame()
    {
        gameStarted = true;
    }

    public void StopGame()
    {
        gameStarted = false;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ここで参照を再設定
        shakeHouse = FindObjectOfType<ShakeHouse>();
        fire = GameObject.Find("Fire"); // "FireObjectName"は実際のオブジェクト名に置き換えてください
        fire.SetActive(false);
        ResetFlags(); // フラグをリセット
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ResetFlags()
    {
        // フラグのリセット
        gameStarted = false;
        gameTime = 0f;
        isFirstErath = false;
        isSecondEarth = false;
        isFire = true;
        isFireDie = false;
        isEarthDie = false;
        isPlayerDead = false;
        isGameClear = false;
        memo = 0;
        selfSpleak_1 = false;
        selfSpleak_2 = false;
        selfSpleak_3 = false;
        selfSpleak_4 = false;
        selfSpleak_5 = false;
    }
}
