using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 家具（Tag: "Kagu"）に対して耐震ストッパーを適用するアイテム。
/// 家具の Rigidbody を削除し、StopperObjectActive を有効化する。
/// </summary>
public class Stopper : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private bool isStopperUse = true; // 使用可能フラグ
    [SerializeField] private float rayDistance = 8f;   // レイの有効距離

    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (!isStopperUse) return;

        var pad = Gamepad.current;
        bool trigger =
            Input.GetMouseButtonDown(1) ||
            (pad != null && pad.buttonSouth.wasPressedThisFrame);

        if (!trigger) return;

        Camera cam = mainCam != null ? mainCam : Camera.main;
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            HandleHit(hit);
        }
    }

    /// <summary>
    /// 家具に命中した場合の処理。
    /// </summary>
    private void HandleHit(RaycastHit hit)
    {
        if (!hit.collider.CompareTag("Kagu")) return;

        if (hit.collider.TryGetComponent<Rigidbody>(out var rb))
        {
            Destroy(rb);
            Debug.Log("Rigidbody destroyed for object with 'Kagu' tag.");
        }

        if (hit.collider.TryGetComponent<StopperObjectActive>(out var stopper))
        {
            stopper.ActivateStopper(); // isStopperActive を直接触らず公開メソッドで有効化
        }
    }

    public bool GetIsStopperUse() => isStopperUse;
    public void SetIsStopperUse(bool value) => isStopperUse = value;
}
