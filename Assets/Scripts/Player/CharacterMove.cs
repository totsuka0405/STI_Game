using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// 一人称視点プレイヤーの移動・視点・姿勢・手持ち表示・入力を統合管理するコントローラ。
/// ・Update：入出力、視点、UI連携、補助状態の管理
/// ・FixedUpdate：物理移動（Rigidbody）
/// ・手持ちアイテムの表示/投下、レイによる注視検知（感度低減）
/// </summary>
public class CharacterMove : MonoBehaviour
{
    /// <summary>シーン内で単一利用を想定した参照用インスタンス。</summary>
    public static CharacterMove instance;

    [Header("移動/視点")]
    [SerializeField] float moveSpeed = 5f;           // 通常移動速度
    [SerializeField] float sprintSpeed = 10f;        // 走り速度
    [SerializeField] float mouseSensitivity = 2f;    // 視点感度
    [SerializeField] Camera playerCamera;            // 視点カメラ

    [Header("手持ち表示")]
    [SerializeField] Transform handTransform;        // 手元の表示位置（子にプレハブをぶら下げる）

    [Header("入力/設定UI")]
    [SerializeField] Slider mouseSensitivitySlider;  // 感度スライダー（双方向連携）
    [SerializeField] GameObject virtualMouse;        // UI操作用の仮想カーソル

    [Header("足音")]
    [SerializeField] AudioClip footstepClip;         // 足音SE
    [SerializeField] AudioSource source;             // 足音出力先

    [Header("しゃがみ")]
    [SerializeField] public float crouchScale = 0.5f;  // しゃがみ時のYスケール
    [SerializeField] public float standScale = 1.0f;   // 立ち時のYスケール
    [SerializeField] public float crouchSpeed = 2.0f;  // スケール補間速度
    private float targetScale;                          // 現在向かうYスケール

    // 手持ち/所持
    private GameObject currentItemInstance; // 手元に表示中のインスタンス
    private ItemBox itemBox;

    // 物理・視点
    private Rigidbody rb;
    private float verticalLookRotation; // ピッチ（上下）積算

    // 進行/ロック
    /// <summary>ゲーム進行中か（外部UI/管理から制御）。</summary>
    public bool isGameStarted { get; set; }
    /// <summary>カーソルをロックするか（外部UIから切り替え可）。</summary>
    public bool isCursolLock { get; set; }

    // 姿勢/移動補助
    bool isSit = false;          // しゃがみ状態
    bool isDontSit = false;      // しゃがみ不可領域内か
    bool isWalkSoundOn = false;  // 足音ループの起動フラグ反転用
    bool isLookItem = false;     // アイテム注視中か（感度低減中）

    float originalMouseSensitivity; // 注視前の感度退避

    // UI連携用：一定時間停止判定
    /// <summary>一定時間（3秒）移動がない状態か。UIの案内表示等に利用。</summary>
    public bool isDontMove = false;
    private float timeSinceLastMove = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetScale = standScale;
        itemBox = ItemBox.instance;
        isCursolLock = true;

