using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandLight : MonoBehaviour
{
    [SerializeField] GameObject lightObj;
    private bool isLight = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame))
        {
            if (isLight)
            {
                lightObj.SetActive(false);
                isLight = false;
            }
            else if (!isLight)
            {
                lightObj.SetActive(true);
                isLight = true;
            }
        }
    }


}
