using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの「使用（クリック）」入力を契機に、画面中央レイキャストで
/// ワールド上の対象ごとの挙動（取得/開閉/切替/イベント発火）を仲介する。
/// </summary>
public class ItemUse : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private float rayDistance = 8f;   // 画面中央からの有効距離

    [Header("SE")]
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source;

    /// <summary>押下1回につき1度だけ処理するためのフラグ。</summary>
    private bool inputProcessed = false;

    /// <summary>使用するカメラ（未指定時は MainCamera を自動取得）。</summary>
    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        HandleUseInput();
    }

    /// <summary>
    /// クリック/Pad South の押下→離しをハンドリングし、押下時のみレイ判定を行う。
    /// </summary>
    private void HandleUseInput()
    {
        if (!CharacterMove.instance.isGameStarted) return;

        var pad = Gamepad.current;
        bool pressed = Input.GetMouseButtonDown(0) || (pad != null && pad.buttonSouth.wasPressedThisFrame);
        bool released = Input.GetMouseButtonUp(0) || (pad != null && pad.buttonSouth.wasReleasedThisFrame);

        if (!inputProcessed && pressed)
        {
            // 画面中央からレイ
            Vector2 center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            Ray ray = (mainCam != null ? mainCam : Camera.main).ScreenPointToRay(center);

            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
            {
                HandleRaycastHit(hit);
            }

            inputProcessed = true; // この押下フェーズは処理済み
        }

        // ボタンが離されたら次の押下に備えてリセット
        if (inputProcessed && released)
        {
            inputProcessed = false;
        }
    }

    /// <summary>
    /// レイが当たった対象のタグに応じて、対応する振る舞いを実行する。
    /// </summary>
    private void HandleRaycastHit(RaycastHit hit)
    {
        var go = hit.collider.gameObject;
        var tag = hit.collider.tag;

        if (tag == "Item")
        {
            // 所持可能数に空きがあるときのみ取得
            if (ItemBox.instance != null &&
                ItemBox.instance.HandItemLimit > ItemBox.instance.HandItemCount)
            {
                PickupObj pickupObj = hit.collider.GetComponentInParent<PickupObj>();
                if (pickupObj != null)
                {
                    pickupObj.OnClickObj();
                    if (SoundManager.instance != null) SoundManager.instance.PlaySE(clip, source);
                }
            }
            else
            {
                Debug.Log("これ以上アイテムを持てません");
            }
        }
        else if (tag == "Door")
        {
            OpenCloseAnim door = hit.collider.GetComponentInParent<OpenCloseAnim>();
            if (door != null) door.ToggleDoor();
        }
        else if (tag == "cap")
        {
            if (GameManager.instance.isFire && SoundManager.instance != null)
            {
                SoundManager.instance.PlaySE(clip, source);
            }
            GameManager.instance.isFire = false;

            ExplainObjectInfo explainInfo = go.GetComponent<ExplainObjectInfo>();
            if (explainInfo != null) explainInfo.IsInfoActive = false;

            Debug.Log("もとせんとじたよ");
        }
        else if (tag == "ClearPos")
        {
            GameManager.instance.EndFrag();
            GameManager.instance.isGameEnd = true;
        }
        else if (tag == "memo")
        {
            Memo memo = hit.collider.GetComponent<Memo>();
            if (memo != null)
            {
                GameManager.instance.memo = memo.memonumber;
                Debug.Log("memonumber: " + memo.memonumber);
            }
        }
        else if (tag == "LightSwitch")
        {
            if (!GameManager.instance.isFirstBreakerDown)
            {
                LightOnOff lightSwitch = hit.collider.GetComponent<LightOnOff>();
                if (lightSwitch != null) lightSwitch.OnLight();
            }
        }
        else if (tag == "Breaker")
        {
            Breaker breaker = hit.collider.GetComponent<Breaker>();
            if (breaker != null)
            {
                if (SoundManager.instance != null) SoundManager.instance.PlaySE(clip, source);

                if (!GameManager.instance.isFirstErath)
                {
                    if (GameManager.instance.isBreakerDown)
                    {
                        breaker.SetLightWakeUp();
                        GameManager.instance.isBreakerDown = false;
                    }
                    else
                    {
                        breaker.SetAllObjectsInactive();
                        GameManager.instance.isBreakerDown = true;
                    }
                }
                else
                {
                    GameManager.instance.isBreakerDown = true;
                }
            }
        }
        else if (tag == "Bag")
        {
            Bag bag = hit.collider.GetComponent<Bag>();
            if (bag != null)
            {
                bag.OnBagEvent();
                if (SoundManager.instance != null) SoundManager.instance.PlaySE(clip, source);
            }
        }
        else if (tag == "Step")
        {
            StepUp stepUp = hit.collider.GetComponent<StepUp>();
            if (stepUp != null) stepUp.StepUpPlayer();
        }
        else if (tag == "stepEffect")
        {
            Step step = hit.collider.GetComponent<Step>();
            if (step != null)
            {
                // numberはログなどで利用しているだけのため代入先は保持
                int stepnumber = step.stepnumber;
                step.CreateAndDestroy();
            }
        }
    }
}
