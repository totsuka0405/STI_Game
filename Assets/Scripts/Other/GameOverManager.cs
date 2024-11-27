using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;    //文字の参照
    public GameObject gameOverPanel;             //パネルの参照
    public Button restartButton;            // 再開ボタンの参照
    private bool isGameOver = false;

    // ゲームオーバー時のメッセージとヒントを設定
    [SerializeField]
    private string[] causesOfDeath;         // ゲームオーバー時のメッセージ
    [SerializeField]
    private string[] hints;                 // ゲームオーバー時のヒント

    /*private string[] causesOfDeath = {
        "炎に包まれた",
        "準備中",
        "時間切れです。"
    };
    private string[] hints = {
        "火元を取り除こう。",
        "準備中",
        "もっと早く脱出を目指しましょう。"
    };*/

    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false); // ボタンを非表示にする
        restartButton.onClick.AddListener(ResetGame); // ボタンにリスナーを追加
    }

    public void GameOver(int causeIndex)
    {
        if (isGameOver) return;

        isGameOver = true;
        gameOverText.text = $"{causesOfDeath[causeIndex]}\nヒント:\n{hints[causeIndex]}";
        gameOverText.gameObject.SetActive(true);
        gameOverPanel.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        Time.timeScale = 0; // ゲームを一時停止
    }

    // ゲームオーバーの状態をリセットする関数
    public void ResetGame()
    {
        isGameOver = false;
        gameOverText.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        Time.timeScale = 1; // ゲームを再開
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
