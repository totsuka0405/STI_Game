using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    
    public float mouseSensitivity = 2f;
    public Camera playerCamera;
    
    // AddForce関連
    /*
    public float maxSpeed = 10f; // 一定の速度
    public float stopForce = 10f; // 停止時にかける力
    */

    private Rigidbody rb;
    private float verticalLookRotation;

    private bool isGameStarted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
        if (GameManager.instance.IsGameStarted())
        {
            if (!isGameStarted)
            {
                isGameStarted = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            View();
        }
    }
    void FixedUpdate()
    {
        if (isGameStarted)
        {
            Moves();
        }
    }

    /*
    /// <summary>
    /// AddForceで移動
    /// </summary>
    void Moves()
    {
        // プレイヤーの移動
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        Vector3 moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
        Vector3 moveForce = transform.TransformDirection(moveDirection) * currentSpeed;

        // 入力がある場合は力を加える
        if (moveDirection.magnitude > 0)
        {
            rb.AddForce(moveForce, ForceMode.Acceleration);
        }
        else
        {
            // 入力がない場合は速度を減衰させる
            Vector3 velocity = rb.velocity;
            Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);
            Vector3 stopForceVector = -horizontalVelocity * stopForce;
            rb.AddForce(stopForceVector, ForceMode.Acceleration);

            // 一定の速度を超えたら速度を制限する
            if (horizontalVelocity.magnitude > maxSpeed)
            {
                rb.velocity = new Vector3(
                    Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed),
                    rb.velocity.y,
                    Mathf.Clamp(rb.velocity.z, -maxSpeed, maxSpeed)
                );
            }
        }

        // 入力を行っていない場合、回転の力を0にする
        if (moveHorizontal == 0 && moveVertical == 0)
        {
            rb.angularVelocity = Vector3.zero;
        }
    }
    */

    /// <summary>
    /// velocityによる移動
    /// </summary>
    void Moves()
    {
        // プレイヤーの移動
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        Vector3 moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
        Vector3 moveVelocity = transform.TransformDirection(moveDirection) * currentSpeed;

        if (moveDirection.magnitude > 0)
        {
            rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        // 入力を行っていない場合、回転の力と移動の力を0にする
        if (moveHorizontal == 0 && moveVertical == 0)
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    void View()
    {
        // プレイヤーの視点操作
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

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
