using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCounter : MonoBehaviour
{
    private TextMeshProUGUI timerText;

    private void Start()
    {
        if (timerText == null)
        {
            Debug.LogError("TextMeshProUGUI コンポーネントがアタッチされていません。");
            return;
        }

    }

    void Update()
    {
        // GameManagerのgameTimeを使用して時間を表示
        float countdownSeconds = GameManager.instance.gameTime;

        if (countdownSeconds > 0)
        {
            // 時間を TimeSpan に変換して表示
            var span = new TimeSpan(0, 0, Mathf.CeilToInt(countdownSeconds));
            timerText.text = span.ToString(@"mm\:ss");
        }
        else
        {
            // タイマーが0になった場合の処理
            timerText.text = "00:00";
        }
    }
}
