using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class GamePadSetting : MonoBehaviour
{
    public GameObject virtualObject;
    public GameObject mouseUI;

    void Update()
    {
        CheckGamepadConnection();

        /*
        // ゲームパッドが接続されているか確認
        if (Gamepad.current != null)
        {
            // ボタンの状態を確認
            foreach (var control in Gamepad.current.allControls)
            {
                if (control is ButtonControl button && button.isPressed)
                {
                    Debug.Log($"Pressed Button: {control.name}");
                }
            }
        }
        */
    }

    void CheckGamepadConnection()
    {
        var gamepad = Gamepad.current;

        // CharacterMove.instance の null チェック
        if (CharacterMove.instance == null)
        {
            Debug.LogError("CharacterMove.instance is null. Ensure it's properly initialized.");
            return;
        }

        // デフォルト値
        bool showMouseUI = false;
        bool showVirtualObject = false;

        if (gamepad != null)
        {
            if (!CharacterMove.instance.isGameStarted)
            {
                showMouseUI = true;
                showVirtualObject = true;
            }
        }

        // UIオブジェクトの状態を設定
        mouseUI.SetActive(showMouseUI);
        virtualObject.SetActive(showVirtualObject);
    }
}
