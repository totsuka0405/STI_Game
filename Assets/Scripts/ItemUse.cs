using UnityEngine;
using UnityEngine.InputSystem;

public class ItemUse : MonoBehaviour
{
    [SerializeField] float rayDistance = 8f;
    [SerializeField] AudioClip clip;
    [SerializeField] AudioSource source;

    private bool inputProcessed = false; // 入力処理済みフラグ

    void Update()
    {
        HadnUseFunctions();
    }

    void HadnUseFunctions()
    {
        if (CharacterMove.instance.isGameStarted)
        {
            // マウスまたはコントローラーでクリックが押された場合、かつ未処理の場合のみ処理
            if (!inputProcessed && (Input.GetMouseButtonDown(0) || (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)))
            {
                // カメラの画面中央の座標を取得
                Vector2 centerScreenPosition = new Vector2(Screen.width / 2, Screen.height / 2);

                // 画面中央からレイを発射
                Ray ray = Camera.main.ScreenPointToRay(centerScreenPosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, rayDistance))
                {
                    HandleRaycastHit(hit);
                }

                inputProcessed = true; // 入力を処理済みに設定
            }

            // ボタンが離されたときにフラグをリセット
            if (inputProcessed && (Input.GetMouseButtonUp(0) || (Gamepad.current != null && Gamepad.current.buttonSouth.wasReleasedThisFrame)))
            {
                inputProcessed = false; // 入力を再度処理可能に設定
            }
        }
    }


    void HandleRaycastHit(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Item"))
        {
            if (ItemBox.instance.handItemLimit > ItemBox.instance.handItemCount)
            {
                PickupObj pickupObj = hit.collider.GetComponentInParent<PickupObj>();
                if (pickupObj != null)
                {
                    pickupObj.OnClickObj();
                    SoundManager.instance.PlaySE(clip, source);
                    Debug.Log("なったあああ");
                }
            }
            else
            {
                Debug.Log("これ以上アイテムを持てません");
            }
        }
        else if (hit.collider.CompareTag("Door"))
        {
            OpenCloseAnim door = hit.collider.GetComponentInParent<OpenCloseAnim>();
            if (door != null)
            {
                door.ToggleDoor();
            }
        }
        else if (hit.collider.CompareTag("cap"))
        {
            if (GameManager.instance.isFire)
            {
                SoundManager.instance.PlaySE(clip, source);
            }
            GameManager.instance.isFire = false;
            ExplainObjectInfo explainInfo = hit.collider.gameObject.GetComponent<ExplainObjectInfo>();
            explainInfo.isInfoActive = false;
            Debug.Log("もとせんとじたよ");
        }
        else if (hit.collider.CompareTag("ClearPos"))
        {
            GameManager.instance.EndFrag();
            GameManager.instance.isGameEnd = true;
        }
        else if (hit.collider.CompareTag("memo"))
        {
            Memo memo = hit.collider.GetComponent<Memo>();
            if (memo != null)
            {
                int memonumber = memo.memonumber;
                Debug.Log("memonumber: " + memonumber);
                GameManager.instance.memo = memonumber;
            }
        }
        else if (hit.collider.CompareTag("LightSwitch"))
        {
            if (!GameManager.instance.isFirstBreakerDown)
            {
                LightOnOff lightSwitch = hit.collider.GetComponent<LightOnOff>();
                if (lightSwitch != null)
                {
                    lightSwitch.OnLight();
                }
            }
        }
        else if (hit.collider.CompareTag("Breaker"))
        {
            Breaker breaker = hit.collider.GetComponent<Breaker>();
            if (breaker != null)
            {
                SoundManager.instance.PlaySE(clip, source);
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
        else if (hit.collider.CompareTag("Bag"))
        {
            Bag bag = hit.collider.GetComponent<Bag>();
            if (bag != null)
            {
                bag.OnBagEvent();
                SoundManager.instance.PlaySE(clip, source);
            }
        }
        else if (hit.collider.CompareTag("Step"))
        {
            StepUp stepUp = hit.collider.GetComponent<StepUp>();
            if (stepUp != null)
            {
                stepUp.stepup();
            }
        }
        else if (hit.collider.CompareTag("stepEffect"))
        {
            Step step = hit.collider.GetComponent<Step>();
            if (step != null)
            {
                int stepnumber = step.stepnumber;
                step.CreateAndDestroy();
            }
        }
    }
}
