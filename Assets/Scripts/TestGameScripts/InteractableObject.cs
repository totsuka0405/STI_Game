using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] private string interactionPrompt = "Press E to interact"; // ユーザーへのプロンプト
    [SerializeField] private bool isInteractable = true; // インタラクション可能かどうか

    // プレイヤーがインタラクトした際に呼ばれる
    public abstract void Interact();

    // プレイヤーが近づいた際に呼ばれる
    public virtual void OnFocus()
    {
        if (isInteractable)
        {
            Debug.Log(interactionPrompt);
        }
    }

    // プレイヤーが離れた際に呼ばれる
    public virtual void OnLoseFocus()
    {
        Debug.Log("Lost focus on object");
    }

    // インタラクション可能かを設定する
    public void SetInteractable(bool value)
    {
        isInteractable = value;
    }

    public bool GetInteractable()
    {
        return isInteractable;
    }
}
