using UnityEngine;

public class Drawer : InteractableObject
{
    private bool isOpen = false; // 引き出しの状態

    public override void Interact()
    {
        if (GetInteractable())
        {
            isOpen = !isOpen;
            Debug.Log(isOpen ? "Drawer is now open" : "Drawer is now closed");
            // 実際のアニメーションや動作を追加
            float moveDistance = isOpen ? 0.5f : -0.5f; // 引き出しの移動距離
            transform.Translate(Vector3.forward * moveDistance);
        }
    }

    public override void OnFocus()
    {
        base.OnFocus();
        Debug.Log("Looking at a drawer");
    }

    public override void OnLoseFocus()
    {
        base.OnLoseFocus();
        Debug.Log("Stopped looking at the drawer");
    }
}
