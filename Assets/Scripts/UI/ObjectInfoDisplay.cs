using UnityEngine;
using TMPro;

/// <summary>
/// プレイヤーの視点（画面中央）にあるオブジェクトを調べ、
/// ExplainObjectInfo を持っていれば情報を表示するコンポーネント。
/// </summary>
public class ObjectInfoDisplay : MonoBehaviour
{
    [Header("参照コンポーネント")]
    [SerializeField] private Camera mainCamera;                // 視点用のカメラ
    [SerializeField] private TextMeshProUGUI infoText;         // 情報を表示するテキスト
    [SerializeField] private GameObject backgroundPanel;       // テキストの背景パネル

    [Header("レイの設定")]
    [SerializeField] private float rayDistance = 8f;           // 照射距離

    private void Update()
    {
        ShowObjectInfo();
    }

    /// <summary>
    /// 画面中央から Ray を飛ばし、オブジェクトに情報があれば表示する。
    /// </summary>
    private void ShowObjectInfo()
    {
        if (mainCamera == null || infoText == null || backgroundPanel == null)
        {
            Debug.LogWarning("ObjectInfoDisplay: 参照が正しく設定されていません。");
            return;
        }

        // 画面中央のスクリーン座標を取得して Ray を生成
        Vector2 centerScreenPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = mainCamera.ScreenPointToRay(centerScreenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            // ヒットしたオブジェクトに ExplainObjectInfo があるか確認
            var explainInfo = hit.collider.GetComponent<ExplainObjectInfo>();
            if (explainInfo != null && explainInfo.IsInfoActive)
            {
                // 情報を表示
                infoText.text = $"{explainInfo.ObjectName}\n{explainInfo.AdditionalInfo}";
                backgroundPanel.SetActive(true);
                return;
            }
        }

        // どの対象にもヒットしなかった場合は非表示
        infoText.text = string.Empty;
        backgroundPanel.SetActive(false);
    }
}
