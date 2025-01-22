using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldFire : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;
    [SerializeField] AudioSource source;
    bool isFire = true;

    private void Update()
    {
        if(GameManager.instance.isFire && isFire)
        {
            SoundManager.instance.PlayLoopSE(audioClip, source);
            isFire = false;
        }
    }
}
