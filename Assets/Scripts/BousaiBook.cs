using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

public class BousaiBook : MonoBehaviour
{
    [SerializeField] GameObject[] bousaiPanels;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;
    int panelindex = 0;

    private void Start()
    {
        panelindex = 0;
    }

    private void Update()
    {
        if (GameManager.instance.isHandBousaiBook)
        {
            if (Input.GetMouseButtonDown(1) || (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame))
            {
                OnNext();
            }
        }
        else
        {
            foreach (var panel in bousaiPanels)
            {
                panel.SetActive(false);
            }
            panelindex = 0;
        }
    }

    public void OnNext()
    {
        if (bousaiPanels.Length > panelindex)
        {
            bousaiPanels[panelindex].SetActive(true);
            if (panelindex > 0)
            {
                bousaiPanels[panelindex - 1].SetActive(false);
            }
            
            panelindex++;
        }
        else
        {
            foreach(var panel in bousaiPanels)
            {
                panel.SetActive(false);
            }
            panelindex = 0;
        }

        SoundManager.instance.PlaySE(clip, source);
    }
}
