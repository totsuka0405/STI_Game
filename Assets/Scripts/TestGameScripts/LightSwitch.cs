using UnityEngine;

public class LightSwitch : InteractableObject
{
    [SerializeField] private Light targetLight; // 操作するライト

    public override void Interact()
    {
        if (GetInteractable())
        {
            if (targetLight != null)
            {
                targetLight.enabled = !targetLight.enabled; // ライトのオン/オフを切り替え
                Debug.Log(targetLight.enabled ? "Light is now ON" : "Light is now OFF");
            }
        }
    }

    public override void OnFocus()
    {
        base.OnFocus();
        Debug.Log("Looking at a light switch");
    }

    public override void OnLoseFocus()
    {
        base.OnLoseFocus();
        Debug.Log("Stopped looking at the light switch");
    }
}
