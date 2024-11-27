using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] float rayDistance = 8f;
    private RaycastHit hit;

    public void Interact()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                //interactable.OnInteract();
            }
        }

        

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
                Door door = hit.collider.GetComponentInParent<Door>();
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

        }
    }

}
