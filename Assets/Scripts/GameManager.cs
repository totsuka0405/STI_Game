using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public ShakeHouse shakeHouse;
    public GameObject fire;

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
    public bool _1Talk = false;
    public bool _2Talk = false;
    public bool _3Talk = false;
    public bool _4Talk = false;
    public bool _5Talk = false;

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

    private void FixedUpdate()
    {
        if (gameStarted)
        {
            gameTime += Time.deltaTime;
            
            if (!isFirstErath && gameTime >= shakeStartTimeFirst)
            {
                Debug.Log("地震だよ");
                shakeHouse.AddEarthquakeEvent(earthquaketime_First, earthquakePower_First);
                shakeHouse.StartShake();
                isFirstErath = true;
                
            }

            if (gameTime >= fireStartTime)
            {
                if (!isFire)
                {

                }
                else if (isFire)
                {
                    FireOpen();
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
                _2Talk = true;
            }

            if (memo == 2)
            {
                _3Talk = true;
            }

            if (memo == 3)
            {
                _4Talk = true;
            }

            if(gameTime >= shakeStartTimeFirst + 40f)
            {
                _5Talk = true;
            }
        }
    }

    void FireOpen()
    {
        fire.SetActive(true);
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
        _1Talk = false;
        _2Talk = false;
        _3Talk = false;
        _4Talk = false;
        _5Talk = false;
    }
}
