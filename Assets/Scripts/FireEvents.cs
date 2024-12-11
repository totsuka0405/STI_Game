using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEvents : MonoBehaviour
{
    public GameObject[] fires;

    public void FireUp(int count)
    {
        fires[count].SetActive(true);
    }

    public void FireInActive()
    {
        foreach (GameObject obj in fires)
        {
            if (obj != null) // 念のためnullチェック
            {
                obj.SetActive(false);
            }
        }
    }
}
