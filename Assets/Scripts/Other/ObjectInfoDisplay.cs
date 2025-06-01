using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ObjectInfoDisplay : MonoBehaviour
{
    public Camera mainCamera; // メインカメラをインスペクターで設定
    public TextMeshProUGUI infoText; // Canvas 上の Text オブジェクトをインスペクターで設定
    public GameObject BackgroundPanel;  // テキストの背景パネル
    [SerializeField] float rayDistance = 8f;

    // オブジェクトごとの情報を保持するクラス
    void Update()
    {
        ShowObjectInfo();
    }

    void ShowObjectInfo()
    {
        // マウスの位置から Ray を生成
        // カメラの画面中央の座標を取得
        Vector2 centerScreenPosition = new Vector2(Screen.width / 2, Screen.height / 2);

        // 画面中央からレイを発射
        Ray ray = Camera.main.ScreenPointToRay(centerScreenPosition);
        RaycastHit hit;


        // Raycast を実行してオブジェクトにヒットするか確認
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // ヒットしたオブジェクトに ExplainObjectInfo がアタッチされているか確認
            ExplainObjectInfo explainInfo = hit.collider.gameObject.GetComponent<ExplainObjectInfo>();
            if (explainInfo != null)
            {
                if (!explainInfo.isInfoActive) return;

                // ヒットしたオブジェクトの情報を取得して表示
                infoText.text = $"{explainInfo.objectName}\n{explainInfo.additionalInfo}";
                BackgroundPanel.SetActive(true); // パネルを表示
                return;
            }
        }

        // ヒットしなかった場合、情報を消去
        infoText.text = "";
        BackgroundPanel.SetActive(false); // パネルを非表示
    }

}
