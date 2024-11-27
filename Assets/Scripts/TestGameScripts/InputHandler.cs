using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Vector2 GetMovementInput()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        return new (moveHorizontal, moveVertical);
    }

    public bool IsInteractKeyPressed()
    {
        return Input.GetKeyDown(KeyCode.E); // 例: インタラクトキーは"E"
    }
}
