using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

/// <summary>
/// ゲームパッド接続状況とゲーム進行状態に応じて、UI表示（マウスUI/仮想ポインタ）を切り替える。
/// ・毎フレーム Gamepad.current を確認
/// ・ゲーム開始前のみガイドを表示（既存仕様踏襲）
/// </summary>
public class GamePadSetting : MonoBehaviour
{
    [Header("表示対象")]
    [SerializeField] private GameObject virtualObject; // 仮想ポインタ等のオブジェクト
    [SerializeField] private GameObject mouseUI;       // マウス操作ガイドUI

    private void Update()
    {
        CheckGamepadConnection();
    }

    /// <summary>
    /// ゲームパッド接続状況と CharacterMove の状態に基づいて UI を切り替える。
    /// </summary>
    private void CheckGamepadConnection()
    {
        var gamepad = Gamepad.current;

        // CharacterMove の存在確認
        if (CharacterMove.instance == null)
        {
            Debug.LogError("CharacterMove.instance is null. Ensure it's properly initialized.");
            return;
        }

        // 既定は非表示
        bool showMouseUI = false;
        bool showVirtualObject = false;

        // 既存仕様：
        // ・ゲームパッドが接続されている AND まだゲーム開始前 → 表示
        if (gamepad != null && !CharacterMove.instance.isGameStarted)
        {
            showMouseUI = true;
            showVirtualObject = true;
        }

        // Null 安全に SetActive
        if (mouseUI != null) mouseUI.SetActive(showMouseUI);
        if (virtualObject != null) virtualObject.SetActive(showVirtualObject);
    }
}
