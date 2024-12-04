using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnOff : MonoBehaviour
{
    [SerializeField] GameObject lightObj;
    [SerializeField] bool isTurn = false;

    public void OnLight()
    {
        if (isTurn)
        {
            lightObj.SetActive(false);
        }
        else
        {
            lightObj.SetActive(true);
        }

        isTurn = !isTurn;
    }
    
}
