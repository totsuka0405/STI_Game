using UnityEngine;

/// <summary>
/// ブレーカーを操作して家の照明を制御するクラス。
/// ・復旧：ライトをまとめて点灯
/// ・遮断：ライトをまとめて消灯
/// </summary>
public class Breaker : MonoBehaviour
{
    [Header("対象ライト（消灯対象）")]
    [SerializeField] private GameObject[] lightObjs;

    [Header("対象スイッチ（復旧時に操作するオブジェクト）")]
    [SerializeField] private GameObject[] switchObjs;

    [Header("ブレーカー操作音")]
    [SerializeField] private AudioClip switchClip;
    [SerializeField] private AudioSource switchSource;

    /// <summary>
    /// ブレーカー復旧時に呼ばれる処理。
    /// 各スイッチの LightOnOff を通じてライトを点灯させる。
    /// </summary>
    public void SetLightWakeUp()
    {
        SoundManager.instance.PlaySE(switchClip, switchSource);

        foreach (GameObject obj in switchObjs)
        {
            if (obj == null) continue;

            LightOnOff lightOnOff = obj.GetComponentInChildren<LightOnOff>();
            if (lightOnOff != null)
            {
                lightOnOff.OnLightWakeUp();
            }
            else
            {
                Debug.LogWarning($"{obj.name} に LightOnOff コンポーネントが見つかりません。");
            }
        }
    }

    /// <summary>
    /// ブレーカー遮断時に呼ばれる処理。
    /// 登録された全ライトをまとめて消灯する。
    /// </summary>
    public void SetAllObjectsInactive()
    {
        SoundManager.instance.PlaySE(switchClip, switchSource);

        foreach (GameObject obj in lightObjs)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}
