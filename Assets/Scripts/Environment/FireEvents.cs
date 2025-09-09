using UnityEngine;

/// <summary>
/// 火災オブジェクト群を制御するクラス。
/// </summary>
public class FireEvents : MonoBehaviour
{
    [Header("火災オブジェクト群")]
    [SerializeField] private GameObject[] fires;

    /// <summary>
    /// 指定されたインデックスの火災を有効化する。
    /// </summary>
    public void FireUp(int index)
    {
        if (fires == null || index < 0 || index >= fires.Length)
        {
            Debug.LogWarning($"FireUp: 無効なインデックス {index}");
            return;
        }

        if (fires[index] != null)
        {
            fires[index].SetActive(true);
        }
    }

    /// <summary>
    /// すべての火災を無効化する。
    /// </summary>
    public void FireInActive()
    {
        if (fires == null) return;

        foreach (var fire in fires)
        {
            if (fire != null)
            {
                fire.SetActive(false);
            }
        }
    }
}
