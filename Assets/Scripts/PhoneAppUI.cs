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
                    appCallPanels.SetActive(false);
                    appMapPanels.SetActive(false);
                    CharacterMove.instance.isCursolLock = true;
                }
                else
                {
                    phoneUI.SetActive(true);
                    CharacterMove.instance.isCursolLock = false;
                }
                
            }
        }
        else
        {
            appCallPanels.SetActive(false);
            appMapPanels.SetActive(false);
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
}
