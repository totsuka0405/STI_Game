using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectInfoDisplay : MonoBehaviour
{
    public Camera mainCamera; // メインカメラをインスペクターで設定
    public TextMeshProUGUI infoText; // Canvas 上の Text オブジェクトをインスペクターで設定
    public GameObject BackgroundPanel;  // テキストの背景パネル
    public float range = 5.0f; // 射程距離

    // オブジェクトごとの情報を保持するクラス
    [System.Serializable]
    public class ObjectInfo
    {
        public GameObject gameObject;
        public string objectName;
        public string additionalInfo;
    }

    public ObjectInfo[] objectsInfo; // 複数のオブジェクト情報を保持する配列

    void Update()
    {
        ShowObjectInfo();
    }

    public void ShowObjectInfo()
    {
        // マウスの位置から Ray を生成
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Raycast を実行してオブジェクトにヒットするか確認
        if (Physics.Raycast(ray, out hit, range)) // range を追加して射程を設定
        {
            // ヒットしたオブジェクトの情報を配列から検索
            foreach (ObjectInfo objInfo in objectsInfo)
            {
                if (hit.collider.gameObject == objInfo.gameObject)
                {
                    // ヒットしたオブジェクトの情報を取得して表示
                    infoText.text = $"{objInfo.objectName}\n{objInfo.additionalInfo}";
                    BackgroundPanel.SetActive(true); // パネルを表示
                    return;
                }
            }
        }

        // ヒットしなかった場合、情報を消去
        infoText.text = "";
        BackgroundPanel.SetActive(false); // パネルを非表示
    }
}
