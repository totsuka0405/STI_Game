using UnityEngine;

public class PlayerCrouch: MonoBehaviour
{
    private bool isScaledDown = false;
    private Transform playerTransform;

    void Start()
    {
        // PlayerオブジェクトのTransformコンポーネントを取得
        playerTransform = GameObject.Find("Player").transform;
    }

    void Update()
    {
        // Xキーが押されたとき
        if (Input.GetKeyDown(KeyCode.X))
        {
            ToggleScale();
        }
    }

    void ToggleScale()
    {
        if (isScaledDown)
        {
            // Yスケールを1.0に戻す
            playerTransform.localScale = new Vector3(playerTransform.localScale.x, 1.0f, playerTransform.localScale.z);
        }
        else
        {
            // Yスケールを0.5にする
            playerTransform.localScale = new Vector3(playerTransform.localScale.x, 0.5f, playerTransform.localScale.z);
        }

        // フラグを反転させる
        isScaledDown = !isScaledDown;
    }
}