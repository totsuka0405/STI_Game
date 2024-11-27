using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] float mouseSensitivity = 2f;

    private float verticalLookRotation;

    public void View()
    {
        // プレイヤーの視点操作
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity + Input.GetAxis("Joystick Look Horizontal") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity + Input.GetAxis("Joystick Look Vertical") * mouseSensitivity;

        verticalLookRotation += mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        // X軸とZ軸の回転を固定
        transform.Rotate(Vector3.up * mouseX);
        playerCamera.transform.localEulerAngles = new Vector3(-verticalLookRotation, 0f, 0f);
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);

        // マウスカーソルのロック/解除
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
        }
    }
}
