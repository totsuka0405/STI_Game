using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandItemPhone : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField] AudioSource source;
    bool isCallEventEnd = false;
     
    void Start()
    {
        GameManager.instance.isHandPhone = true;
        GameManager.instance.isHandItemUse = true;
        if (GameManager.instance.isCallPhone)
        {
            isCallEventEnd = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.isCallPhone && !isCallEventEnd)
        {
            isCallEventEnd = true;
            SoundManager.instance.PlaySE(clip, source);
        }
    }

    private void OnDisable()
    {
        GameManager.instance.isHandPhone = false;
        GameManager.instance.isHandItemUse = false;
    }
}
