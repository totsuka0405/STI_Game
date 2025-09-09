using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 手持ちスマホUIの起動・アプリ選択（カーソル移動/決定）・各アプリ画面の開閉を制御する。
/// ・右クリック/Pad East でスマホUIの開閉
/// ・十字/WASD/矢印でアイコン選択（グリッド移動）
/// ・左クリック/Pad South で選択中アプリを起動（電話/地図）
/// </summary>
public class PhoneAppUI : MonoBehaviour
{
    [Header("Root UI")]
    [SerializeField] private GameObject phoneUI;

    [Header("App Panels")]
    [SerializeField] private GameObject appCallPanels;
    [SerializeField] private GameObject appMapPanels;

    [Header("App Icons (Grid Order)")]
    [SerializeField] private GameObject[] apps; // 各アイコンのルート。子に "Frame" を持つ想定

    [Header("SE")]
    [SerializeField] private AudioClip moveClip;  // アイコン移動
    [SerializeField] private AudioClip decideClip; // 決定
    [SerializeField] private AudioSource audioSource;

    [Header("Grid")]
    [SerializeField] private int gridWidth = 3;   // グリッドの列数（アイコンの配置順に依存）
    [SerializeField] private int startIndex = 5;  // 起動時に選択状態にするインデックス

    private int currentSelectIndex = 0;
    private Camera mainCam; // 予備で保持（今回は未使用）

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Start()
    {
        // 初期選択
        currentSelectIndex = Mathf.Clamp(startIndex, 0, (apps != null ? apps.Length - 1 : 0));
        SetFrameActive(currentSelectIndex, true);
        UpdateSelection();
    }

    private void Update()
    {
        // スマホを手に持っていない場合はUIを閉じて終了
        if (GameManager.instance == null || !GameManager.instance.isHandPhone)
        {
            SafeSetActive(appCallPanels, false);
            SafeSetActive(appMapPanels, false);
            SafeSetActive(phoneUI, false);
            return;
        }

        var pad = Gamepad.current;

        // 右クリック or Pad East でスマホの開閉
        bool toggle =
            Input.GetMouseButtonDown(1) ||
            (pad != null && pad.buttonEast.wasPressedThisFrame);

        if (toggle)
        {
            bool nowOpen = phoneUI != null && phoneUI.activeSelf;
            SafeSetActive(phoneUI, !nowOpen);
            if (nowOpen)
            {
                SafeSetActive(appCallPanels, false);
                SafeSetActive(appMapPanels, false);
            }
        }

        // 開いている間のみ入力受付
        if (phoneUI != null && phoneUI.activeSelf)
        {
            UISelectInput();

            bool decide =
                Input.GetMouseButtonDown(0) ||
                (pad != null && pad.buttonSouth.wasPressedThisFrame);

            if (decide) SelectOnApp();
        }
    }

    /// <summary>地図アプリの表示トグル＋閲覧フラグをセット。</summary>
    public void OnMap()
    {
        TogglePanel(appMapPanels);
        if (GameManager.instance != null) GameManager.instance.isMapWatch = true;
    }

    /// <summary>電話アプリの表示トグル＋通話フラグをセット。</summary>
    public void OnCall()
    {
        TogglePanel(appCallPanels);
        if (GameManager.instance != null) GameManager.instance.isCallPhone = true;
    }

    /// <summary>アイコン選択の移動入力（上下左右／D-Pad）。</summary>
    private void UISelectInput()
    {
        if (apps == null || apps.Length == 0 || gridWidth <= 0) return;

        var pad = Gamepad.current;
        bool selectionChanged = false;
        int prevIndex = currentSelectIndex;

        if (Input.GetKeyDown(KeyCode.UpArrow) || (pad != null && pad.dpad.up.wasPressedThisFrame))
        {
            if (currentSelectIndex >= gridWidth)
            {
                currentSelectIndex -= gridWidth;
                selectionChanged = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || (pad != null && pad.dpad.down.wasPressedThisFrame))
        {
            if (currentSelectIndex + gridWidth < apps.Length)
            {
                currentSelectIndex += gridWidth;
                selectionChanged = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || (pad != null && pad.dpad.left.wasPressedThisFrame))
        {
            if (currentSelectIndex % gridWidth > 0)
            {
                currentSelectIndex -= 1;
                selectionChanged = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || (pad != null && pad.dpad.right.wasPressedThisFrame))
        {
            if (currentSelectIndex % gridWidth < gridWidth - 1 && currentSelectIndex + 1 < apps.Length)
            {
                currentSelectIndex += 1;
                selectionChanged = true;
            }
        }

        if (selectionChanged)
        {
            SetFrameActive(prevIndex, false);
            UpdateSelection();
            PlaySE(moveClip);
        }
    }

    /// <summary>選択表示（Frame）の更新。全アイコンのFrameを一旦OFF→選択中のみON。</summary>
    private void UpdateSelection()
    {
        if (apps == null || apps.Length == 0) return;

        // 全ての Frame を OFF
        for (int i = 0; i < apps.Length; i++)
        {
            SetFrameActive(i, false);
        }

        // 現在選択中の Frame を ON
        SetFrameActive(currentSelectIndex, true);
    }

    /// <summary>現在選択中のアプリを起動（インデックスで分岐）。</summary>
    private void SelectOnApp()
    {
        PlaySE(decideClip);

        // 既存仕様：0=Map, 1=Call
        if (currentSelectIndex == 0) OnMap();
        else if (currentSelectIndex == 1) OnCall();
        // それ以外のアイコンは現状未割り当て（将来拡張用）
    }

    private void OnDisable()
    {
        SafeSetActive(appCallPanels, false);
        SafeSetActive(appMapPanels, false);
        SafeSetActive(phoneUI, false);
    }

    // ===== ユーティリティ =====

    private void TogglePanel(GameObject panel)
    {
        if (panel == null) return;
        panel.SetActive(!panel.activeSelf);
    }

    private void SetFrameActive(int appIndex, bool active)
    {
        if (apps == null || appIndex < 0 || appIndex >= apps.Length || apps[appIndex] == null) return;

        Transform frame = apps[appIndex].transform.Find("Frame");
        if (frame != null) frame.gameObject.SetActive(active);
    }

    private void PlaySE(AudioClip clip)
    {
        if (clip == null || SoundManager.instance == null) return;
        SoundManager.instance.PlaySE(clip, audioSource);
    }

    private static void SafeSetActive(GameObject go, bool active)
    {
        if (go != null) go.SetActive(active);
    }
}
