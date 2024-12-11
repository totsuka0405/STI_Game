using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breaker : MonoBehaviour
{
    [SerializeField] GameObject[] lightObjs;
    [SerializeField] GameObject[] switchObj;


    public void SetLightWakeUp()
    {
        foreach(GameObject obj in switchObj)
        {
            if(obj != null)
            {
                LightOnOff lightOnOff = obj.GetComponentInChildren<LightOnOff>();
                lightOnOff.OnLightWakeUp();
            }
        }
    }

    public void SetAllObjectsInactive()
    {
        foreach (GameObject obj in lightObjs)
        {
            if (obj != null) // 念のためnullチェック
            {
                obj.SetActive(false);
            }
        }
    }
}
