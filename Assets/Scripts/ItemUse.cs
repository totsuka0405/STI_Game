using UnityEngine;
using UnityEngine.InputSystem;

public class ItemUse : MonoBehaviour
{
    [SerializeField] float rayDistance = 8f;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                if (hit.collider.CompareTag("Item"))
                {
                    // 子オブジェクトのColliderを持つ親のPickupObjスクリプトを取得
                    PickupObj pickupObj = hit.collider.GetComponentInParent<PickupObj>();
                    if (pickupObj != null)
                    {
                        pickupObj.OnClickObj();
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
                    GameManager.instance.isFire = false;
                    Debug.Log("もとせんとじたよ");
                }
                else if (hit.collider.CompareTag("ClearPos"))
                {
                    GameManager.instance.isGameClear = true;
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
                    LightOnOff lightSwitch = hit.collider.GetComponent<LightOnOff>();
                    if (lightSwitch != null)
                    {
                        lightSwitch.OnLight();
                    }
                }

            }
        }
    }
}
