using UnityEngine;

/// <summary>
/// プレイヤーが調べた際などに表示される補足情報を保持するコンポーネント。
/// - インスペクターでオブジェクト名と追加情報を設定
/// - 有効フラグによって表示／非表示を制御可能
/// </summary>
public class ExplainObjectInfo : MonoBehaviour
{
    [Header("表示用の名前")]
    [SerializeField] private string objectName;

    [Header("追加説明テキスト")]
    [SerializeField][TextArea] private string additionalInfo;

    [Header("情報が有効かどうか")]
    [SerializeField] private bool isInfoActive = true;

    // 外部から取得専用のプロパティ
    public string ObjectName => objectName;
    public string AdditionalInfo => additionalInfo;
    public bool IsInfoActive
    {
        get => isInfoActive;
        set => isInfoActive = value;
    }
}
