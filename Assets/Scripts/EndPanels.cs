using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPanels : MonoBehaviour
{
    [SerializeField] GameObject[] panels;
    int panelindex = 0;

    private void Start()
    {
        panelindex = 0;
        panels[panelindex].SetActive(true);
        panelindex++;
    }

    public void OnNext()
    {
        if(panels.Length > panelindex)
        {
            panels[panelindex].SetActive(true);
            panelindex++;
        }
    }
}
