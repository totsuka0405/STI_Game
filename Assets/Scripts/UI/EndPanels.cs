using UnityEngine;

/// <summary>
/// エンディング用のパネル群を順番に表示する制御クラス。
/// ・Start時に最初のパネルを表示
/// ・OnNext() 呼び出しで次のパネルを表示
/// </summary>
public class EndPanels : MonoBehaviour
{
    [Header("表示するパネル群（順番に表示される）")]
    [SerializeField] private GameObject[] panels;

    private int panelIndex = 0;

    private void Start()
    {
        panelIndex = 0;

        if (panels != null && panels.Length > 0 && panels[0] != null)
        {
            panels[0].SetActive(true);
            panelIndex++;
        }
        else
        {
            Debug.LogWarning("EndPanels にパネルが設定されていません。");
        }
    }

    /// <summary>
    /// 次のパネルを表示する（存在する場合のみ）。
    /// </summary>
    public void OnNext()
    {
        if (panels == null) return;

        if (panelIndex < panels.Length && panels[panelIndex] != null)
        {
            panels[panelIndex].SetActive(true);
            panelIndex++;
        }
    }
}
