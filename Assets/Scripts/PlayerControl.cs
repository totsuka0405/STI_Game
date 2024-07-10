using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public Vector2 rotationSpeed = new Vector2(0.01f, 0.01f);
    public float maxRotationAngle = 90f;
    public float minRotationAngle = -100f;

    public float displayDuration = 0.5f; // 表示する時間
    public float gameOver = 60f;

    public TextMeshProUGUI displayText; // UIのTextコンポーネントを参照するための変数
    public GameObject BackgroundPanel;  // テキストの背景パネル

    private Vector2 lastMousePosition;
    private Vector2 newAngle = Vector2.zero;

    private Coroutine displayCoroutine; // コルーチンを管理するための変数

    private void Start()
    {
        // ゲーム開始から60秒後に視点回転を開始するコルーチンを開始
        //StartCoroutine(RotateViewAfterDelay(gameOver, 0.75f, 90f));
        displayText.text = ""; // テキストをクリアする
        BackgroundPanel.SetActive(false); // パネルを非表示
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            RotateCamera(1);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            RotateCamera(-1);
        }
    }

    void HandleMouseLook()
    {
        if (Input.GetMouseButtonDown(0))
        {
            newAngle = transform.localEulerAngles;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            newAngle.y -= (lastMousePosition.x - Input.mousePosition.x) * rotationSpeed.y;
            newAngle.x -= (Input.mousePosition.y - lastMousePosition.y) * rotationSpeed.x;

            // x軸の回転角度を制限する前に、360度から -180度 ～ 180度 に変換
            if (newAngle.x > 180f) newAngle.x -= 360f;

            // x軸の回転角度を制限
            newAngle.x = Mathf.Clamp(newAngle.x, minRotationAngle, maxRotationAngle);

            // 角度を再度360度に変換
            if (newAngle.x < 0f) newAngle.x += 360f;

            transform.localEulerAngles = newAngle;
            lastMousePosition = Input.mousePosition;
        }
    }

    void RotateCamera(int direction)
    {
        transform.Rotate(0, direction * 1.5f * rotationSpeed.y, 0);
    }

    void OnCollisionEnter(Collision collision)
    {

        Debug.Log("ON");
        CustomText customText = collision.gameObject.GetComponent<CustomText>();
        if (customText != null)
        {
            Debug.Log("ON2");
            displayText.text = "Touched: " + customText.objectName + "\n" + customText.displayText;
            BackgroundPanel.SetActive(true); // パネルを表示
        }
    }

    void OnCollisionExit(Collision collision)
    {
        CustomText customText = collision.gameObject.GetComponent<CustomText>();
        if (customText != null)
        {
            if (displayCoroutine != null)
            {
                StopCoroutine(displayCoroutine); // 既存のコルーチンを停止
            }
            displayCoroutine = StartCoroutine(DisplayTextForDuration(displayDuration, customText.objectName, customText.displayText));
        }
    }

    IEnumerator DisplayTextForDuration(float duration, string objName, string text)
    {
        displayText.text = "Touched: " + objName + "\n" + text;
        BackgroundPanel.SetActive(true); // パネルを表示

        yield return new WaitForSeconds(duration);

        displayText.text = ""; // テキストをクリア
        BackgroundPanel.SetActive(false); // パネルを非表示
    }
}
