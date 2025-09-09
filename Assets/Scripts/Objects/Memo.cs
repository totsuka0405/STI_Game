using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ゲーム内メモを表すコンポーネント。
/// ・memonumber は GameManager に渡される番号
/// ・スペースキー or ゲームパッドのボタンでリセット
/// </summary>
public class Memo : MonoBehaviour
{
    [Header("このメモの識別番号")]
    public int memonumber = 0;

    private void Update()
    {
        // 入力でメモ番号をリセット
        if (Input.GetKeyDown(KeyCode.Space) ||
            (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame))
        {
            GameManager.instance.memo = 0;
        }
    }
}
