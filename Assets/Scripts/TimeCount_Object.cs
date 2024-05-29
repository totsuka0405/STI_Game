using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCount_Object : MonoBehaviour
{
    private TextMeshPro timerText;
    public int countdownMinutes = 10;
    private float countdownSeconds;

    private void Start()
    {
        // "Time"という名前のゲームオブジェクトを探し、TextMeshProUGUI コンポーネントを取得
        timerText = GameObject.Find("Time_Object")?.GetComponent<TextMeshPro>();

        if (timerText == null)
        {
            Debug.LogError("TextMeshProUGUI コンポーネントが見つかりませんでした。");
        }

        // カウントダウンの初期秒数を設定
        countdownSeconds = countdownMinutes * 60;
    }

    void Update()
    {
        if (countdownSeconds > 0)
        {
            // タイマーを更新
            countdownSeconds -= Time.deltaTime;

            // 時間を TimeSpan に変換して表示
            var span = new TimeSpan(0, 0, Mathf.CeilToInt(countdownSeconds));
            timerText.text = span.ToString(@"mm\:ss");
        }
        else
        {
            // タイマーが0になった場合の処理
            countdownSeconds = 0;
            timerText.text = "00:00";
            // 0秒になったときの追加処理をここに書く
        }
    }
}