        // UI連携（感度スライダーの初期同期とイベント登録）
        if (mouseSensitivitySlider != null)
        {
            mouseSensitivitySlider.value = mouseSensitivity;
            mouseSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityChanged);
        }
    }

    private void Update()
    {
        // 進行中のみ操作・視点・UI制御を有効化
        if (GameManager.instance.IsGameStarted())
        {
            if (isGameStarted)
            {
                // カーソルロック/可視制御（UI操作時は解除）
                if (isCursolLock)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    if (virtualMouse != null)
                    {
                        virtualMouse.transform.position = Vector3.zero;
                        virtualMouse.SetActive(false);
                    }
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    if (virtualMouse != null) virtualMouse.SetActive(true);
                }

                // 入出力と付随処理
                View();                 // 視点
                ItemChange();           // 所持切替
                CheckItemRaycast();     // 注視検知（感度低減）
                if (!isDontSit) Crouch();// 姿勢
                ItemDrop();             // 投下

                // 一定時間無操作検知（UI側の再表示などで使用）
                isDontMove = timeSinceLastMove > 3f;
            }
            else
            {
                // 非進行時はUI操作を可能に
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                if (virtualMouse != null) virtualMouse.SetActive(true);
            }
        }
    }

    private void FixedUpdate()
    {
        // 物理移動は FixedUpdate で行う
        if (isGameStarted)
        {
            Moves();
        }
    }

    /// <summary>
    /// WASD/左スティックでの移動をRigidbodyの速度として適用する。
    /// ・Shiftでダッシュ
    /// ・停止時は角速度をクリア
    /// ・移動/停止に応じて足音ループを制御
    /// ・無操作継続時間を計測
    /// </summary>
    private void Moves()
    {
        float moveHorizontal = 0f;
        float moveVertical = 0f;

        // キーボード
        if (Input.GetKey(KeyCode.A)) moveHorizontal = -1f;
        else if (Input.GetKey(KeyCode.D)) moveHorizontal = 1f;

        if (Input.GetKey(KeyCode.W)) moveVertical = 1f;
        else if (Input.GetKey(KeyCode.S)) moveVertical = -1f;

        // ゲームパッド（デッドゾーン簡易）
        if (Gamepad.current != null)
        {
            if (Mathf.Abs(Gamepad.current.leftStick.x.ReadValue()) > 0.1f)
                moveHorizontal = Gamepad.current.leftStick.x.ReadValue();

            if (Mathf.Abs(Gamepad.current.leftStick.y.ReadValue()) > 0.1f)
                moveVertical = Gamepad.current.leftStick.y.ReadValue();
        }

        // スピード
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        // ローカル前後左右 → ワールド速度
        Vector3 moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
        Vector3 moveVelocity = transform.TransformDirection(moveDirection) * currentSpeed;

        bool isMoving = moveDirection.magnitude > 0;

        // 速度適用/停止カウント
        if (isMoving)
        {
            rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
            timeSinceLastMove = 0f;
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            timeSinceLastMove += Time.fixedDeltaTime;
        }

        // 角速度クリア（停止時の残留回転抑制）
        if (!isMoving)
        {
            rb.angularVelocity = Vector3.zero;
        }

        // 足音ループ制御
        if (!isMoving) MoveSoundOfff();
        else MoveSoundOn();
    }

    /// <summary>移動開始時に足音SEのループを開始する。</summary>
    private void MoveSoundOn()
    {
        if (isWalkSoundOn)
        {
            SoundManager.instance.PlayLoopSE(footstepClip, source);
            isWalkSoundOn = false;
        }
    }

    /// <summary>停止時に足音SEのループを停止する。</summary>
    private void MoveSoundOfff()
    {
        SoundManager.instance.StopLoopSE(source);
        isWalkSoundOn = true;
    }

    /// <summary>UIのスライダー変更で視点感度を反映する。</summary>
    private void OnMouseSensitivityChanged(float value)
    {
        mouseSensitivity = value;
    }

    /// <summary>
    /// 視点操作を適用する（マウス/右スティック）。
    /// ・ヨー：本体回転（Y軸）
    /// ・ピッチ：カメラのローカル回転（X軸）
    /// ・Escでカーソルのロック/解除をトグル
    /// </summary>
    private void View()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (Gamepad.current != null)
        {
            mouseX += Gamepad.current.rightStick.x.ReadValue() * mouseSensitivity;
            mouseY += Gamepad.current.rightStick.y.ReadValue() * mouseSensitivity;
        }

        verticalLookRotation += mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);
        playerCamera.transform.localEulerAngles = new Vector3(-verticalLookRotation, 0f, 0f);
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
        }
    }

    /// <summary>
    /// 所持中アイテムの切り替えを処理する。
    /// ・数字キー/ホイール/ショルダーボタンに対応
    /// ・ItemBox の現在選択を更新
    /// </summary>
    private void ItemChange()
    {
        // 数字キー
        if (Input.GetKeyDown(KeyCode.Alpha1)) ItemBox.instance.SelectItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ItemBox.instance.SelectItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ItemBox.instance.SelectItem(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ItemBox.instance.SelectItem(3);

        // マウスホイール
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            int direction = scroll > 0 ? -1 : 1;
            int newIndex = ItemBox.instance.GetNextIndex(direction);
            ItemBox.instance.SelectItem(newIndex);
        }

        // ゲームパッド
        if (Gamepad.current != null)
        {
            if (Gamepad.current.rightShoulder.wasPressedThisFrame)
            {
                int nextIndex = ItemBox.instance.GetNextIndex(1);
                ItemBox.instance.SelectItem(nextIndex);
            }
            else if (Gamepad.current.leftShoulder.wasPressedThisFrame)
            {
                int prevIndex = ItemBox.instance.GetNextIndex(-1);
                ItemBox.instance.SelectItem(prevIndex);
            }
        }
    }

    /// <summary>
    /// 所持アイテムを捨てる入力を処理する（G / Pad 北ボタン）。
    /// ItemBox に対して削除要求を送る。
    /// </summary>
    private void ItemDrop()
    {
        if (Input.GetKeyDown(KeyCode.G) || (Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame))
        {
            ItemBox.instance.RemoveSelectedItem();
        }
    }

    /// <summary>
    /// 指定アイテムのフィールド用プレハブを手元から生成し、ワールドに放つ。
    /// </summary>
    public void DropItemgenerate(Item selectedItem)
    {
        GameObject dropObj = Instantiate(selectedItem.FieldItemPrefab, handTransform);
        dropObj.transform.localScale = selectedItem.FieldItemPrefab.transform.localScale;
        dropObj.transform.SetParent(null);
    }

    /// <summary>
    /// 指定アイテムの手持ち表示プレハブを手元に生成する（既存は破棄）。
    /// </summary>
    public void DisplaySelectedItemInHand(Item selectedItem)
    {
        if (currentItemInstance != null)
        {
            Destroy(currentItemInstance);
        }

        if (selectedItem != null && selectedItem.HandItemPrefab != null)
        {
            currentItemInstance = Instantiate(selectedItem.HandItemPrefab, handTransform);
            currentItemInstance.transform.localPosition = selectedItem.HandItemPrefab.transform.localPosition;
            currentItemInstance.transform.localRotation = selectedItem.HandItemPrefab.transform.localRotation;
        }
    }

    /// <summary>
    /// しゃがみ/立ちのトグルと、Yスケールの時間補間を行う。
    /// ・LeftCtrl / 左スティック押し込み
    /// </summary>
    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || (Gamepad.current != null && Gamepad.current.leftStickButton.wasPressedThisFrame))
        {
            targetScale = isSit ? standScale : crouchScale;
            isSit = !isSit;
        }

        Vector3 scale = this.transform.localScale;
        scale.y = Mathf.Lerp(scale.y, targetScale, crouchSpeed * Time.deltaTime);
        this.transform.localScale = scale;
    }

    /// <summary>
    /// 画面中央からのレイで「Item」タグを注視したかを検知し、
    /// 注視中のみマウス感度を低減する（視線合わせの操作をしやすくする意図）。
    /// </summary>
    private void CheckItemRaycast()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1.3f))
        {
            if (hit.collider.CompareTag("Item"))
            {
                if (!isLookItem)
                {
                    isLookItem = true;
                    originalMouseSensitivity = mouseSensitivity;
                    mouseSensitivity = originalMouseSensitivity / 20f;
                }
            }
            else
            {
                if (isLookItem)
                {
                    isLookItem = false;
                    mouseSensitivity = originalMouseSensitivity;
                }
            }
        }
        else
        {
            if (isLookItem)
            {
                isLookItem = false;
                mouseSensitivity = originalMouseSensitivity;
            }
        }
    }

    /// <summary>しゃがみ不可エリアに入ったらしゃがみを抑止する。</summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DontSitPos"))
        {
            isDontSit = true;
        }
    }

    /// <summary>しゃがみ不可エリアを出たらしゃがみを許可する。</summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DontSitPos"))
        {
            isDontSit = false;
        }
    }
}
