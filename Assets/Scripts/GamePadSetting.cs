using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePadSetting : MonoBehaviour
{
    public GameObject virtualObject;

    void Update()
    {
        CheckGamepadConnection();
        
    }

    void CheckGamepadConnection()
    {
        var gamepad = Gamepad.current;

        if (gamepad != null)
        {
            virtualObject.SetActive(true);
        }
        else
        {
            virtualObject.SetActive(false);
        }
    }
}
