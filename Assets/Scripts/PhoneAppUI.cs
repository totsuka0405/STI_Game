using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PhoneAppUI : MonoBehaviour
{
    [SerializeField] GameObject phoneUI;
    [SerializeField] GameObject appCallPanels;
    [SerializeField] GameObject appMapPanels;
    [SerializeField] GameObject[] apps;
    [SerializeField] AudioClip clip;
    [SerializeField] AudioClip clip2;
    [SerializeField] AudioSource audioSource;

    int currentSelectIndex = 0;
    int gridWidth = 3;

    private void Start()
    {
        currentSelectIndex = 5;
        GameObject selectedApp = apps[currentSelectIndex];
        Transform selectedFrame = selectedApp.transform.Find("Frame");
        if (selectedFrame != null)
        {
            selectedFrame.gameObject.SetActive(true);
        }

        UpdateSelection();
    }

    private void Update()
    {
        if (GameManager.instance.isHandPhone)
        {
            if (Input.GetMouseButtonDown(1) || (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame))
            {
                if (phoneUI.activeSelf)
                {
                    phoneUI.SetActive(false);
                    appCallPanels.SetActive(false);
                    appMapPanels.SetActive(false);
                }
                else
                {
                    phoneUI.SetActive(true);
                }

            }

            if (phoneUI.activeSelf == true)
            {
                UISelectInput();

                if (Input.GetMouseButtonDown(0) || (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame))
                {
                    SelectOnApp();
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
        if (appMapPanels.activeSelf)
        {
            appMapPanels.SetActive(false);
        }
        else
        {
            appMapPanels.SetActive(true);
        }
        GameManager.instance.isMapWatch = true;
    }

    public void OnCall()
    {
        if (appCallPanels.activeSelf)
        {
            appCallPanels.SetActive(false);
        }
        else
        {
            appCallPanels.SetActive(true);
        }
        GameManager.instance.isCallPhone = true;
    }

    void UISelectInput()
    {
        bool selectionChanged = false;

        if (Input.GetKeyDown(KeyCode.UpArrow) || (Gamepad.current != null && Gamepad.current.dpad.up.wasPressedThisFrame))
        {
            if(currentSelectIndex >= gridWidth)
            {
                currentSelectIndex -= gridWidth;
                selectionChanged = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || (Gamepad.current != null && Gamepad.current.dpad.down.wasPressedThisFrame))
        {
            if (currentSelectIndex + gridWidth < apps.Length)
            {
                currentSelectIndex += gridWidth;
                selectionChanged = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || (Gamepad.current != null && Gamepad.current.dpad.left.wasPressedThisFrame))
        {
            if (currentSelectIndex % gridWidth > 0)
            {
                currentSelectIndex -= 1;
                selectionChanged = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || (Gamepad.current != null && Gamepad.current.dpad.right.wasPressedThisFrame))
        {
            if (currentSelectIndex % gridWidth < gridWidth - 1 && currentSelectIndex + 1 < apps.Length) // 右の列が存在する場合
            {
                currentSelectIndex += 1;
                selectionChanged = true;
            }
        }

        if (selectionChanged)
        {
            UpdateSelection();
            SoundManager.instance.PlaySE(clip, audioSource);
        }
    }

    void UpdateSelection()
    {
        // すべてのAPPのFrameを一度trueにする
        foreach (GameObject app in apps)
        {
            Transform frame = app.transform.Find("Frame");
            if (frame != null)
            {
                frame.gameObject.SetActive(false);
            }
        }

        // 現在選択中のAPPのFrameをfalseにする
        GameObject selectedApp = apps[currentSelectIndex];
        Transform selectedFrame = selectedApp.transform.Find("Frame");
        if (selectedFrame != null)
        {
            selectedFrame.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// アプリを呼び出す際の入力処理
    /// </summary>
    void SelectOnApp()
    {
        SoundManager.instance.PlaySE(clip2, audioSource);
        if (currentSelectIndex == 0)
        {
            OnMap();
        }
        else if (currentSelectIndex == 1)
        {
            OnCall();
        }
    }

    private void OnDisable()
    {
        appCallPanels.SetActive(false);
        appMapPanels.SetActive(false);
        phoneUI.SetActive(false);
    }
}
