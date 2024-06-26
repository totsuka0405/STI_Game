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
    public float shakeStartTimeFirst = 180f;
    [Header("地震発生時間：2回目")]
    public float shakeStartTimeSeconds = 300f;
    [Header("火災発生時間")]
    public float fireStartTime = 200f;

    [Header("地震の全体時間：1回目")]
    public float earthquaketime_First = 20f;
    [Header("地震の全体時間：2回目")]
    public float earthquaketime_Seconds = 60f;
    [Header("地震の大きさ：1回目")]
    public float earthquakePower_First = 0.5f;
    [Header("地震の大きさ：2回目")]
    public float earthquakePower_Seconds = 1.0f;

    // 災害イベントフラグ
    private bool isFirstErath = false;       // 地震
    private bool isSecondEarth = false;
    public bool isFire = true;          // 火事


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
                isSecondEarth = true;

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

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
