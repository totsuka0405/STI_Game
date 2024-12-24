using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldFirstAidKit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.isHaveFirstAidKit = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        GameManager.instance.isHaveFirstAidKit = true;
    }
}
