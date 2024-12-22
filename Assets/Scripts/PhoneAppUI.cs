using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhoneAppUI : MonoBehaviour
{
    [SerializeField] GameObject phoneUI;
    [SerializeField] GameObject appCallPanels;
    [SerializeField] GameObject appMapPanels;

    private void Start()
    {
       
    }

    private void Update()
    {
        if (GameManager.instance.isHandPhone)
        {
            if (Input.GetMouseButtonDown(1) || (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame))
            {
                if (phoneUI.activeSelf)
                {
                    phoneUI.SetActive(false);
                    CharacterMove.instance.isCursolLock = !CharacterMove.instance.isCursolLock;
                }
                else
                {
                    phoneUI.SetActive(true);
                    CharacterMove.instance.isCursolLock = !CharacterMove.instance.isCursolLock;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                
            }
        }
        else
        {
            phoneUI.SetActive(false);
        }
    }

    public void OnMap()
    {
        appMapPanels.SetActive(true);
        GameManager.instance.isMapWatch = true;
    }

    public void OnCall()
    {
        appCallPanels.SetActive(true);
        GameManager.instance.isCallPhone = true;
    }

    private void OnDisable()
    {
        appCallPanels.SetActive(false);
        appMapPanels.SetActive(false);
        phoneUI.SetActive(false);
    }
}
