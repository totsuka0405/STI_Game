using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCount_Object : MonoBehaviour
{
    public TextMeshPro timerText; // Public field to assign in the inspector

    private void Start()
    {
        if (timerText == null)
        {
            Debug.LogError("TextMeshPro コンポーネントがアタッチされていません。");
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
