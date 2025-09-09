using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpObj : MonoBehaviour
{
    public bool isUpObj = false;
    private void OnTriggerEnter(Collider other)
    {
        isUpObj = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isUpObj = false;
    }
}
