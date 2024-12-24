using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldHandLight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.isHaveHandLight = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        GameManager.instance.isHaveHandLight = true;
    }
}
