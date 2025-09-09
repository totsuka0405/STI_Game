using UnityEngine;

/// <summary>
/// 指定されたオブジェクトの上にプレイヤーを移動させる処理。
/// 段差や足場への乗り上げを補助する用途。
/// </summary>
public class StepUp : MonoBehaviour
{
    [Header("移動先（足場などのオブジェクト）")]
    [SerializeField] private GameObject stepObj;

    [Header("プレイヤー（任意指定。未設定時はタグ 'Player' で検索）")]
    [SerializeField] private GameObject player;

    /// <summary>
    /// プレイヤーを stepObj の上に移動させる。
    /// </summary>
    public void StepUpPlayer()
    {
        // 未指定ならシーンから Player タグを検索
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (stepObj == null || player == null)
        {
            Debug.LogError("StepUp: stepObj または Player が設定されていません。");
            return;
        }

        // stepObj の上面位置を算出
        float stepTopY = stepObj.transform.position.y + (stepObj.transform.localScale.y / 2f);
        Vector3 stepTopPosition = new Vector3(
            stepObj.transform.position.x,
            stepTopY,
            stepObj.transform.position.z
        );

        // プレイヤーの大きさを考慮して少し浮かせる
        float playerHalfHeight = player.transform.localScale.y / 2f;
        Vector3 adjustedPosition = stepTopPosition + new Vector3(0, playerHalfHeight + 0.05f, 0);

        // プレイヤーを移動
        player.transform.position = adjustedPosition;

        Debug.Log("StepUp: プレイヤーを段差の上に移動しました。");
    }
}
