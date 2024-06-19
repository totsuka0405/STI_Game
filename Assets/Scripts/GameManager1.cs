using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager1 : MonoBehaviour
{
    public float gameOverTime = 3.0f; // ゲームオーバーまでの時間
    private float elapsedTime = 0.0f;
    private bool gameOver = false;

    // GameOverManagerのインスタンスを保持する
    private GameOverManager gameOverManager;

    void Start()
    {
        // GameOverManagerのインスタンスを取得
        gameOverManager = FindObjectOfType<GameOverManager>();
        if (gameOverManager == null)
        {
            Debug.LogError("GameOverManagerが見つかりません！");
        }
    }

    void Update()
    {
        if (!gameOver)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= gameOverTime)
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        // ゲームオーバーの処理をここに書く
        Debug.Log("Game Over");
        gameOver = true;

        if (gameOverManager != null)
        {
            // GameOverManagerのGameOverメソッドを呼び出す
            gameOverManager.GameOver(0); // 時間切れ
        }

        // 例えばシーンを再読み込みするなどの処理を行う
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
