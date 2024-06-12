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
    public float gameTime = 0f;
    private bool isErath = false;
    public bool isFire = true;


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
            
            if (!isErath && gameTime >= 30f)
            {
                Debug.Log("地震だよ");
                shakeHouse.StartShake();
                isErath = true;
                
            }

            if (gameTime >= 35f)
            {
                if (!isFire)
                {

                }
                else if (isFire)
                {
                    FireOpen();
                }
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
