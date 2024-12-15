using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopperObjectActive : MonoBehaviour
{
    [SerializeField] GameObject[] stopperObjects;
    public bool isStopperActive = false;

    void Start()
    {
        foreach (GameObject obj in stopperObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    public void StopperActive()
    {
        if (isStopperActive)
        {
            foreach (GameObject obj in stopperObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
        }
    }
}
