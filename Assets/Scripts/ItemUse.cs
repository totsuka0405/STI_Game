using UnityEngine;

public class ItemUse : MonoBehaviour
{
    public float rayDistance = 8f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
            }
        }
    }
}
