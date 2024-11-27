using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    // GameOverManagerのインスタンスを保持する
    private GameOverManager gameOverManager;

    // Start is called before the first frame update
    void Start()
    {
        // GameOverManagerのインスタンスを取得
        gameOverManager = FindObjectOfType<GameOverManager>();
        if (gameOverManager == null)
        {
            Debug.LogError("GameOverManagerが見つかりません！");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーがFireのColliderに触れたとき
        if (other.CompareTag("Player"))
        {
            GameOver();
        }
    }

    void GameOver()
    {
        // ゲームオーバーの処理をここに書く
        Debug.Log("Game Over");
        //gameOver = true;

        if (gameOverManager != null)
        {
            // GameOverManagerのGameOverメソッドを呼び出す
            gameOverManager.GameOver(2); //轢かれた
        }

        // 例えばシーンを再読み込みするなどの処理を行う
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}