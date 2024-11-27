using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDoor : InteractableObject
{
    private bool isOpen = false; // ドアの状態

    public override void Interact()
    {
        if (GetInteractable())
        {
            isOpen = !isOpen;
            Debug.Log(isOpen ? "Door is now open" : "Door is now closed");
            // 実際のアニメーションや動作を追加
            transform.Rotate(Vector3.up, isOpen ? 90f : -90f); // ドアを回転
        }
    }

    public override void OnFocus()
    {
        base.OnFocus();
        Debug.Log("Looking at a door");
    }

    public override void OnLoseFocus()
    {
        base.OnLoseFocus();
        Debug.Log("Stopped looking at the door");
    }
}
