using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{
    public GameObject DestObj; // Inspector で設定可能に
    public GameObject JeneObj; // 生成するプレハブ
    public int stepnumber = 0;

    // GameObjectの生成と破壊
    public void CreateAndDestroy()
    {
        if (JeneObj != null)
        {
            // Y軸回転を保持する
            Quaternion rotation = Quaternion.identity; // デフォルト回転
            if (DestObj != null)
            {
                // DestObjのY軸回転を取得
                rotation = Quaternion.Euler(0, DestObj.transform.eulerAngles.y, 0);
            }

            // JeneObjを生成
            Instantiate(JeneObj, transform.position, rotation);
            Debug.Log("オブジェクトを生成しました");
        }
        else
        {
            Debug.LogError("JeneObj が設定されていません！");
        }

        if (DestObj != null)
        {
            // DestObj を破壊
            Destroy(DestObj);
            DestObj = null; // 参照をクリア
            Debug.Log("オブジェクトを破壊しました");
        }
        else
        {
            Debug.LogWarning("DestObj が設定されていません。");
        }
    }
}
