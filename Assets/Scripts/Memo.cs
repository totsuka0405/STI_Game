using UnityEngine;
using UnityEngine.InputSystem;

public class Memo : MonoBehaviour
{
    public int memonumber = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            GameManager.instance.memo = 0;
        }
    }
}
