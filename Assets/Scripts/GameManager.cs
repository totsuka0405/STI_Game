using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ShakeHouse shakeHouse;

    private bool gameStarted = false;
    private float gameTime = 0f;
    private bool isErath = false;

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

            if (!isErath && gameTime >= 5f)
            {
                Debug.Log("地震だよ");
                shakeHouse.StartShake();
                isErath = true;
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
}
