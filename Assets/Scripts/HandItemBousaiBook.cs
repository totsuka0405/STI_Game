using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandItemBousaiBook : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.isHandBousaiBook = true;
        GameManager.instance.isHandItemUse = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        GameManager.instance.isHandBousaiBook = false;
        GameManager.instance.isHandItemUse = false;
    }
}
