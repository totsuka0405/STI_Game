using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandItemPhone : MonoBehaviour
{
    void Start()
    {
        GameManager.instance.isHandPhone = true;
        GameManager.instance.isHandItemUse = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDisable()
    {
        GameManager.instance.isHandPhone = false;
        GameManager.instance.isHandItemUse = false;
    }
}
