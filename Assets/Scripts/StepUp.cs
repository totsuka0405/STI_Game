using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepUp : MonoBehaviour
{
    public GameObject stepObj; // 移動先のオブジェクト
    public GameObject Player; // インスペクターで設定されたプレイヤーオブジェクト（nullでも構わない）

    public void stepup()
    {
        // シーン内の全てのGameObjectを対象にPlayerタグのオブジェクトを探索
        GameObject playerInScene = GameObject.FindGameObjectWithTag("Player");

        if (stepObj == null || playerInScene == null)
        {
            Debug.LogError("stepObj または シーン内のPlayer が設定されていません！");
            return;
        }

        // stepObj の上面の位置を取得
        Vector3 stepTopPosition = stepObj.transform.position + new Vector3(0, stepObj.transform.localScale.y / 2, 0);

        // Player の大きさを考慮して少し浮かせる
        Vector3 playerAdjustedPosition = stepTopPosition + new Vector3(0, playerInScene.transform.localScale.y / 2 - 1.0f, 0);

        // Player の位置を移動
        playerInScene.transform.position = playerAdjustedPosition;

        Debug.Log("フッ!!!");
    }
}
