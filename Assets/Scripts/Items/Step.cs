using UnityEngine;

/// <summary>
/// 指定位置に新しいオブジェクトを生成し、既存オブジェクトを破壊する。
/// ・DestObj の Y 回転を継承して生成
/// ・生成／破壊は CreateAndDestroy() を呼ぶだけ
/// </summary>
public class Step : MonoBehaviour
{
    [Header("生成元/生成先の参照")]
    [SerializeField] private GameObject DestObj;  // 破壊対象（フィールド名は維持）
    [SerializeField] private GameObject JeneObj;  // 生成プレハブ（フィールド名は維持）

    [Header("識別用番号（外部参照のため公開名を維持）")]
    public int stepnumber = 0;

    /// <summary>
    /// 新しいオブジェクトを生成し、古いオブジェクトを破壊する。
    /// </summary>
    public void CreateAndDestroy()
    {
        // 生成
        if (JeneObj != null)
        {
            Quaternion rotation = Quaternion.identity;
            if (DestObj != null)
            {
                rotation = Quaternion.Euler(0f, DestObj.transform.eulerAngles.y, 0f);
            }

            Instantiate(JeneObj, transform.position, rotation);
            Debug.Log($"Step: {JeneObj.name} を生成しました。");
        }
        else
        {
            Debug.LogError("Step: JeneObj が設定されていません。");
        }

        // 破壊
        if (DestObj != null)
        {
            Destroy(DestObj);
            DestObj = null; // 参照クリア
            Debug.Log("Step: DestObj を破壊しました。");
        }
        else
        {
            Debug.LogWarning("Step: DestObj が設定されていません。");
        }
    }
}
