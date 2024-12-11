using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMove : MonoBehaviour
{
    public static CharacterMove instance;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] float mouseSensitivity = 2f;
    [SerializeField] Camera playerCamera;
    [SerializeField] Transform handTransform; // アイテムを表示する手の位置

    [SerializeField] UpObj upObj;

    public float crouchScale = 0.5f; // しゃがんだときのスケール
    public float standScale = 1.0f; // 立ち上がったときのスケール
    public float crouchSpeed = 2.0f; // しゃがみ・立ち上がりのスピード
    private float targetScale; // 目標のスケール

    private GameObject currentItemInstance;
    private ItemBox itemBox;
    private Rigidbody rb;
    private float verticalLookRotation;
    private bool isGameStarted = false;
    bool isSit = false;
    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetScale = standScale; // 初期状態は立っている
        itemBox = ItemBox.instance;
        if(itemBox == null)
        {
            Debug.LogError("ItemBox instance not found.");
        }
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
            ItemChange();
            Crouch();
        }
    }
    void FixedUpdate()
    {
        if (isGameStarted)
        {
            Moves();
           
        }
    }

    /// <summary>
    /// velocityによる移動
    /// </summary>
    void Moves()
    {
        // プレイヤーの移動
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

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

    void ItemChange()
    {
        // キー入力によるアイテム切り替え
        if (Input.GetKeyDown(KeyCode.Alpha1)) ItemBox.instance.SelectItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ItemBox.instance.SelectItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ItemBox.instance.SelectItem(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ItemBox.instance.SelectItem(3);

        // マウスホイールによるアイテム切り替え
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            int direction = scroll > 0 ? 1 : -1;
            int newIndex = ItemBox.instance.GetNextIndex(direction);
            ItemBox.instance.SelectItem(newIndex);
        }

        // ゲームパッドのアイテム切り替え
        if (Gamepad.current != null)
        {
            if (Gamepad.current.rightShoulder.wasPressedThisFrame) // R1ボタンが押された場合
            {
                int nextIndex = ItemBox.instance.GetNextIndex(1);
                ItemBox.instance.SelectItem(nextIndex);
            }
            else if (Gamepad.current.leftShoulder.wasPressedThisFrame) // L1ボタンが押された場合
            {
                int prevIndex = ItemBox.instance.GetNextIndex(-1);
                ItemBox.instance.SelectItem(prevIndex);
            }
        }
    }

    public void DisplaySelectedItemInHand(Item selectedItem)
    {
        // 既に手元にアイテムがある場合は削除
        if (currentItemInstance != null)
        {
            Destroy(currentItemInstance);
        }

        // 新しいアイテムを手元に表示
        if (selectedItem != null && selectedItem.itemPrefab != null)
        {
            currentItemInstance = Instantiate(selectedItem.itemPrefab, handTransform);
            currentItemInstance.transform.localPosition = selectedItem.itemPrefab.transform.localPosition;
            currentItemInstance.transform.localRotation = selectedItem.itemPrefab.transform.localRotation;
        }
    }

    void Crouch()
    {
        if (!upObj.isUpObj)
        {
            /// 左コントロールキーが押されたらしゃがみ・立ち状態を切り替える
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (isSit)
                {
                    targetScale = standScale; // 立ち上がる
                }
                else
                {
                    targetScale = crouchScale; // しゃがむ
                }

                isSit = !isSit; // 状態を切り替える
            }

            // 現在のスケールを目標スケールに向けて補間する
            Vector3 scale = this.transform.localScale;
            scale.y = Mathf.Lerp(scale.y, targetScale, crouchSpeed * Time.deltaTime);
            this.transform.localScale = scale;
        }
        
    }

}
