using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnOff : MonoBehaviour
{
    [SerializeField] GameObject lightObj;
    [SerializeField] AudioClip audioClip;
    [SerializeField] AudioSource source;
    public bool isTurn = false;

    public void OnLightWakeUp()
    {
        if (isTurn)
        {
            lightObj.SetActive(true);
        }
        else
        {
            lightObj.SetActive(false);
        }
    }

    public void OnLight()
    {
        if (GameManager.instance.isBreakerDown == false)
        {
            if (isTurn)
            {
                lightObj.SetActive(false);
            }
            else
            {
                lightObj.SetActive(true);
            }
            SoundManager.instance.PlaySE(audioClip, source);
            isTurn = !isTurn;
        }   
    }
    
}
