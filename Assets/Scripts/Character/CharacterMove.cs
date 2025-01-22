using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterMove : MonoBehaviour
{
    public static CharacterMove instance;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] float mouseSensitivity = 2f;
    [SerializeField] Camera playerCamera;
    [SerializeField] Transform handTransform; // アイテムを表示する手の位置
    [SerializeField] Slider mouseSensitivitySlider;
    [SerializeField] GameObject virtualMouse;
    [SerializeField] AudioClip footstepClip;          // 足音のクリップ
    [SerializeField] AudioSource source;

    public float crouchScale = 0.5f; // しゃがんだときのスケール
    public float standScale = 1.0f; // 立ち上がったときのスケール
    public float crouchSpeed = 2.0f; // しゃがみ・立ち上がりのスピード
    private float targetScale; // 目標のスケール

    private GameObject currentItemInstance;
    private ItemBox itemBox;
    private Rigidbody rb;
    private float verticalLookRotation;
    public bool isGameStarted { get; set; }
    public bool isCursolLock { get; set; }
    bool isSit = false;
    bool isDontSit = false;
    bool isWalkSoundOn = false;
    bool isLookItem = false;

    float originalMouseSensitivity;

    public bool isDontMove = false; // 3秒間移動しない状態か
    private float timeSinceLastMove = 0f; // 最後に移動した時間をカウント

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
        isCursolLock = true;

        if (itemBox == null)
        {
            Debug.LogError("ItemBox instance not found.");
        }
        if (mouseSensitivitySlider != null)
        {
            mouseSensitivitySlider.value = mouseSensitivity;
            mouseSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityChanged);
        }
    }

    void Update()
    {
        if (GameManager.instance.IsGameStarted())
        {
            if (isGameStarted)
            {
                if (isCursolLock)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    virtualMouse.transform.position = Vector3.zero;
                    virtualMouse.SetActive(false);
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    virtualMouse.SetActive(true);
                }
                View();
                ItemChange();
                CheckItemRaycast();
                if (!isDontSit)
                {
                    Crouch();
                }
                ItemDrop();

                // 3秒間移動がなかった場合にフラグを更新
                if (timeSinceLastMove > 3f)
                {
                    isDontMove = true;
                }
                else
                {
                    isDontMove = false;
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                virtualMouse.SetActive(true);

            }
            
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
        float moveHorizontal = 0f;
        float moveVertical = 0f;

        if (Input.GetKey(KeyCode.A)) moveHorizontal = -1f;
        else if (Input.GetKey(KeyCode.D)) moveHorizontal = 1f;

        if (Input.GetKey(KeyCode.W)) moveVertical = 1f;
        else if (Input.GetKey(KeyCode.S)) moveVertical = -1f;

        if(Gamepad.current != null)
        {
            if (Mathf.Abs(Gamepad.current.leftStick.x.ReadValue()) > 0.1f)
                moveHorizontal = Gamepad.current.leftStick.x.ReadValue();

            if (Mathf.Abs(Gamepad.current.leftStick.y.ReadValue()) > 0.1f)
                moveVertical = Gamepad.current.leftStick.y.ReadValue();
        }
        

        // スピード設定
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        // 移動方向
        Vector3 moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
        Vector3 moveVelocity = transform.TransformDirection(moveDirection) * currentSpeed;

        bool isMoving = moveDirection.magnitude > 0;

        // プレイヤーの移動状態を反映
        if (isMoving)
        {
            rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
            timeSinceLastMove = 0f; // 移動したのでリセット
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            timeSinceLastMove += Time.fixedDeltaTime; // 動かない時間を加算
        }

        // 回転速度のリセット
        if (!isMoving)
        {
            rb.angularVelocity = Vector3.zero;
        }

        // 移動音をオン/オフ
        if (!isMoving)
        {
            MoveSoundOfff();
        }
        else
        {
            MoveSoundOn();
        }
    }

    void MoveSoundOn()
    {
        if (isWalkSoundOn)
        {
            SoundManager.instance.PlayLoopSE(footstepClip, source);
            isWalkSoundOn = false;
        }
    }

    void MoveSoundOfff()
    {
        SoundManager.instance.StopLoopSE(source);
        isWalkSoundOn = true;
    }

    void OnMouseSensitivityChanged(float value)
    {
        mouseSensitivity = value; // スライダーの値を感度に反映
    }

    void View()
    {
        // プレイヤーの視点操作
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // コントローラーのスティックによる視点操作
        if (Gamepad.current != null)
        {
            mouseX += Gamepad.current.rightStick.x.ReadValue() * mouseSensitivity;
            mouseY += Gamepad.current.rightStick.y.ReadValue() * mouseSensitivity;
        }
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
            int direction = scroll > 0 ? -1 : 1;
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

    void ItemDrop()
    {
        if(Input.GetKeyDown(KeyCode.G) || Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame)
        {
            ItemBox.instance.RemoveSelectedItem();
        }
    }

    public void DropItemgenerate(Item selectedItem)
    {
        GameObject dropObj = Instantiate(selectedItem.fieldItemPrefab,handTransform);
        dropObj.transform.localScale = selectedItem.fieldItemPrefab.transform.localScale;
        dropObj.transform.SetParent(null);
    }

    public void DisplaySelectedItemInHand(Item selectedItem)
    {
        // 既に手元にアイテムがある場合は削除
        if (currentItemInstance != null)
        {
            Destroy(currentItemInstance);
        }

        // 新しいアイテムを手元に表示
        if (selectedItem != null && selectedItem.handItemPrefab != null)
        {
            currentItemInstance = Instantiate(selectedItem.handItemPrefab, handTransform);
            currentItemInstance.transform.localPosition = selectedItem.handItemPrefab.transform.localPosition;
            currentItemInstance.transform.localRotation = selectedItem.handItemPrefab.transform.localRotation;
        }
    }

    void Crouch()
    {
        /// 左コントロールキーが押されたらしゃがみ・立ち状態を切り替える
        if (Input.GetKeyDown(KeyCode.LeftControl) || Gamepad.current != null && Gamepad.current.leftStickButton.wasPressedThisFrame)
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

    void CheckItemRaycast()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)); // 画面中央からレイを飛ばす
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1.3f)) // レイが当たる距離を8fに設定
        {
            if (hit.collider.CompareTag("Item"))
            {
                if (!isLookItem) // アイテムを初めて見たとき
                {
                    isLookItem = true;
                    originalMouseSensitivity = mouseSensitivity; // 現在の感度を元の感度として保存
                    mouseSensitivity = originalMouseSensitivity / 20f; // アイテムを見たときに感度を4分の1に設定
                }
            }
            else
            {
                if (isLookItem) // アイテムから外れた場合
                {
                    isLookItem = false;
                    mouseSensitivity = originalMouseSensitivity; // 元の感度に戻す
                }
            }
        }
        else
        {
            if (isLookItem) // アイテムが視界外になった場合
            {
                isLookItem = false;
                mouseSensitivity = originalMouseSensitivity; // 元の感度に戻す
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DontSitPos"))
        {
           isDontSit = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DontSitPos"))
        {
            isDontSit = false;
        }
    }
}
